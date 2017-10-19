using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LondonMovingSouth.CatalogService
{
    public static class DbInitializer
    {
        public static void Initialize(CatalogContext context)
        {
            context.Database.EnsureCreated();

            if (context.Products.Any())
            {
                return;
            }


            const int numberOfProducts = 10;
            var numOfConditions = Enum.GetValues(typeof(Condition)).Cast<int>().Last();

            for (var i = 0; i < numberOfProducts; i++)
            {
                context.Products.Add(new Product
                {
                    Id = i,
                    DetailId = i,
                    Category = (Category)new Random().Next(1, 10),
                    Summary = "summary" + i,
                    Price = new Random().Next(1, 100),
                    CreatedDate = DateTime.UtcNow.ToString("g"),
                    ModifiedDate = DateTime.UtcNow.ToString("g")
                });

                context.Details.Add(new Details
                {
                    Id = i,
                    Brand = "brand" + i,
                    Description = "Detailed paragraph lorem ipsum blabla \n" +
                                    "blablablablablablablablablablablabla\n" +
                                    "blablablablablablablablablablablabla\n" +
                                    "blablablablablablablablablablablabla" + i,
                    Dimensions = new Random().Next(1, 50) + "X" + new Random().Next(1, 50) + "X" + new Random().Next(1, 50),
                    Weight = new Random().Next(1, 5000) + "gm", 
                    Colour = "color" + i,
                    Quantity = new Random().Next(1,50),
                    Condition = (Condition)new Random().Next(1, numOfConditions)
                });
            }

            context.SaveChanges();
        }
    }
}
