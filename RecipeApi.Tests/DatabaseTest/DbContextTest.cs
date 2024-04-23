using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RecipeApi.Database;
using Xunit;

namespace RecipeApi.Tests.DatabaseTest
{
    public class DbContextTest : TestDbContextBase
    {
        [Fact]
        public void DbContext_CanCreateInstance()
        {
            // Database in memory without connect to real database
            var options = new DbContextOptionsBuilder<RecipeDbContext>()
                .UseInMemoryDatabase(databaseName: "DbContext_CanCreateInstance")
                .Options;

            // act
            using (var context = new RecipeDbContext(options))
            {
                // Assert - czy instancja zostala stworzona poprawnie
                Assert.NotNull(context);
            }
        }
    }
}