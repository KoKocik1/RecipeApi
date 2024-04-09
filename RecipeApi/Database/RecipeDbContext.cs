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
        public DbSet<Recipe_Ingredient> Recipe_Ingredients { get; set; }
        public DbSet<Recipe_Instruction> Recipe_Instructions { get; set; }
        public DbSet<Units_ingredient> Units_ingredients { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Recipe>()
                .Property(r => r.Title)
                .IsRequired();

            // modelBuilder.Entity<Recipe>()
            //     .Property(r => r.Instructions)
            //     .IsRequired();

            // modelBuilder.Entity<Recipe>()
            //     .Property(r => r.RecipeIngredients)
            //     .IsRequired();

            // modelBuilder.Entity<Recipe_Ingredient>()
            //     .HasKey(ri => new { ri.RecipeId, ri.IngredientId });

            // modelBuilder.Entity<Recipe_Ingredient>()
            //     .HasOne(ri => ri.Recipe)
            //     .WithMany(r => r.RecipeIngredients)
            //     .HasForeignKey(ri => ri.RecipeId);

            // modelBuilder.Entity<Recipe_Ingredient>()
            //     .HasOne(ri => ri.Ingredient)
            //     .WithMany()
            //     .HasForeignKey(ri => ri.IngredientId);
        }

    }
}
