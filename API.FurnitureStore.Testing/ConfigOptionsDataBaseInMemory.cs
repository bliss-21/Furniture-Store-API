using API.FurnitureStore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.FurnitureStore.Testing
{
    internal static class ConfigOptionsDataBaseInMemory
    {
        public static DbContextOptions<APIFurnitureStoreContext> CreateNewContextOptions()
        {
            // Create a fresh service provider, and therefore a fresh 
            // InMemory database instance.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // Create a new options instance telling the context to use an
            // InMemory database and the new service provider.
            var builder = new DbContextOptionsBuilder<APIFurnitureStoreContext>();
            builder.UseInMemoryDatabase($"database-in-memori-{Guid.NewGuid}")
                    .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }
    }
}
