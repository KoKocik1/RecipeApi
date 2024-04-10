using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RecipeApi.Models;

namespace RecipeApi.IService
{
    public interface IIngrededientService
    {
        IngredientDto GetIngredient(int id);
        IEnumerable<IngredientDto> GetIngredients();
        int AddIngredient(CreateIngredientDto ingredient);
        void UpdateIngredient(int id, UpdateIngredientDto ingredient);
        void DeleteIngredient(int id);

    }
}