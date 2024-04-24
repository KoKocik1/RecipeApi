using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
            _dbContext.Recipes.Add(recipeEntity);
            _dbContext.SaveChanges();

            return recipeEntity.Id;
        }
        public int CloneRecipe(int recipeId, string userId)
        {
            var recipe = getRecipe(recipeId);

            if (recipe is null) throw new NotFoundException("Recipe not found");

            var cloneRecipe = _mapper.Map<Recipe>(recipe);

            cloneRecipe.UserId = userId;

            _dbContext.Recipes.Add(cloneRecipe);
            _dbContext.SaveChanges();

            return cloneRecipe.Id;
        }

        public void DeleteRecipe(int recipeId, string userId)
        {
            _logger.LogInformation($"Deleting recipe with id {recipeId} by user {userId}");

            var recipe = _dbContext.Recipes.FirstOrDefault(r => r.Id == recipeId);
            if (recipe is null) throw new NotFoundException("Recipe not found");

            if (userId != recipe.UserId) throw new ForbidException("You are not authorized to perform this action");

            _dbContext.Recipes.Remove(recipe);
            _dbContext.SaveChanges();
        }

        public RecipeDetailsDto GetRecipe(int recipeId)
        {
            var recipe = getRecipe(recipeId);

            if (recipe is null) throw new NotFoundException("Recipe not found");

            recipe.Instructions = recipe.Instructions.OrderBy(i => i.Order).ToList();

            return _mapper.Map<RecipeDetailsDto>(recipe);
        }

        public PageResult<RecipeDto> GetRecipes(RecipeQuery query)
        {
            //code in short way but hard to read
            // var recipesQuery = _dbContext.Recipes
            //         .Where(r =>
            //             (query.SearchPhase == null || (
            //                 r.Title.ToLower().Contains(query.SearchPhase.ToLower()) ||
            //                 r.Description.ToLower().Contains(query.SearchPhase.ToLower())
            //             )) && (query.IngredientsIds == null ||
            //                 query.IngredientsIds.All(ingredientId =>
            //                 r.Ingredients.Any(i => i.IngredientId == ingredientId))
            //             ));

            var querySearchByPhrase = _dbContext.Recipes
                .Where(r =>
                    query.SearchPhase == null || (
                        r.Title.ToLower().Contains(query.SearchPhase.ToLower()) ||
                        r.Description.ToLower().Contains(query.SearchPhase.ToLower())
                    ));

            var querySearchByIngredients = querySearchByPhrase;

            if (query.IngredientsIds != null && query.IngredientsIds.Any())
            {
                querySearchByIngredients = querySearchByPhrase
                    .Where(r =>
                        query.IngredientsIds.All(ingredientId =>
                            r.Ingredients.Any(i => i.IngredientId == ingredientId)));
            }

            var recipesQuery = querySearchByIngredients;

            if (recipesQuery is null) throw new NotFoundException("No recipes found");


                var columnsSelection = new Dictionary<string, Expression<Func<Recipe, object>>>
                {
                    {nameof(Recipe.Title), r=>r.Title},
                    {nameof(Recipe.CreatedAt), r=>r.CreatedAt},
                };

                var selectedColumn = columnsSelection[query.SortBy.ToString()];

                recipesQuery = query.SortDirection == SortDirections.ASC
                    ? recipesQuery.OrderBy(selectedColumn)
                    : recipesQuery.OrderByDescending(selectedColumn);
            

            var recipes = recipesQuery
                            .Skip(query.PageSize * (query.PageNumber - 1))
                            .Take(query.PageSize)
                            .ToList();

            if (recipes.Count == 0) throw new NotFoundException("No recipes found");

            var totalItemsCount = recipesQuery.Count();

            if (totalItemsCount == 0) throw new NotFoundException("No recipes found");

            var recipesDto = _mapper.Map<List<RecipeDto>>(recipes);

            return new PageResult<RecipeDto>(recipesDto, totalItemsCount, query.PageSize, query.PageNumber);

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

        public void UpdateRecipe(int recipeId, UpdateRecipeDto recipe, string userId)
        {
            _logger.LogInformation($"Updating recipe with id {recipeId} by user {userId}");

            if (recipe is null) throw new BadRequestException("Invalid recipe");

            var recipeEntity = _dbContext.Recipes
                .Include(r => r.User)
                .FirstOrDefault(r => r.Id == recipeId);

            if (recipeEntity is null) throw new NotFoundException("Recipe not found");

            if (userId != recipeEntity.UserId) throw new ForbidException("You are not authorized to perform this action");

            _dbContext.RecipeIngredients.RemoveRange(_dbContext.RecipeIngredients.Where(ri => ri.RecipeId == recipeEntity.Id));
            _dbContext.RecipeInstructions.RemoveRange(_dbContext.RecipeInstructions.Where(ri => ri.RecipeId == recipeEntity.Id));

            _mapper.Map(recipe, recipeEntity);

            _dbContext.Recipes.Update(recipeEntity);
            _dbContext.SaveChanges();
        }

        private Recipe getRecipe(int recipeId)
        {
            return _dbContext.Recipes
            .Include(r => r.Ingredients)
                .ThenInclude(ri => ri.Ingredient)
            .Include(r => r.Ingredients)
                .ThenInclude(ri => ri.UnitIngredient)
            .Include(r => r.Instructions)
            .Include(r => r.User)
            .FirstOrDefault(r => r.Id == recipeId);
        }
    }
}