using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RecipeApi.Models;

namespace RecipeApi.IService
{
    public interface IRecipeService
    {
        RecipeDetailsDto GetRecipe(int recipeId);
        PageResult<RecipeDto> GetRecipes(RecipeQuery query);
        IEnumerable<RecipeDto> GetRecipesByAuthor(string authorId);
        int AddRecipe(CreateRecipeDto recipe, string userId);
        void UpdateRecipe(int recipeId, UpdateRecipeDto recipe, string userId);
        void DeleteRecipe(int recipeId, string userId);
        int CloneRecipe(int recipeId, string userId);

    }
}