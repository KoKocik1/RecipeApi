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

        public int AddRecipeIngredient(CreateRecipeIngredientToExistingRecipeDto ingredient)
        {
            var recipeIngredient = _mapper.Map<RecipeIngredient>(ingredient);
            _dbContext.RecipeIngredients.Add(recipeIngredient);
            _dbContext.SaveChanges();

            return recipeIngredient.Id;
        }

        public void DeleteRecipeIngredient(int id)
        {
            _logger.LogInformation($"Deleting recipeIngredient with id {id}");

            var recipeIngredient = _dbContext.RecipeIngredients.FirstOrDefault(i => i.Id == id);

            if (recipeIngredient is null) throw new NotFoundException("Recipe ingredient not found");

            _dbContext.RecipeIngredients.Remove(recipeIngredient);
            _dbContext.SaveChanges();
        }

        public RecipeIngredientDto GetRecipeIngredient(int id)
        {
            var recipeIngredient = _dbContext.RecipeIngredients.FirstOrDefault(i => i.Id==id);

            if (recipeIngredient is null) throw new NotFoundException("Recipe ingredient not found");

            return _mapper.Map<RecipeIngredientDto>(recipeIngredient);
        }

        public IEnumerable<RecipeIngredientDto> GetRecipeIngredients(int recipeId)
        {
            var recipeIngredients = _dbContext.RecipeIngredients.Where(i => i.RecipeId == recipeId).ToList();

            return _mapper.Map<IEnumerable<RecipeIngredientDto>>(recipeIngredients);
        }

        public void UpdateRecipeIngredient(int id, UpdateRecipeIngredientDto ingredient)
        {
            _logger.LogInformation($"Updating recipe ingredient with id {id}");

            var recipeIngredient = _dbContext.RecipeIngredients.FirstOrDefault(i => i.Id == id);

            if (recipeIngredient is null) throw new NotFoundException("Recipe ingredient not found");

            recipeIngredient.IngredientId = ingredient.IngredientId;
            recipeIngredient.Quantity = ingredient.Quantity;
            recipeIngredient.UnitIngredientId = ingredient.UnitIngredientId;

            _dbContext.SaveChanges();
        }
    }
}