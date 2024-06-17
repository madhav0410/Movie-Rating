using DataAceess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAceess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Rating> Ratings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
           .HasKey(u => u.Id);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique(); // Ensure email is unique

            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired();

            modelBuilder.Entity<Movie>()
           .HasKey(m => m.Id);

            modelBuilder.Entity<Movie>()
                .HasIndex(m => m.Title)
                .IsUnique(); // Ensure title is unique

            modelBuilder.Entity<Movie>()
                .Property(m => m.Title)
                .IsRequired();


            // Configure Rating entity
            modelBuilder.Entity<Rating>()
                .HasKey(r => r.Id);

            // Configure the foreign key relationship
            modelBuilder.Entity<Rating>()
            .HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserEmail)
            .HasPrincipalKey(u => u.Email);


            // Configure Rating entity
            modelBuilder.Entity<Rating>()
                .HasKey(r => r.Id);

            // Configure the foreign key relationship
            modelBuilder.Entity<Rating>()
            .HasOne(r => r.movie)
            .WithMany()
            .HasForeignKey(r => r.Movie)
            .HasPrincipalKey(u => u.Title);


            modelBuilder.Entity<Review>()
                .HasKey(re => re.Id);
            modelBuilder.Entity<Review>()
                .HasOne(re => re.user)
                .WithMany()
                .HasForeignKey(re => re.User)
                .HasPrincipalKey(u => u.Email);

            modelBuilder.Entity<Review>()
                .HasKey(re => re.Id);
            modelBuilder.Entity<Review>()
                .HasOne(re => re.movie)
                .WithMany()
                .HasForeignKey(re => re.Movie)
                .HasPrincipalKey(m => m.Title);
        }
    }
}
