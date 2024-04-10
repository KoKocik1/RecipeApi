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
    public class RecipeInstructionService : IRecipeInstructionService
    {
        private readonly RecipeDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public RecipeInstructionService(RecipeDbContext context, IMapper mapper, ILogger<RecipeInstructionService> logger)
        {
            _dbContext = context;
            _mapper = mapper;
            _logger = logger;
        }

        public int AddRecipeInstruction(CreateRecipeInstructionToExistingRecipeDto instructionDto)
        {
            if(instructionDto is null) throw new BadRequestException("Empty instruction data");

            var instruction = _mapper.Map<RecipeInstruction>(instructionDto);
            _dbContext.RecipeInstructions.Add(instruction);
            _dbContext.SaveChanges();

            return instruction.Id;
        }

        public void DeleteRecipeInstruction(int id)
        {
            _logger.LogInformation($"Deleting instruction with id {id}");

            var instruction = _dbContext.RecipeInstructions.FirstOrDefault(i => i.Id == id);

            if (instruction is null) throw new NotFoundException("Instruction not found");

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
            
            if (instructions.Count==0) throw new NotFoundException("Instructions not found");

            return _mapper.Map<IEnumerable<RecipeInstructionDto>>(instructions);
        }


        public void UpdateRecipeInstruction(int id, UpdateRecipeInstructionDto instruction)
        {
            var instructionToUpdate = _dbContext.RecipeInstructions.FirstOrDefault(i => i.Id == id);

            if (instructionToUpdate is null) throw new NotFoundException("Instruction not found");

            instructionToUpdate.Instruction = instruction.Instruction;
            instructionToUpdate.Order = instruction.Order;

            _dbContext.RecipeInstructions.Update(instructionToUpdate);
            _dbContext.SaveChanges();
        }
    }
}