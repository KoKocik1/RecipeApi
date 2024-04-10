using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using RecipeApi.Database;
using RecipeApi.Exceptions;
using RecipeApi.IService;
using RecipeApi.Models;

namespace RecipeApi.Service
{
    public class RecipeIngredientService : IRecipeIngredientService
    {
        private readonly RecipeDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public RecipeIngredientService(RecipeDbContext context, IMapper mapper, ILogger<RecipeIngredientService> logger)
        {
            _dbContext = context;
            _mapper = mapper;
            _logger = logger;
        }

        public int AddRecipeIngredient(CreateRecipeIngredientDto ingredient)
        {
            var recipeIngredient = _mapper.Map<RecipeIngredient>(ingredient);
            _dbContext.Recipe_Ingredients.Add(recipeIngredient);
            _dbContext.SaveChanges();

            return recipeIngredient.Id;
        }

        public void DeleteRecipeIngredient(int id)
        {
            _logger.LogInformation($"Deleting recipeIngredient with id {id}");

            var recipeIngredient = _dbContext.Recipe_Ingredients.FirstOrDefault(i => i.Id == id);

            if (recipeIngredient is null) throw new NotFoundException("Recipe ingredient not found");

            _dbContext.Recipe_Ingredients.Remove(recipeIngredient);
            _dbContext.SaveChanges();
        }

        public RecipeIngredientDto GetRecipeIngredient(int id)
        {
            var recipeIngredient = _dbContext.Recipe_Ingredients.FirstOrDefault(i => i.Id==id);

            if (recipeIngredient is null) throw new NotFoundException("Recipe ingredient not found");

            return _mapper.Map<RecipeIngredientDto>(recipeIngredient);
        }

        public IEnumerable<RecipeIngredientDto> GetRecipeIngredients(int recipeId)
        {
            var recipeIngredients = _dbContext.Recipe_Ingredients.Where(i => i.RecipeId == recipeId).ToList();

            return _mapper.Map<IEnumerable<RecipeIngredientDto>>(recipeIngredients);
        }

        public void UpdateRecipeIngredient(int id, CreateRecipeIngredientDto ingredient)
        {
            _logger.LogInformation($"Updating recipe ingredient with id {id}");

            var recipeIngredient = _dbContext.Recipe_Ingredients.FirstOrDefault(i => i.Id == id);

            if (recipeIngredient is null) throw new NotFoundException("Recipe ingredient not found");

            recipeIngredient.IngredientId = ingredient.IngredientId;
            recipeIngredient.Quantity = ingredient.Quantity;
            recipeIngredient.Unit_ingredientId = ingredient.UnitId;

            _dbContext.SaveChanges();
        }
    }
}