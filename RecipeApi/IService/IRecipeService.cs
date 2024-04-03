using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RecipeApi.Models;

namespace RecipeApi.IService
{
    public interface IRecipeService
    {
        IngredientDto GetIngredient(int id);
        IEnumerable<IngredientDto> GetIngredients();
        int AddIngredient(IngredientDto ingredient);
        void UpdateIngredient(int id, IngredientDto ingredient);
        void DeleteIngredient(int id);
        
    }
}