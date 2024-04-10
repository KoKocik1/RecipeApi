using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RecipeApi.IService;
using RecipeApi.Models;

namespace RecipeApi.Controllers
{
    [Route("api/recipe-unit")]
    [ApiController]
    public class UnitIngredientController : ControllerBase
    {
        private readonly ILogger<UnitIngredientController> _logger;
        private readonly IUnitIngredientService _unitIngredientService;

        public UnitIngredientController(ILogger<UnitIngredientController> logger, IUnitIngredientService unitIngredientService)
        {
            _logger = logger;
            _unitIngredientService = unitIngredientService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<UnitIngredientDto>> Get()
        {
            var units = _unitIngredientService.GetUnits();
            return Ok(units);
        }
        
        [HttpGet("{id}")]
        public ActionResult<UnitIngredientDto> Get(int id)
        {
            var unit = _unitIngredientService.GetUnit(id);
            return Ok(unit);
        }
    }
}