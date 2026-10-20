using System;
using Microsoft.EntityFrameworkCore;
using DnsEquipmentLedger.Models;

namespace DnsEquipmentLedger.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<EquipmentItem> EquipmentItems { get; set; }
        public DbSet<EquipmentMovement> EquipmentMovements { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=dns_equipment_ledger.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EquipmentItem>()
                .HasIndex(e => e.InventoryNumber)
                .IsUnique();

            modelBuilder.Entity<Employee>()
                .HasIndex(emp => emp.TabNumber)
                .IsUnique();

            modelBuilder.Entity<EquipmentItem>()
                .HasOne(e => e.ResponsibleEmployee)
                .WithMany(emp => emp.AssignedEquipment)
                .HasForeignKey(e => e.ResponsibleEmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EquipmentMovement>()
                .HasOne(m => m.FromLocation)
                .WithMany()
                .HasForeignKey(m => m.FromLocationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EquipmentMovement>()
                .HasOne(m => m.ToLocation)
                .WithMany()
                .HasForeignKey(m => m.ToLocationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Торговое и кассовое оборудование", Description = "ТСД, сканеры штрих-кодов, онлайн-кассы" },
                new Category { CategoryId = 2, Name = "Компьютеры и ноутбуки", Description = "Рабочие станции продавцов-консультантов и управляющего" },
                new Category { CategoryId = 3, Name = "Сетевое и серверное оборудование", Description = "Маршрутизаторы, коммутаторы, точки доступа Wi-Fi" }
            );

            modelBuilder.Entity<Manufacturer>().HasData(
                new Manufacturer { ManufacturerId = 1, Name = "DEXP", SupportContact = "support@dexp.club" },
                new Manufacturer { ManufacturerId = 2, Name = "ARDOR GAMING", SupportContact = "service@ardor-gaming.com" },
                new Manufacturer { ManufacturerId = 3, Name = "Honeywell", SupportContact = "8-800-100-20-30" },
                new Manufacturer { ManufacturerId = 4, Name = "АТОЛ", SupportContact = "support@atol.ru" }
            );

            modelBuilder.Entity<Location>().HasData(
                new Location { LocationId = 1, Name = "Основной склад филиала", ZoneType = "Складская зона", RoomNumber = "П-101" },
                new Location { LocationId = 2, Name = "Торговый зал (Зона выдачи и касс)", ZoneType = "Торговая зона", RoomNumber = "ТЗ-1" },
                new Location { LocationId = 3, Name = "Сервисный центр / Приёмка по гарантии", ZoneType = "Сервисная зона", RoomNumber = "СЦ-2" }
            );

            modelBuilder.Entity<Employee>().HasData(
                new Employee { EmployeeId = 1, TabNumber = "DNS-00142", FullName = "Смирнов Алексей Викторович", Position = "Управляющий магазином", Phone = "+7(913)555-12-34" },
                new Employee { EmployeeId = 2, TabNumber = "DNS-00289", FullName = "Козлов Дмитрий Игоревич", Position = "Старший кладовщик", Phone = "+7(913)555-56-78" },
                new Employee { EmployeeId = 3, TabNumber = "DNS-00415", FullName = "Орлова Екатерина Сергеевна", Position = "Специалист сервисного центра", Phone = "+7(913)555-90-12" }
            );

            modelBuilder.Entity<EquipmentItem>().HasData(
                new EquipmentItem
                {
                    EquipmentItemId = 1,
                    CategoryId = 1,
                    ManufacturerId = 3,
                    LocationId = 1,
                    ResponsibleEmployeeId = 2,
                    InventoryNumber = "INV-DNS-2026-0001",
                    SerialNumber = "HW-EDA51-994812",
                    ModelName = "Терминал сбора данных Honeywell ScanPal EDA51",
                    PurchaseDate = new DateTime(2026, 2, 10),
                    Cost = 42500.00m,
                    Status = "В эксплуатации"
                },
                new EquipmentItem
                {
                    EquipmentItemId = 2,
                    CategoryId = 2,
                    ManufacturerId = 1,
                    LocationId = 2,
                    ResponsibleEmployeeId = 1,
                    InventoryNumber = "INV-DNS-2026-0002",
                    SerialNumber = "DEXP-ATLAS-773120",
                    ModelName = "Ноутбук DEXP Atlas H15 (Пост проверки товара)",
                    PurchaseDate = new DateTime(2026, 3, 15),
                    Cost = 38990.00m,
                    Status = "В эксплуатации"
                }
            );

            modelBuilder.Entity<EquipmentMovement>().HasData(
                new EquipmentMovement
                {
                    EquipmentMovementId = 1,
                    EquipmentItemId = 1,
                    FromLocationId = null,
                    ToLocationId = 1,
                    EmployeeId = 2,
                    MovementDate = new DateTime(2026, 2, 10, 10, 30, 0),
                    OperationType = "Первичная приёмка",
                    Comment = "Постановка ТСД на учёт на основной склад филиала"
                },
                new EquipmentMovement
                {
                    EquipmentMovementId = 2,
                    EquipmentItemId = 2,
                    FromLocationId = 1,
                    ToLocationId = 2,
                    EmployeeId = 1,
                    MovementDate = new DateTime(2026, 3, 16, 14, 15, 0),
                    OperationType = "Перемещение",
                    Comment = "Выдача ноутбука со склада в торговый зал на стойку проверки техники"
                }
            );
        }
    }
}