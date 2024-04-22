using System.ComponentModel.DataAnnotations;

namespace RecipeApi.Database
{
    public class RecipeIngredient
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public int IngredientId { get; set; }
        public virtual Ingredient Ingredient { get; set; }
        public int UnitIngredientId { get; set; }
        public virtual UnitIngredient UnitIngredient { get; set; }
        public double Quantity { get; set; }
    }
}