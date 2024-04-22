using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeApi.Models
{
    public class UpdateRecipeDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string TimeToCook { get; set; }
        // public List<CreateRecipeInstructionToExistingRecipeDto> Instructions { get; set; }
        // public List<CreateRecipeIngredientToExistingRecipeDto> Ingredients { get; set; }
        public int Portions { get; set; }
    }
}