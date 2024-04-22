using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeApi.Models
{
    public class UserPublicDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string AboutMe { get; set; }
    }
}