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
            var audit = new Audit();
            audit.CreatedBy = UserName;

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
    }
}
