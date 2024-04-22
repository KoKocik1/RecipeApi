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
    public class UnitIngredientService : IUnitIngredientService
    {
        private readonly RecipeDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        
        public UnitIngredientService(RecipeDbContext context, IMapper mapper, ILogger<UnitIngredientService> logger)
        {
            _dbContext = context;
            _mapper = mapper;
            _logger = logger;
        }
        public UnitIngredientDto GetUnit(int id)
        {
            var unit = _dbContext.UnitIngredients.FirstOrDefault(i => i.Id == id);
            if (unit is null) throw new NotFoundException("Unit not found");
            return _mapper.Map<UnitIngredientDto>(unit);

        }

        public IEnumerable<UnitIngredientDto> GetUnits()
        {
            var units = _dbContext.UnitIngredients.ToList();
            return _mapper.Map<IEnumerable<UnitIngredientDto>>(units);
        }
    }
}