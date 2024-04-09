using Microsoft.EntityFrameworkCore;

namespace RecipeApi.Database
{
    public class RecipeDbContext : DbContext
    {
        public RecipeDbContext(DbContextOptions<RecipeDbContext> options) : base(options)
        {
        }

        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<RecipeIngredient> Recipe_Ingredients { get; set; }
        public DbSet<RecipeInstruction> Recipe_Instructions { get; set; }
        public DbSet<UnitsIngredient> Units_ingredients { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Recipe>()
                .Property(r => r.Title)
                .IsRequired();          
        }

    }
}
