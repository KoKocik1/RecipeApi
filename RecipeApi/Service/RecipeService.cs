using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RecipeApi.Authentication;
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
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<ApplicationUser> _userManager;
        public RecipeService(
            RecipeDbContext context,
            IMapper mapper,
            ILogger<RecipeService> logger,
            IAuthorizationService authorizationService,
            UserManager<ApplicationUser> userManager)
        {
            _dbContext = context;
            _mapper = mapper;
            _logger = logger;
            _authorizationService = authorizationService;
            _userManager = userManager;
        }
        public int AddRecipe(CreateRecipeDto recipe, string userId)
        {

            if (recipe is null) throw new BadRequestException("Empty recipe data");

            var recipeEntity = _mapper.Map<Recipe>(recipe);
            recipeEntity.UserId = userId;
            recipeEntity.CreatedAt = DateTime.Now.ToUniversalTime();
            _dbContext.Recipes.Add(recipeEntity);
            _dbContext.SaveChanges();

            return recipeEntity.Id;
        }

        public void DeleteRecipe(int id, string userId)
        {
            _logger.LogInformation($"Deleting recipe with id {id}");
            var recipe = _dbContext.Recipes.FirstOrDefault(r => r.Id == id);
            if (recipe is null) throw new NotFoundException("Recipe not found");

            if(userId!=recipe.UserId) throw new ForbidException("You are not authorized to perform this action");

            var recipeIngredients = _dbContext.RecipeIngredients.Where(ri => ri.RecipeId == id);
            var recipeInstructions = _dbContext.RecipeInstructions.Where(ri => ri.RecipeId == recipe.Id);
            
            _dbContext.Recipes.Remove(recipe);
            _dbContext.RecipeIngredients.RemoveRange(recipeIngredients);
            _dbContext.RecipeInstructions.RemoveRange(recipeInstructions);
            _dbContext.SaveChanges();
        }

        public RecipeDetailsDto GetRecipe(int id)
        {
            var recipe = _dbContext.Recipes
            .Include(r => r.Ingredients)
                .ThenInclude(ri => ri.Ingredient)
            .Include(r => r.Ingredients)
                .ThenInclude(ri => ri.UnitIngredient)
            .Include(r => r.Instructions)
            .Include(r => r.User)
            .FirstOrDefault(r => r.Id == id);

            if (recipe is null) throw new NotFoundException("Recipe not found");

            recipe.Instructions = recipe.Instructions.OrderBy(i => i.Order).ToList();

            return _mapper.Map<RecipeDetailsDto>(recipe);
        }

        public IEnumerable<RecipeDto> GetRecipes()
        {
            var recipes = _dbContext.Recipes.ToList();

            if (recipes is null) throw new NotFoundException("No recipes found");

            return _mapper.Map<IEnumerable<RecipeDto>>(recipes);
        }

        public IEnumerable<RecipeDto> GetRecipesByAuthor(string authorId)
        {
            var recipes = _dbContext.Recipes.Where(r => r.UserId == authorId)
            .Include(r => r.User)
            .Include(r => r.Ingredients)
            .Include(r => r.Instructions)
            .ToList();

            if (recipes.Count == 0) throw new NotFoundException("No recipes found for this author");

            return _mapper.Map<IEnumerable<RecipeDto>>(recipes);
        }

        public void UpdateRecipe(int id, UpdateRecipeDto recipe, string userId)
        {
            _logger.LogInformation($"Updating recipe with id {id}");

            if (recipe is null) throw new BadRequestException("Invalid recipe");

            var recipeEntity = _dbContext.Recipes.FirstOrDefault(r => r.Id == id);
            if (recipeEntity is null) throw new NotFoundException("Recipe not found");

            if(userId!=recipeEntity.UserId) throw new ForbidException("You are not authorized to perform this action");
            

            recipeEntity.Title = recipe.Title;
            recipeEntity.Description = recipe.Description;
            recipeEntity.Portions = recipe.Portions;
            recipeEntity.TimeToCook = recipe.TimeToCook;
            recipeEntity.UpdatedAt = DateTime.Now.ToUniversalTime();

            // recipeEntity.Instructions = _mapper.Map<List<RecipeInstruction>>(recipe.Instructions);
            // recipeEntity.Ingredients = _mapper.Map<List<RecipeIngredient>>(recipe.Ingredients);

            _dbContext.Recipes.Update(recipeEntity);
            _dbContext.SaveChanges();
        }
    }
}