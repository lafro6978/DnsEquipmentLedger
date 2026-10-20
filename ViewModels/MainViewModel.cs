using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using DnsEquipmentLedger.Data;
using DnsEquipmentLedger.Models;

namespace DnsEquipmentLedger.ViewModels
{
    /// <summary>
    /// Шаблон проектирования Command (Команда) для привязки кнопок в WPF MVVM
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Func<object?, bool>? _canExecute;

        public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter) => _canExecute == null || _canExecute(parameter);
        public void Execute(object? parameter) => _execute(parameter);

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }

    /// <summary>
    /// Главная ViewModel приложения (Архитектура MVVM)
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly EquipmentRepository _repository = new EquipmentRepository();
        private List<EquipmentItem> _allEquipment = new List<EquipmentItem>();

        // Коллекция для отображения в таблице DataGrid
        public ObservableCollection<EquipmentItem> FilteredEquipment { get; set; } = new ObservableCollection<EquipmentItem>();
        public ObservableCollection<Category> Categories { get; set; } = new ObservableCollection<Category>();

        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                ApplyFilter(); // Мгновенная фильтрация при вводе текста
            }
        }

        private Category? _selectedCategory;
        public Category? SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged();
                ApplyFilter(); // Мгновенная фильтрация при выборе категории
            }
        }

        private EquipmentItem? _selectedEquipment;
        public EquipmentItem? SelectedEquipment
        {
            get => _selectedEquipment;
            set
            {
                _selectedEquipment = value;
                OnPropertyChanged();
            }
        }

        // Команды для кнопок интерфейса
        public ICommand ResetFilterCommand { get; }
        public ICommand RefreshCommand { get; }

        public MainViewModel()
        {
            ResetFilterCommand = new RelayCommand(_ => ResetFilter());
            RefreshCommand = new RelayCommand(_ => LoadData());

            LoadData();
        }

        public void LoadData()
        {
            _allEquipment = _repository.GetAllEquipment();

            Categories.Clear();
            Categories.Add(new Category { CategoryId = 0, Name = "Все категории" });
            foreach (var cat in _repository.GetAllCategories())
            {
                Categories.Add(cat);
            }

            SelectedCategory = Categories.FirstOrDefault();
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            var query = _allEquipment.AsEnumerable();

            if (SelectedCategory != null && SelectedCategory.CategoryId != 0)
            {
                query = query.Where(e => e.CategoryId == SelectedCategory.CategoryId);
            }

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                string lower = SearchText.Trim().ToLower();
                query = query.Where(e =>
                    e.InventoryNumber.ToLower().Contains(lower) ||
                    e.ModelName.ToLower().Contains(lower) ||
                    e.SerialNumber.ToLower().Contains(lower) ||
                    e.ResponsibleEmployee.FullName.ToLower().Contains(lower));
            }

            FilteredEquipment.Clear();
            foreach (var item in query)
            {
                FilteredEquipment.Add(item);
            }
        }

        private void ResetFilter()
        {
            SearchText = string.Empty;
            SelectedCategory = Categories.FirstOrDefault();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}