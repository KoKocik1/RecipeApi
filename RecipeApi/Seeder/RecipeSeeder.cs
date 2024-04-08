using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RecipeApi.Database;

namespace RecipeApi.Seeder
{
    public class RecipeSeeder
    {
        private readonly RecipeDbContext _dbContext;

        public RecipeSeeder(RecipeDbContext context)
        {
            _dbContext = context;
        }

        public void Seed()
        {
            if (_dbContext.Database.CanConnect())
            {
                var pendingMigration = _dbContext.Database.GetPendingMigrations();
                if (pendingMigration != null && pendingMigration.Any())
                {
                    _dbContext.Database.Migrate();
                }


                if (!_dbContext.Roles.Any())
                {
                    var roles = getRoles();
                    _dbContext.Roles.AddRange(roles);
                    _dbContext.SaveChanges();
                }
                if (!_dbContext.Ingredients.Any())
                {
                    var ingredients = getIngredients();
                    _dbContext.Ingredients.AddRange(ingredients);
                    _dbContext.SaveChanges();
                }
            }
        }
        private IEnumerable<Role> getRoles()
        {
            var roles = new List<Role>()
            {
                new Role(){
                    Name="User"
                },
                new Role(){
                    Name="Manager"
                },
                new Role(){
                    Name="Admin"
                }
            };
            return roles;
        }
        private IEnumerable<Ingredient> getIngredients()
        {
            var ingredients = new List<Ingredient>()
            {
                new Ingredient(){
                    Name="Salt",
                },
                new Ingredient(){
                    Name="Pepper",
                },
                new Ingredient(){
                    Name="Sugar",
                }
            };
            return ingredients;
        }
    }
}