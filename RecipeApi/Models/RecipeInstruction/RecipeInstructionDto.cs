using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeApi.Models
{
    public class RecipeInstructionDto
    {
        public int Id { get; set; }
        public string Instruction { get; set; }
        public int Order { get; set; }
    }
}