using ClinicalWebApplication.HelpingClasses;
using ClinicalWebApplication.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicalWebApplication.DbSeed
{
    public class AppDbInitializer
    {
        public static void DbSeed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

                context.Database.EnsureCreated();

                if (!context.Users.Any())
                {
                    User obj = new User()
                    {
                        FirstName = "Uzair",
                        LastName = "Aslam",
                        Email = "uzair.aslam02@gmail.com",
                        Password = StringCipher.Encrypt("123"),
                        Role = 1,
                        IsActive = 1,
                        CreatedAt = GeneralPurpose.DateTimeNow(),
                    };


                    context.Users.Add(obj);

                    context.SaveChanges();
                }

                

            }

        }

    }
}
