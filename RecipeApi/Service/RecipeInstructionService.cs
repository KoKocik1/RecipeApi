using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using RecipeApi.Authentication;
using RecipeApi.Database;
using RecipeApi.Exceptions;
using RecipeApi.IService;
using RecipeApi.Models;

namespace RecipeApi.Service
{
    public class RecipeInstructionService : IRecipeInstructionService
    {
        private readonly RecipeDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContentService _userContentService;


        public RecipeInstructionService(RecipeDbContext context,
        IMapper mapper,
        ILogger<RecipeInstructionService> logger,
        IAuthorizationService authorizationService,
        IUserContentService userContentService)
        {
            _dbContext = context;
            _mapper = mapper;
            _logger = logger;
            _authorizationService = authorizationService;
            _userContentService = userContentService;
        }

        public int AddRecipeInstruction(CreateRecipeInstructionToExistingRecipeDto instructionDto)
        {
            if (instructionDto is null) throw new BadRequestException("Empty instruction data");

            var instruction = _mapper.Map<RecipeInstruction>(instructionDto);

            checkAuthorization(_userContentService.User, instructionDto.RecipeId);

            _dbContext.RecipeInstructions.Add(instruction);
            _dbContext.SaveChanges();

            return instruction.Id;
        }

        public void DeleteRecipeInstruction(int id)
        {
            _logger.LogInformation($"Deleting instruction with id {id}");

            var instruction = _dbContext.RecipeInstructions.FirstOrDefault(i => i.Id == id);

            if (instruction is null) throw new NotFoundException("Instruction not found");

            checkAuthorization(_userContentService.User, instruction.RecipeId);

            _dbContext.RecipeInstructions.Remove(instruction);
            _dbContext.SaveChanges();
        }

        public RecipeInstructionDto GetRecipeInstruction(int id)
        {
            var instruction = _dbContext.RecipeInstructions.FirstOrDefault(i => i.Id == id);

            if (instruction is null) throw new NotFoundException("Instruction not found");

            return _mapper.Map<RecipeInstructionDto>(instruction);
        }

        public IEnumerable<RecipeInstructionDto> GetRecipeInstructionsByRecipeId(int recipeId)
        {
            var instructions = _dbContext.RecipeInstructions.Where(i => i.RecipeId == recipeId).OrderBy(i => i.Order).ToList();

            if (instructions.Count == 0) throw new NotFoundException("Instructions not found");

            return _mapper.Map<IEnumerable<RecipeInstructionDto>>(instructions);
        }


        public void UpdateRecipeInstruction(int id, UpdateRecipeInstructionDto instruction)
        {

            var instructionToUpdate = _dbContext.RecipeInstructions.FirstOrDefault(i => i.Id == id);

            if (instructionToUpdate is null) throw new NotFoundException("Instruction not found");

            checkAuthorization(_userContentService.User, instructionToUpdate.RecipeId);

            instructionToUpdate.Instruction = instruction.Instruction;
            instructionToUpdate.Order = instruction.Order;

            _dbContext.RecipeInstructions.Update(instructionToUpdate);
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