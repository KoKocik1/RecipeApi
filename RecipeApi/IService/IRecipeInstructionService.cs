using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RecipeApi.Models;

namespace RecipeApi.IService
{
    public interface IRecipeInstructionService
    {
        void DeleteRecipeInstruction(int id);
        RecipeInstructionDto GetRecipeInstruction(int id);
        IEnumerable<RecipeInstructionDto> GetRecipeInstructions();
        IEnumerable<RecipeInstructionDto> GetRecipeInstructionsByRecipeId(int recipeId);
        int AddRecipeInstruction(CreateRecipeInstructionDto recipeInstruction);
        void UpdateRecipeInstruction(int id, RecipeInstructionDto recipeInstruction);
        
    }
}