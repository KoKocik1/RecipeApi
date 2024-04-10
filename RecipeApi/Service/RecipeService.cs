using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RecipeApi.Database;
using RecipeApi.Exceptions;
using RecipeApi.IService;
using RecipeApi.Models;

namespace RecipeApi.Service
{
    public class RecipeService : IRecipeService
    {
        private readonly RecipeDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public RecipeService(RecipeDbContext context, IMapper mapper, ILogger<RecipeService> logger)
        {
            _dbContext = context;
            _mapper = mapper;
            _logger = logger;
        }
        public int AddRecipe(CreateRecipeDto recipe)
        {
            var recipeEntity = _mapper.Map<Recipe>(recipe);
            //TODO: temporary 
            recipeEntity.UserId = 4;
            recipeEntity.CreatedAt = DateTime.Now.ToUniversalTime();
            _dbContext.Recipes.Add(recipeEntity);
            _dbContext.SaveChanges();

            // if(recipe.Ingredients != null)
            // {
            //     foreach (var ingredient in recipe.Ingredients)
            //     {
            //         var recipeIngredient = _mapper.Map<RecipeIngredient>(ingredient);
            //         recipeIngredient.RecipeId = recipeEntity.Id;
            //         _dbContext.RecipeIngredients.Add(recipeIngredient);
            //     }
            // }
            // if(recipe.Instructions != null)
            // {
            //     foreach (var instruction in recipe.Instructions)
            //     {
            //         var recipeInstruction = _mapper.Map<RecipeInstruction>(instruction);
            //         recipeInstruction.RecipeId = recipeEntity.Id;
            //         _dbContext.RecipeInstructions.Add(recipeInstruction);
            //     }
            // }
            return recipeEntity.Id;
        }

        public void DeleteRecipe(int id)
        {
            _logger.LogInformation($"Deleting recipe with id {id}");
            var recipe = _dbContext.Recipes.FirstOrDefault(r => r.Id == id);
            if (recipe is null) throw new NotFoundException("Recipe not found");
            _dbContext.Recipes.Remove(recipe);
            _dbContext.SaveChanges();
        }

        public RecipeDetailsDto GetRecipe(int id)
        {
            var recipe = _dbContext.Recipes
            .Include(r=>r.Ingredients)
                .ThenInclude(ri => ri.Ingredient)
            .Include(r=>r.Ingredients)
                .ThenInclude(ri=>ri.UnitIngredient)
            .Include(r=>r.Instructions)
            .Include(r=>r.User)
            .FirstOrDefault(r => r.Id == id);

            recipe.Instructions = recipe.Instructions.OrderBy(i => i.Order).ToList();

            if (recipe is null) throw new NotFoundException("Recipe not found");
            return _mapper.Map<RecipeDetailsDto>(recipe);
        }

        public IEnumerable<RecipeDto> GetRecipes()
        {
            var recipes = _dbContext.Recipes.ToList();
            return _mapper.Map<IEnumerable<RecipeDto>>(recipes);
        }

        public IEnumerable<RecipeDto> GetRecipesByAuthor(int authorId)
        {
            var recipes = _dbContext.Recipes.Where(r => r.UserId == authorId)
            .Include(r=>r.User)
            .Include(r=>r.Ingredients)
            .Include(r=>r.Instructions)
            .ToList();
            return _mapper.Map<IEnumerable<RecipeDto>>(recipes);
        }

        public void UpdateRecipe(int id, UpdateRecipeDto recipe)
        {
            _logger.LogInformation($"Updating recipe with id {id}");
            var recipeEntity = _dbContext.Recipes.FirstOrDefault(r => r.Id == id);
            if (recipeEntity is null) throw new NotFoundException("Recipe not found");
            recipeEntity.Title = recipe.Title;
            recipeEntity.Description = recipe.Description;
            recipeEntity.Portions = recipe.Portions;
            recipeEntity.TimeToCook = recipe.TimeToCook;
            recipeEntity.UpdatedAt = DateTime.Now;
            // recipeEntity.Instructions = _mapper.Map<List<RecipeInstruction>>(recipe.Instructions);
            // recipeEntity.Ingredients = _mapper.Map<List<RecipeIngredient>>(recipe.Ingredients);

            _dbContext.Recipes.Update(recipeEntity);
            _dbContext.SaveChanges();
        }
    }
}