using API.FurnitureStore.Shared;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace API.FurnitureStore.Data
{
    public class APIFurnitureStoreContext : IdentityDbContext
    {
        public APIFurnitureStoreContext(DbContextOptions options) : base(options) 
        {
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories{ get; set; }
        public DbSet<OrderItem> OrderDetails { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Establesemos que OrderDetail trenda una clave primaria compuesta con OrderId y ProductId
            modelBuilder.Entity<OrderItem>()
                .HasKey(od => new { od.OrderId, od.ProductId });

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context">DataBase context</param>
        /// <param name="dataFake">(Default: false) si quiere insertar datos de prueba</param>
        public static void SetInitialize(APIFurnitureStoreContext context, bool dataFake = false)
        {

            if (dataFake)
            {
                if (!context.ProductCategories.Any())
                {
                    context.ProductCategories.Add(new ProductCategory() { Name = "Categoria A" });
                    context.ProductCategories.Add(new ProductCategory() { Name = "Categoria B" });
                    context.ProductCategories.Add(new ProductCategory() { Name = "Categoria C" });
                    context.SaveChanges();
                }
            }

            
        }

    }
}
