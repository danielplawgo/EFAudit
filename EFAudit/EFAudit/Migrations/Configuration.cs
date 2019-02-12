namespace EFAudit.Migrations
{
    using Bogus;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<EFAudit.DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(EFAudit.DataContext context)
        {
            if(context.Categories.Any() == false)
            {
                var categories = new Faker<Category>()
                    .RuleFor(c => c.Name, (f, c) => f.Commerce.Categories(1)[0])
                    .Generate(10);

                context.Categories.AddRange(categories);

                context.SaveChanges();
            }

            if (context.Products.Any() == false)
            {
                var categories = context.Categories.ToList();

                var products = new Faker<Product>()
                    .RuleFor(c => c.Name, (f, c) => f.Commerce.ProductName())
                    .RuleFor(c => c.Category, (f, c) => f.PickRandom(categories))
                    .Generate(10);

                context.Products.AddRange(products);

                context.SaveChanges();
            }
        }
    }
}
