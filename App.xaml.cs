using System.Windows;
using Microsoft.EntityFrameworkCore;
using DnsEquipmentLedger.Data;

namespace DnsEquipmentLedger
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            using (var db = new AppDbContext())
            {
                db.Database.Migrate();
            }
        }
    }
}