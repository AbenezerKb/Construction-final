﻿using ERP.Context;
using Microsoft.EntityFrameworkCore;

namespace Construction.Context
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProd)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProd);
            }
        }

        private static void SeedData(AppDbContext context, bool isProd)
        {
            if (isProd)
            {
                Console.WriteLine("--> Attempting to apply migrations...");
                try
                {
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not run migrations: {ex.Message}");
                }
            }

            if (!context.Contracts.Any())
            {
               // Console.WriteLine("--> Seeding Data...");

               // context.Contracts.AddRange(
               //     new Contracts() { Name = "Dot Net", Publisher = "Microsoft", Cost = "Free" },
               //     new Platform() { Name = "SQL Server Express", Publisher = "Microsoft", Cost = "Free" },
               //     new Platform() { Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free" }
               // );

               // context.SaveChanges();
            }
            else
            {
                //Console.WriteLine("--> We already have data");
            }
        }
    }
}
