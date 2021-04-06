using System;
using api.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace api.Data
{
    public class DataContext : IdentityDbContext<AppUser, AppRole, int,
            IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>,
            IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        private static ILogger _logger;

        public DataContext(DbContextOptions options, ILogger logger) : base(options)
        {
            _logger = logger;
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Category> Categorys { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<BookAuthor> BookAuthors { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>()
                .HasMany(ur => ur.UserRole)
                .WithOne(u => u.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            builder.Entity<AppRole>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

            builder.Entity<Book>()
                .HasOne(b => b.Category)
                .WithMany(c => c.Books)
                .HasForeignKey(b => b.CategoryCode)
                .IsRequired();

            builder.Entity<BookAuthor>()
                .HasKey(bu => new { bu.AuthorId, bu.BookId });

        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //=> optionsBuilder.LogTo(_logger.Information);
    }
}
