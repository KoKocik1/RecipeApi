using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeApi.Models
{
    public class UpdateIngredientDto
    {
        public string Name { get; set; }

        public bool Verified { get; set; }
    }
}