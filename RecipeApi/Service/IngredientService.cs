using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using RecipeApi.Database;
using RecipeApi.IService;
using RecipeApi.Models;

namespace RecipeApi.Service
{
    public class IngredientService : IRecipeService
    {
        private readonly RecipeDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public IngredientService(RecipeDbContext context, IMapper mapper, ILogger<IngredientService> logger)
        {
            _dbContext = context;
            _mapper = mapper;
            _logger = logger;
        }

        public int AddIngredient(IngredientDto ingredientDto)
        {
            var ingredient = _mapper.Map<Ingredient>(ingredientDto);
            _dbContext.Ingredients.Add(ingredient);
            _dbContext.SaveChanges();

            return ingredient.Id;
        }

        public void DeleteIngredient(int id)
        {
            _logger.LogInformation($"Deleting ingredient with id {id}");

            var ingredient = _dbContext.Ingredients.FirstOrDefault(i => i.Id == id);

            if (ingredient is null) throw new DllNotFoundException("Ingredient not found");

            _dbContext.Ingredients.Remove(ingredient);
            _dbContext.SaveChanges();
        }

        public IngredientDto GetIngredient(int id)
        {
            var ingredient = _dbContext.Ingredients.FirstOrDefault(i => i.Id == id);

            if (ingredient is null) throw new DllNotFoundException("Ingredient not found");

            return _mapper.Map<IngredientDto>(ingredient);
        }

        public IEnumerable<IngredientDto> GetIngredients()
        {
            var ingredients = _dbContext.Ingredients.ToList();
            
            return _mapper.Map<IEnumerable<IngredientDto>>(ingredients);
        }

        public void UpdateIngredient(int id, IngredientDto ingredient)
        {
            throw new NotImplementedException();
        }
    }
}