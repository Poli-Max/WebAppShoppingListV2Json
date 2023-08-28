using System;
using Web_App_Shop_V2.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Web_App_Shop_V2.Domain.Helper;
using Web_App_Shop_V2.Domain.Enum;

namespace Web_App_Shop_V2.DAL;

public class AplicationDbContext : DbContext
{
    public AplicationDbContext(DbContextOptions<AplicationDbContext> options): base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<Product> Product { get; set; }

    public DbSet<User> user { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) //метод создания admin
    {

        modelBuilder.Entity<User>(builder =>
        {
            builder.HasData(new User
            {
                id = 1,
                name = "Polet",
                password = EncryptionPassword.HashPassword("123321"),
                typeUser = TypeUser.admin
            });

            builder.Property(x => x.id).ValueGeneratedOnAdd();
            builder.Property(x => x.name).IsRequired();
            builder.Property(x => x.password).HasMaxLength(100).IsRequired();
        });
    }
}