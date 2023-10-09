using Microsoft.EntityFrameworkCore;
using WebApi.Model;

namespace WebApi.Datos
{
    public class AplicationDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public AplicationDbContext(DbContextOptions<AplicationDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product() { 
                    Id=1, 
                    Name="Gaseosa",
                    Category="Bebidas",
                    Proveedor="Coca Cola",
                    Precio=15.50,
                    Stock=20,
                    Description="",
                    
                    FechaCreacion=DateTime.Now,
                    imgUrl=""
                },
                new Product()
                {
                    Id = 2,
                    Name = "Agua",
                    Category = "Bebidas",
                    Proveedor = "Coca Cola",
                    Precio = 5.50,
                    Stock = 120,
                    Description = "",
                    
                    FechaCreacion = DateTime.Now,
                    imgUrl = ""
                }
                );
        }
    }
}
