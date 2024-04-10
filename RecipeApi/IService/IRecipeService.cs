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
        IEnumerable<RecipeDto> GetRecipesByAuthor(int authorId);
        int AddRecipe(CreateRecipeDto recipe);
        void UpdateRecipe(int id, UpdateRecipeDto recipe);
        void DeleteRecipe(int id);

    }
}