using System.ComponentModel.DataAnnotations;

namespace RecipeApi.Database
{
    public class RecipeIngredient
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public int IngredientId { get; set; }
        public virtual Ingredient Ingredient { get; set; }

        public int Units_ingredientId { get; set; }
        public virtual UnitsIngredient Unit { get; set; }

        public double Quantity { get; set; }
    }
}