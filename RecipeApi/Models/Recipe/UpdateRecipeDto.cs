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
        public int Portions { get; set; }
        public List<UpdateRecipeInstructionDto> Instructions { get; set; }
        public List<UpdateRecipeIngredientDto> Ingredients { get; set; }
    }
}