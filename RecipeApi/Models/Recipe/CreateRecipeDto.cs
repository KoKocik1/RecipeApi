using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RecipeApi.Models;

namespace RecipeApi.Models
{
    public class CreateRecipeDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string TimeToCook { get; set; }
        public int Portions { get; set; }
    }
}