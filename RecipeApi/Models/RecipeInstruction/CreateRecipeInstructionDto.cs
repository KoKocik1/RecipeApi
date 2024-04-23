using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeApi.Models
{
    public class CreateRecipeInstructionDto
    {
        public string Instruction { get; set; }
        public int Order { get; set; }
    }
}