using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using api.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace api.Data.Seeds
{
    public class Seed
    {
        public static async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            if (await userManager.Users.AnyAsync())
                return;

            var userData = await System.IO.File.ReadAllTextAsync("Data/Seeds/UserSeedData.json");
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);
            var queue = new Queue<AppUser>(users);

            if (queue.Count <= 0)
                return;

            var roles = new List<AppRole>
            {
                new AppRole { Name = "Admin"},
                new AppRole { Name = "Staff"},
                new AppRole { Name = "Guest"}
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            int i = 1;
            while (queue.Count > 0)
            {
                var user = queue.Dequeue();
                user.Id = 0;
                user.UserName = user.UserName.ToLower();

                var role = i++ <= 3 ? "Staff" : "Guest";

                await userManager.CreateAsync(user, "12345678");
                await userManager.AddToRoleAsync(user, role);
            }
            
            var admin = new AppUser
            {
                UserName = "administrator"
            };

            await userManager.CreateAsync(admin, "12345678");

            await userManager.AddToRoleAsync(admin, "Admin");
            await userManager.AddToRoleAsync(admin, "Staff");
        }

        public static async Task SeedBooks(DataContext context)
        {
            //Category
            if (!await context.Categorys.AnyAsync())
            {

                var categoryData = await System.IO.File.ReadAllTextAsync("Data/Seeds/CategorySeedData.json");
                var categories = JsonSerializer.Deserialize<List<Category>>(categoryData);

                if (categories == null)
                    return;

                await context.Categorys.AddRangeAsync(categories);
                await context.SaveChangesAsync();
            }

            //Author
            if (!await context.Authors.AnyAsync())
            {

                var authorData = await System.IO.File.ReadAllTextAsync("Data/Seeds/AuthorSeedData.json");
                var authors = JsonSerializer.Deserialize<List<Author>>(authorData);

                if (authors == null)
                    return;

                authors.Sort((x, y) => x.Id.CompareTo(y.Id));
                authors.ForEach(a =>
                {
                    a.Id = 0;
                    context.Authors.Add(a);
                });

                await context.Authors.AddRangeAsync(authors);
                await context.SaveChangesAsync();
            }

            //Book
            if (!await context.Books.AnyAsync())
            {

                var bookData = await System.IO.File.ReadAllTextAsync("Data/Seeds/BookSeedData.json");
                var books = JsonSerializer.Deserialize<List<Book>>(bookData);

                if (books == null)
                    return;

                await context.Books.AddRangeAsync(books);
                await context.SaveChangesAsync();
            }

            
        }


    }
}
