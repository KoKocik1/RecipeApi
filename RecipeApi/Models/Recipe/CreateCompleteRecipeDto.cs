using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeApi.Models.Recipe
{
    public class CreateCompleteRecipeDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string TimeToCook { get; set; }
        public int Portions { get; set; }
        public List<CreateRecipeInstructionToNewRecipeDto> Instructions { get; set; }
        public List<CreateRecipeIngredientToNewRecipeDto> Ingredients { get; set; }
    }
}