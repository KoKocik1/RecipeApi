using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeApi.Database
{
    public class Recipe_Instruction
    {
        public int Id { get; set; }
        public string Instruction { get; set; }
        public int RecipeId { get; set; }
        // public virtual Recipe Recipe { get; set; }
        public int Order { get; set; }
    }
}