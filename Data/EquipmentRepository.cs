using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DnsEquipmentLedger.Models;

namespace DnsEquipmentLedger.Data
{
    /// <summary>
    /// Шаблон проектирования Repository (Репозиторий) для работы с БД учёта техники
    /// </summary>
    public class EquipmentRepository
    {
        public List<EquipmentItem> GetAllEquipment()
        {
            using var db = new AppDbContext();
            return db.EquipmentItems
                .Include(e => e.Category)
                .Include(e => e.Manufacturer)
                .Include(e => e.Location)
                .Include(e => e.ResponsibleEmployee)
                .OrderBy(e => e.InventoryNumber)
                .ToList();
        }

        public List<Category> GetAllCategories()
        {
            using var db = new AppDbContext();
            return db.Categories.OrderBy(c => c.Name).ToList();
        }

        public void AddEquipment(EquipmentItem newItem)
        {
            using var db = new AppDbContext();
            db.EquipmentItems.Add(newItem);
            db.SaveChanges();

            var movement = new EquipmentMovement
            {
                EquipmentItemId = newItem.EquipmentItemId,
                FromLocationId = null,
                ToLocationId = newItem.LocationId,
                EmployeeId = newItem.ResponsibleEmployeeId,
                OperationType = "Первичная приёмка",
                Comment = $"Постановка на учёт: {newItem.ModelName}"
            };

            db.EquipmentMovements.Add(movement);
            db.SaveChanges();
        }
    }
}