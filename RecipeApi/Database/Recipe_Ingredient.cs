using System.ComponentModel.DataAnnotations;

namespace RecipeApi.Database
{
    public class Recipe_Ingredient
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        // public virtual Recipe Recipe { get; set; }
        public int IngredientId { get; set; }
        public virtual Ingredient Ingredient { get; set; }

        public int Units_ingredientId { get; set; }
        public virtual Units_ingredient Unit { get; set; }

        public double Quantity { get; set; }
    }
}