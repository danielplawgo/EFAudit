using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace EFAudit
{
    public class DataContext : DbContext
    {
        static DataContext()
        {
            AuditManager.DefaultConfiguration.AutoSavePreAction = (context, audit) =>
               (context as DataContext).AuditEntries.AddRange(audit.Entries);
        }

        public DataContext()
            : base("Name=DefaultConnection")
        {
            
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<AuditEntry> AuditEntries { get; set; }

        public DbSet<AuditEntryProperty> AuditEntryProperties { get; set; }

        public string UserName { get; set; } = "System";

        public override int SaveChanges()
        {
            var audit = ConfigureAudit();

            audit.PreSaveChanges(this);
            var rowAffecteds = base.SaveChanges();
            audit.PostSaveChanges();

            if (audit.Configuration.AutoSavePreAction != null)
            {
                audit.Configuration.AutoSavePreAction(this, audit);
                base.SaveChanges();
            }

            return rowAffecteds;
        }

        private Audit ConfigureAudit()
        {
            var audit = new Audit();
            audit.CreatedBy = UserName;

            //audit.Configuration.Exclude(o => true);
            //audit.Configuration.Include<Product>();
            //audit.Configuration.Include(o => o is Product && ((Product)o).Name == "Product Name");

            //audit.Configuration.Exclude<Product>();
            //audit.Configuration.Exclude(o => o is Product && ((Product)o).Name == "Product Name");

            //audit.Configuration.ExcludeProperty<BaseModel>(t => t.IsActive);

            //audit.Configuration.ExcludeProperty<Product>();
            //audit.Configuration.IncludeProperty<Product>(t => t.Name);

            //audit.Configuration.Format<Product>(x => x.Price, x => ((decimal)x).ToString("0.00 zl"));

            //audit.Configuration.IgnorePropertyUnchanged = false;

            //audit.Configuration.SoftDeleted<BaseModel>(x => x.IsActive == false);

            return audit;
        }
    }
}
