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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Recipe>()
                .Property(r => r.Title)
                .IsRequired();

            modelBuilder.Entity<Recipe>()
                .Property(r => r.Instructions)
                .IsRequired();

            modelBuilder.Entity<Recipe>()
                .Property(r => r.Ingredients)
                .IsRequired();
            //TODO
        }

    }
}
