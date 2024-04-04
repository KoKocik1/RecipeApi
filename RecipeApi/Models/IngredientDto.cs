using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeApi.Models
{
    public class IngredientDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Verified { get; set; }
    }
}