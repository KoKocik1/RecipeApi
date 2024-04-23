using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeApi.Models
{
    public class UserPublicDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string AboutMe { get; set; }
    }
}