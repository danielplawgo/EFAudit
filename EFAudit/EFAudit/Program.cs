using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace EFAudit
{
    class Program
    {
        static void Main(string[] args)
        {
            int id = AddTest();
            EditTest();
            SoftDeleteTest();
            DeleteTest();
            ShowHistory(id);
        }

        static int AddTest()
        {
            using (DataContext db = new DataContext())
            {
                var category = db.Categories.FirstOrDefault();

                var product = new Product()
                {
                    Name = "Product Name",
                    Category = category,
                    Description = "Product Description",
                    Price = 10.23M
                };

                db.Products.Add(product);

                db.SaveChanges();

                return product.Id;
            }
        }

        static void EditTest()
        {
            using (DataContext db = new DataContext())
            {
                var category = db.Categories
                    .OrderByDescending(c => c.Id)
                    .FirstOrDefault();

                var product = db.Products
                    .OrderByDescending(c => c.Id)
                    .FirstOrDefault();

                product.Category = category;
                product.Name = "New Product Name";

                db.SaveChanges();
            }
        }

        static void SoftDeleteTest()
        {
            using (DataContext db = new DataContext())
            {
                var product = db.Products
                    .OrderByDescending(c => c.Id)
                    .FirstOrDefault();

                product.IsActive = false;

                db.SaveChanges();
            }
        }

        static void DeleteTest()
        {
            using (DataContext db = new DataContext())
            {
                var product = db.Products
                    .OrderByDescending(c => c.Id)
                    .FirstOrDefault();

                db.Products.Remove(product);

                db.SaveChanges();
            }
        }

        static void ShowHistory(int id)
        {
            using (DataContext db = new DataContext())
            {
                var histories = db.AuditEntries.Where<Product>(id);

                foreach (var history in histories)
                {
                    Console.WriteLine($"State: {history.State}");

                    foreach (var property in history.Properties)
                    {
                        Console.WriteLine($"Propepty: {property.PropertyName}, OldValue: {property.OldValue}, NewValue: {property.NewValue}");
                    }

                    Console.WriteLine();
                }
            }
        }
    }
}
