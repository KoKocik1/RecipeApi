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
using RecipeApi.Models.Recipe;

namespace RecipeApi.Service
{
    public class RecipeService : IRecipeService
    {
        private readonly RecipeDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IUserContentService _userContentService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IIngrededientService _ingredientService;
        private readonly IRecipeIngredientService _recipeIngredientService;
        private readonly IRecipeInstructionService _recipeInstructionService;
        public RecipeService(
            RecipeDbContext context,
            IMapper mapper,
            ILogger<RecipeService> logger,
            IUserContentService userContentService,
            IAuthorizationService authorizationService,
            IIngrededientService ingredientService,
            IRecipeIngredientService recipeIngredientService,
            IRecipeInstructionService recipeInstructionService)
        {
            _dbContext = context;
            _mapper = mapper;
            _logger = logger;
            _userContentService = userContentService;
            _authorizationService = authorizationService;
            _recipeIngredientService = recipeIngredientService;
            _recipeInstructionService = recipeInstructionService;
            _ingredientService = ingredientService;
        }

        public int AddRecipe(CreateRecipeDto recipe)
        {
            if (recipe is null)
                throw new BadRequestException("Empty recipe data");

            var recipeEntity = _mapper.Map<Recipe>(recipe);
            recipeEntity.UserId = _userContentService.GetUserId;
            recipeEntity.CreatedAt = DateTime.Now.ToUniversalTime();
            _dbContext.Recipes.Add(recipeEntity);
            _dbContext.SaveChanges();

            return recipeEntity.Id;
        }
        // private async Task<int> AddRecipe(CreateRecipeDto recipe)
        // {
        //     if (recipe is null)
        //         throw new BadRequestException("Empty recipe data");

        //     var recipeEntity = _mapper.Map<Recipe>(recipe);
        //     recipeEntity.UserId = _userContentService.GetUserId;
        //     recipeEntity.CreatedAt = DateTime.Now.ToUniversalTime();
        //     await _dbContext.Recipes.AddAsync(recipeEntity);
        //     await _dbContext.SaveChangesAsync(); //todo: check if it is needed

        //     return recipeEntity.Id;
        // }

    //     public async int AddCompleteRecipe(CreateCompleteRecipeDto recipe)
    //     {

    //         if (recipe is null) throw new BadRequestException("Empty recipe data");

    //         using (var transaction = _dbContext.Database.BeginTransaction())
    //         {
    //             var createRecipeDto = _mapper.Map<CreateRecipeDto>(recipe); //TODO: create mapper
    //             int newRecipeId = await AddRecipe(createRecipeDto);

    //             foreach (var ingredient in recipe.Ingredients)
    //             {
    //                 var existedIngredient = _ingredientService.GetIngredient(ingredient.IngredientId);
    //                 if (existedIngredient is null)
    //                 {
    //                     var newIngredient = _mapper.Map<CreateIngredientDto>(ingredient); //TODO: create mapper
    //                     ingredient.IngredientId = _ingredientService.AddIngredient(newIngredient);
    //                 }

    //             }


    //             var ingredientId = _ingredientService.AddIngredient(ingredient);
    //             var createIngredient
    //                 var recipeIngredient = new CreateRecipeIngredientToExistingRecipeDto
    //                 {
    //                     RecipeId = recipeEntity.Id,
    //                     IngredientId = ingredientId,
    //                     Quantity = ingredient.Quantity,
    //                     UnitIngredientId = ingredient.UnitIngredientId
    //                 };
    //             _recipeIngredientService.AddRecipeIngredient(recipeIngredient);
    //         }

    //         foreach (var instruction in recipe.Instructions)
    //         {
    //             var recipeInstruction = new CreateRecipeInstructionToExistingRecipeDto
    //             {
    //                 RecipeId = recipeEntity.Id,
    //                 Description = instruction.Description,
    //                 Order = instruction.Order
    //             };
    //             _recipeInstructionService.AddRecipeInstruction(recipeInstruction);
    //         }

    //         return recipeEntity.Id;
    //     }
    // }

    public void DeleteRecipe(int id)
    {
        _logger.LogInformation($"Deleting recipe with id {id}");
        var recipe = _dbContext.Recipes.FirstOrDefault(r => r.Id == id);
        if (recipe is null) throw new NotFoundException("Recipe not found");

        checkAuthorization(_userContentService.User, recipe);

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

    public IEnumerable<RecipeDto> GetRecipesByAuthor(int authorId)
    {
        var recipes = _dbContext.Recipes.Where(r => r.UserId == authorId)
        .Include(r => r.User)
        .Include(r => r.Ingredients)
        .Include(r => r.Instructions)
        .ToList();

        if (recipes.Count == 0) throw new NotFoundException("No recipes found for this author");

        return _mapper.Map<IEnumerable<RecipeDto>>(recipes);
    }

    public void UpdateRecipe(int id, UpdateRecipeDto recipe)
    {
        _logger.LogInformation($"Updating recipe with id {id}");

        if (recipe is null) throw new BadRequestException("Invalid recipe");

        var recipeEntity = _dbContext.Recipes.FirstOrDefault(r => r.Id == id);
        if (recipeEntity is null) throw new NotFoundException("Recipe not found");

        checkAuthorization(_userContentService.User, recipeEntity);

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
    private void checkAuthorization(ClaimsPrincipal user, Recipe recipe)
    {
        var authorizationResult = _authorizationService.AuthorizeAsync(user, recipe,
            new SameAuthorRequirement()).Result;
        if (!authorizationResult.Succeeded)
        {
            throw new ForbidException("You are not authorized to perform this action");
        }
    }
}
}