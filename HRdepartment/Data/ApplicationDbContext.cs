using HRdepartment.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRdepartment.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<UserLoginHistory> UserLoginHistories { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<Post> Post { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);// Задает схему для базы данных
            builder.HasDefaultSchema("Identity");   /* Переименовывает таблицу пользователей из AspNetUsers в Identity.User. */
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable(name: "User");
            });
            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Role");
            });
            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
            });
            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });
            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });
            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");
            });
            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
            });
            builder.Entity<UserLoginHistory>().ToTable("UserLoginHistory");
            builder.Entity<Department>().ToTable("Department");
            builder.Entity<Post>().ToTable("Post");

            builder.Entity<Department>()
                .HasMany(h => h.ApplicationUser)
                .WithOne(v => v.Department)
                .HasForeignKey(v => v.Department_Id)
                .OnDelete(DeleteBehavior.Cascade);
           
            
            builder.Entity<Post>()
              .HasMany(h => h.ApplicationUser)
              .WithOne(v => v.Post)
              .HasForeignKey(v => v.Id_Post)
              .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
