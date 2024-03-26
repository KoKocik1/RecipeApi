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
            // Baza danych w pamięci bez koniecznosci łączenia się z rzeczywistą bazą danych
            var options = new DbContextOptionsBuilder<RecipeDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
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