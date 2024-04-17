using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using RecipeApi.Database;
using RecipeApi.Exceptions;
using RecipeApi.IService;
using RecipeApi.Models;

namespace RecipeApi.Service
{
    public class IngredientService : IIngrededientService
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

        public int AddIngredient(CreateIngredientDto ingredientDto)
        {

            //TODO: Move to validator
            if(_dbContext.Ingredients.FirstOrDefault(r=>r.Name==ingredientDto.Name) is not null) throw new BadRequestException("Ingredient already exists");
            
            var ingredient = _mapper.Map<Ingredient>(ingredientDto);
            _dbContext.Ingredients.Add(ingredient);
            _dbContext.SaveChanges();

            return ingredient.Id;
        }

        public void DeleteIngredient(int id)
        {
            _logger.LogInformation($"Deleting ingredient with id {id}");

            var ingredient = _dbContext.Ingredients.FirstOrDefault(i => i.Id == id);

            if (ingredient is null) throw new NotFoundException("Ingredient not found");

            _dbContext.Ingredients.Remove(ingredient);
            _dbContext.SaveChanges();
        }

        public IngredientDto GetIngredient(int id)
        {
            var ingredient = _dbContext.Ingredients.FirstOrDefault(i => i.Id == id);

            if (ingredient is null) throw new NotFoundException("Ingredient not found");

            return _mapper.Map<IngredientDto>(ingredient);
        }

        public IngredientDto GetIngredient(string name)
        {
            var ingredient = _dbContext.Ingredients.FirstOrDefault(i => i.Name == name);

            if (ingredient is null) throw new NotFoundException("Ingredient not found");

            return _mapper.Map<IngredientDto>(ingredient);
        }

        public IEnumerable<IngredientDto> GetIngredients()
        {
            var ingredients = _dbContext.Ingredients.ToList();

            if (ingredients is null) throw new NotFoundException("Ingredients not found");

            return _mapper.Map<IEnumerable<IngredientDto>>(ingredients);
        }

        public void UpdateIngredient(int id, UpdateIngredientDto ingredient)
        {
            var ingredientToUpdate = _dbContext.Ingredients.FirstOrDefault(i => i.Id == id);

            if (ingredientToUpdate is null) throw new NotFoundException("Ingredient not found");

            ingredientToUpdate.Name = ingredient.Name;
            ingredientToUpdate.Verified = ingredient.Verified;

            _dbContext.SaveChanges();
        }
    }
}