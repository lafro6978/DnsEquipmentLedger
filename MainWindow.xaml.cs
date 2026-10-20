using System.Windows;
using DnsEquipmentLedger.ViewModels;

namespace DnsEquipmentLedger
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainViewModel();
        }
    }
}