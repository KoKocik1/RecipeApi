using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RecipeApi.Database;

namespace RecipeApi.Tests
{
    public class TestDbContextBase : IDisposable
    {
        protected readonly DbContextOptions<RecipeDbContext> Options;

        protected TestDbContextBase()
        {
            Options = new DbContextOptionsBuilder<RecipeDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        public void Dispose()
        {
            // Dispose resources if needed
        }
    }
}