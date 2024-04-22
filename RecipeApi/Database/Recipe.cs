using System.ComponentModel.DataAnnotations.Schema;
using RecipeApi.Database;

namespace RecipeApi.Database
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string TimeToCook { get; set; }
        public virtual List<RecipeInstruction> Instructions { get; set; }
        public virtual List<RecipeIngredient> Ingredients { get; set; }
        public int Portions { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public int? UserId { get; set; }
        public virtual User User { get; set; }
    }
}