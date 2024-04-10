using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RecipeApi.Database;
using RecipeApi.Models;

namespace RecipeApi.IService
{
    public interface IRecipeIngredientService
    {
        RecipeIngredientDto GetRecipeIngredient(int id);
        IEnumerable<RecipeIngredientDto> GetRecipeIngredients(int recipeId);
        int AddRecipeIngredient(CreateRecipeIngredientToExistingRecipeDto ingredient);
        void UpdateRecipeIngredient(int id, UpdateRecipeIngredientDto ingredient);
        void DeleteRecipeIngredient(int id);

    }
}