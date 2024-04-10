using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RecipeApi.Models;

namespace RecipeApi.IService
{
    public interface IUnitIngredientService
    {
        IEnumerable<UnitIngredientDto> GetUnits();
        UnitIngredientDto GetUnit(int id);
    }
}