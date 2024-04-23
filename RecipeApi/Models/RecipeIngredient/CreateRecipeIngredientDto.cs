using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeApi.Models
{
    public class CreateRecipeIngredientDto
    {
        public int IngredientId { get; set; }
        public int UnitIngredientId { get; set; }
        public double Quantity { get; set; }
    }
}