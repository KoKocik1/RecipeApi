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

        public int AddInstruction(CreateRecipeInstructionDto instructionDto)
        {
            var instruction = _mapper.Map<RecipeInstruction>(instructionDto);
            _dbContext.Recipe_Instructions.Add(instruction);
            _dbContext.SaveChanges();

            return instruction.Id;
        }

        public int AddRecipeInstruction(CreateRecipeInstructionDto recipeInstruction)
        {
            throw new NotImplementedException();
        }

        public void DeleteInstruction(int id)
        {
            _logger.LogInformation($"Deleting instruction with id {id}");

            var instruction = _dbContext.Recipe_Instructions.FirstOrDefault(i => i.Id == id);

            if (instruction is null) throw new NotFoundException("Instruction not found");

            _dbContext.Recipe_Instructions.Remove(instruction);
            _dbContext.SaveChanges();
        }

        public void DeleteRecipeInstruction(int id)
        {
            throw new NotImplementedException();
        }

        public RecipeInstructionDto GetRecipeInstruction(int id)
        {
            var instruction = _dbContext.Recipe_Instructions.FirstOrDefault(i => i.Id == id);

            if (instruction is null) throw new NotFoundException("Instruction not found");

            return _mapper.Map<RecipeInstructionDto>(instruction);
        }

        public IEnumerable<RecipeInstructionDto> GetRecipeInstructions()
        {
            var instructions = _dbContext.Recipe_Instructions.ToList();
            return _mapper.Map<IEnumerable<RecipeInstructionDto>>(instructions);
        }

        public IEnumerable<RecipeInstructionDto> GetRecipeInstructionsByRecipeId(int recipeId)
        {
            var instructions = _dbContext.Recipe_Instructions.Where(i => i.RecipeId == recipeId).ToList();
            return _mapper.Map<IEnumerable<RecipeInstructionDto>>(instructions);
        }


        public void UpdateRecipeInstruction(int id, RecipeInstructionDto instruction)
        {
            var instructionToUpdate = _dbContext.Recipe_Instructions.FirstOrDefault(i => i.Id == id);

            if (instructionToUpdate is null) throw new NotFoundException("Instruction not found");

            instructionToUpdate.Instruction = instruction.Instruction;
            instructionToUpdate.Order = instruction.Order;

            _dbContext.Recipe_Instructions.Update(instructionToUpdate);
            _dbContext.SaveChanges();
        }
    }
}