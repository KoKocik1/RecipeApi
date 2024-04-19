using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RecipeApi.Models;
using RecipeApi.Models.User;

namespace RecipeApi.IService
{
    public interface IAccountService
    {
        Task RegisterUserAsync(RegisterUserDto dto);
        Task<LoginResult> LoginUserAsync(LoginDto dto);
        Task<string> ConfirmEmailAsync(string userId, string token);
        Task SingOutAsync();
    }
}