using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RecipeApi.Database;

namespace RecipeApi.Models
{
    public class RecipeDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}