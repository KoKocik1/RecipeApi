using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeApi.Models
{
    public class RecipeIngredientDto
    {
        
        public int RecipeId { get; set; }
        public int IngredientId { get; set; }
        public string UnitId { get; set; }
        public double Quantity { get; set; }
    }
}