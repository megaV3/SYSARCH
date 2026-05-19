using Microsoft.EntityFrameworkCore;
using Villarin_SYSARCH.Models;

namespace Villarin_SYSARCH.Data
{
    public static class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            AppDbContext context = app.ApplicationServices
            .CreateScope().ServiceProvider
            .GetRequiredService<AppDbContext>();
            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
            if (!context.Accounts.Any())
            {
                context.Accounts.AddRange(
                    new Account
                    {
                        Id = 123,
                        FirstName = "John",
                        LastName = "Doe",
                        MiddleName = "Admin",
                        CourseLevel = "4",
                        Course = "BSCS",
                        Email = "admin@gmail.com",
                        Address = "Admin Street",
                        Password = "admin"
                    },
                    new Account
                    {
                        Id = 12,
                        FirstName = "John Harriford",
                        LastName = "Villarin",
                        MiddleName = "Middle",
                        CourseLevel = "3",
                        Course = "BSIT",
                        Email = "johnhvillarin@gmail.com",
                        Address = "Cebu City",
                        Password = "student"
                    },
                    new Account
                    {
                        Id = 444,
                        FirstName = "Jane Doe",
                        LastName = "Doe",
                        MiddleName = "ASDASd",
                        CourseLevel = "3",
                        Course = "BSIT",
                        Email = "janedoe@gmail.com",
                        Address = "Guadalupe",
                        Password = "student"
                    }
                );
                context.SaveChanges();
            }

            if (!context.Announcements.Any())
            {
                context.Announcements.AddRange(
                    new Announcement
                    {
                        Description = "This is the first announcement.",
                        DateCreated = DateTime.Now,
                        Author = "CCS Admin"
                    },
                    new Announcement
                    {
                        Description = "This is the second announcement.",
                        DateCreated = DateTime.Now,
                        Author = "CCS Admin"
                    }
                );
                context.SaveChanges();
            }

            if (!context.Announcements.Any())
            {
                context.Announcements.AddRange(
                    new Announcement
                    {
                        Description = "This is the first announcement.",
                        DateCreated = DateTime.Now,
                        Author = "CCS Admin"
                    },
                    new Announcement
                    {
                        Description = "This is the second announcement.",
                        DateCreated = DateTime.Now,
                        Author = "CCS Admin"
                    }
                );
                context.SaveChanges();
            }


            if (!context.CurrentSitIns.Any())
            {
                context.CurrentSitIns.AddRange(
                    new CurrentSitIn
                    {
                        Id = 123,
                        Name = "John Admin Doe",
                        Purpose = "Testing System",
                        Lab = "530",
                        Status = "done",
                        Feedback = "Very Awesome",
                        SessionNumber = 1,
                        Points = 1,
                        isPointsGiven = true
                    },
                    new CurrentSitIn
                    {
                        Id = 123,
                        Name = "John Admin Doe",
                        Purpose = "Testing System 2",
                        Lab = "528",
                        Status = "sitting in",
                        Feedback = null,
                        SessionNumber = 2,
                        Points = 0,
                        isPointsGiven = false
                    }
                );
                context.SaveChanges();
            }

            if (!context.PointsLogs.Any())
            {
                // 1. Fetch the students we seeded earlier using their unique emails
                var firstAccount = context.Accounts.FirstOrDefault(a => a.Email == "johnhvillarin@gmail.com");
                var secondAccount = context.Accounts.FirstOrDefault(a => a.Email == "janedoe@gmail.com");

                // 2. Double-check that they exist before trying to read their IDs
                if (firstAccount != null && secondAccount != null)
                {
                    context.PointsLogs.AddRange(
                        new PointsLog
                        {
                            // We use the primary key that EF Core generated dynamically
                            AccountUniqueId = firstAccount.UniqueId,
                            PointsGiven = 1,
                            Reason = "Submitted feedback for Sit-in Session #1",
                            DateLogged = DateTime.Now.AddDays(-1) // Logged yesterday
                        },
                        new PointsLog
                        {
                            AccountUniqueId = secondAccount.UniqueId,
                            PointsGiven = 1,
                            Reason = "Submitted feedback for Sit-in Session #1",
                            DateLogged = DateTime.Now
                        }
                    );

                    context.SaveChanges();
                }
            }
        }
    }
}