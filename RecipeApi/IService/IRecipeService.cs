using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RecipeApi.Models;

namespace RecipeApi.IService
{
    public interface IRecipeService
    {
        RecipeDetailsDto GetRecipe(int id);
        IEnumerable<RecipeDto> GetRecipes();
        IEnumerable<RecipeDto> GetRecipesByAuthor(string authorId);
        int AddRecipe(CreateRecipeDto recipe, string userId);
        void UpdateRecipe(int id, UpdateRecipeDto recipe, string userId);
        void DeleteRecipe(int id, string userId);

    }
}