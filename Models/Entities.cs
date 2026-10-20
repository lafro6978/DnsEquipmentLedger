using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DnsEquipmentLedger.Models
{
    /// <summary>
    /// 1. Таблица «Категории техники» (Categories)
    /// </summary>
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(250)]
        public string? Description { get; set; }

        public ICollection<EquipmentItem> EquipmentItems { get; set; } = new List<EquipmentItem>();
    }

    /// <summary>
    /// 2. Таблица «Производители / Бренды» (Manufacturers)
    /// </summary>
    public class Manufacturer
    {
        [Key]
        public int ManufacturerId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(150)]
        public string? SupportContact { get; set; }

        public ICollection<EquipmentItem> EquipmentItems { get; set; } = new List<EquipmentItem>();
    }

    /// <summary>
    /// 3. Таблица «Сотрудники / Материально ответственные лица» (Employees)
    /// </summary>
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }

        [Required]
        [MaxLength(20)]
        public string TabNumber { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Position { get; set; } = string.Empty;

        [MaxLength(20)]
        [Column(TypeName = "varchar(20)")]
        public string? Phone { get; set; }

        public ICollection<EquipmentItem> AssignedEquipment { get; set; } = new List<EquipmentItem>();
        public ICollection<EquipmentMovement> RegisteredMovements { get; set; } = new List<EquipmentMovement>();
    }

    /// <summary>
    /// 4. Таблица «Локации / Помещения филиала "ДНС"» (Locations)
    /// </summary>
    public class Location
    {
        [Key]
        public int LocationId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string ZoneType { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? RoomNumber { get; set; }

        public ICollection<EquipmentItem> EquipmentItems { get; set; } = new List<EquipmentItem>();
    }

    /// <summary>
    /// 5. Таблица «Единицы техники» (EquipmentItems) — Центральная таблица
    /// </summary>
    public class EquipmentItem
    {
        [Key]
        public int EquipmentItemId { get; set; }

        // FK
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        public int ManufacturerId { get; set; }
        public Manufacturer Manufacturer { get; set; } = null!;

        public int LocationId { get; set; }
        public Location Location { get; set; } = null!;

        public int ResponsibleEmployeeId { get; set; }
        public Employee ResponsibleEmployee { get; set; } = null!;

        // Основные атрибуты
        [Required]
        [MaxLength(50)]
        public string InventoryNumber { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [Column(TypeName = "varchar(100)")]
        public string SerialNumber { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string ModelName { get; set; } = string.Empty;

        [Column(TypeName = "date")]
        public DateTime PurchaseDate { get; set; } = DateTime.Today;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Cost { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "В эксплуатации";

        // История всех движений данной единицы техники
        public ICollection<EquipmentMovement> Movements { get; set; } = new List<EquipmentMovement>();
    }

    /// <summary>
    /// 6. Таблица «Журнал учёта движений техники» (EquipmentMovements)
    /// </summary>
    public class EquipmentMovement
    {
        [Key]
        public int EquipmentMovementId { get; set; }

        // FK
        public int EquipmentItemId { get; set; }
        public EquipmentItem EquipmentItem { get; set; } = null!;

        public int? FromLocationId { get; set; }
        public Location? FromLocation { get; set; }

        public int ToLocationId { get; set; } 
        public Location ToLocation { get; set; } = null!;

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;

        // Атрибуты операции
        public DateTime MovementDate { get; set; } = DateTime.Now;

        [Required]
        [MaxLength(50)]
        public string OperationType { get; set; } = string.Empty;

        [MaxLength(300)]
        public string? Comment { get; set; }
    }
}