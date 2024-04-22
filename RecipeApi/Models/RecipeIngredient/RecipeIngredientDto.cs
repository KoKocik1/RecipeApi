using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RecipeApi.Database;

namespace RecipeApi.Models
{
    public class RecipeIngredientDto
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public int IngredientId { get; set; }
        public string Name { get; set; }
        public bool Verified { get; set; }
        public int UnitIngredientId { get; set; }
        public string Type { get; set; }
        public double Quantity { get; set; }
    }
}