using System.ComponentModel.DataAnnotations.Schema;
using RecipeApi.Database;

namespace RecipeApi.Database
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string TimeToCook { get; set; }
        public List<Recipe_Instruction> Instructions { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public int Portions { get; set; }

        public DateTime CreatedAt { get; set; }


    }
}