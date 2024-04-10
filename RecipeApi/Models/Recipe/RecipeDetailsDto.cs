using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeApi.Models
{
    public class RecipeDetailsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string TimeToCook { get; set; }
        public List<RecipeInstructionDto> Instructions { get; set; }
        public List<RecipeIngredientDto> Ingredients { get; set; }
        public int Portions { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public RecipeAuthorDto User { get; set; }

    }
}