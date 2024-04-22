using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RecipeApi.Authentication;
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
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContentService _userContentService;

        public RecipeIngredientService(RecipeDbContext context,
        IMapper mapper,
        ILogger<RecipeIngredientService> logger,
        IAuthorizationService authorizationService,
        IUserContentService userContentService)
        {
            _dbContext = context;
            _mapper = mapper;
            _logger = logger;
            _authorizationService = authorizationService;
            _userContentService = userContentService;
        }

        public int AddRecipeIngredient(CreateRecipeIngredientToExistingRecipeDto ingredient)
        {
            if (ingredient is null) throw new BadRequestException("Empty ingredient data");
            var recipeIngredient = _mapper.Map<RecipeIngredient>(ingredient);

            checkAuthorization(_userContentService.User, recipeIngredient.RecipeId);

            var existingIngredient = _dbContext.RecipeIngredients.FirstOrDefault(i => i.RecipeId == ingredient.RecipeId && i.IngredientId == ingredient.IngredientId);
            if (existingIngredient != null) throw new BadRequestException("Ingredient already exists in recipe");

            var existingUnitIngredient = _dbContext.UnitIngredients.FirstOrDefault(u => u.Id == ingredient.UnitIngredientId);
            if (existingUnitIngredient is null) throw new NotFoundException("Unit ingredient not found");

            var existingRecipe = _dbContext.Recipes.FirstOrDefault(r => r.Id == ingredient.RecipeId);
            if (existingRecipe is null) throw new NotFoundException("Recipe not found");

            _dbContext.RecipeIngredients.Add(recipeIngredient);
            _dbContext.SaveChanges();

            return recipeIngredient.Id;
        }

        public void DeleteRecipeIngredient(int id)
        {
            _logger.LogInformation($"Deleting recipeIngredient with id {id}");

            var recipeIngredient = _dbContext.RecipeIngredients.FirstOrDefault(i => i.Id == id);

            if (recipeIngredient is null) throw new NotFoundException("Recipe ingredient not found");

            checkAuthorization(_userContentService.User, recipeIngredient.RecipeId);

            _dbContext.RecipeIngredients.Remove(recipeIngredient);
            _dbContext.SaveChanges();
        }

        public RecipeIngredientDto GetRecipeIngredient(int id)
        {
            var recipeIngredient = _dbContext.RecipeIngredients.Where(i => i.Id == id)
                .Include(i => i.Ingredient)
                .Include(i => i.UnitIngredient)
                .FirstOrDefault();

            if (recipeIngredient is null) throw new NotFoundException("Recipe ingredient not found");

            return _mapper.Map<RecipeIngredientDto>(recipeIngredient);
        }

        public IEnumerable<RecipeIngredientDto> GetRecipeIngredients(int recipeId)
        {
            var recipeIngredients = _dbContext.RecipeIngredients
            .Where(i => i.RecipeId == recipeId)
            .Include(i => i.Ingredient)
            .Include(i => i.UnitIngredient)
            .ToList();

            if (recipeIngredients.Count == 0) throw new NotFoundException("Recipe ingredients not found");

            return _mapper.Map<IEnumerable<RecipeIngredientDto>>(recipeIngredients);
        }

        public void UpdateRecipeIngredient(int id, UpdateRecipeIngredientDto ingredient)
        {
            _logger.LogInformation($"Updating recipe ingredient with id {id}");

            if (ingredient is null) throw new BadRequestException("Empty ingredient data");

            var recipeIngredient = _dbContext.RecipeIngredients.FirstOrDefault(i => i.Id == id);
            if (recipeIngredient is null) throw new NotFoundException("Recipe ingredient not found");

            var existingUnitIngredient = _dbContext.UnitIngredients.FirstOrDefault(u => u.Id == ingredient.UnitIngredientId);
            if (existingUnitIngredient is null) throw new NotFoundException("Unit ingredient not found");

            var existingRecipe = _dbContext.Recipes.FirstOrDefault(r => r.Id == ingredient.RecipeId);
            if (existingRecipe is null) throw new NotFoundException("Recipe not found");
            
            checkAuthorization(_userContentService.User, recipeIngredient.RecipeId);

            recipeIngredient.IngredientId = ingredient.IngredientId;
            recipeIngredient.Quantity = ingredient.Quantity;
            recipeIngredient.UnitIngredientId = ingredient.UnitIngredientId;

            _dbContext.SaveChanges();
        }
        private void checkAuthorization(ClaimsPrincipal user, int recipeId)
        {
            var recipe = _dbContext.Recipes.FirstOrDefault(r => r.Id == recipeId);
            if (recipe is null) throw new NotFoundException("Recipe not found");

            var authorizationResult = _authorizationService.AuthorizeAsync(user, recipe,
                new SameAuthorRequirement()).Result;
            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException("You are not authorized to perform this action");
            }
            else
            {
                recipe.UpdatedAt = DateTime.Now.ToUniversalTime();
            }
        }
    }
}