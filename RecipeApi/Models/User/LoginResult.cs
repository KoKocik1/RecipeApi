using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeApi.Models.User
{
    public class LoginResult
    {
        public string Token { get; set; }
        public string Expires_at { get; set; }
    }
}