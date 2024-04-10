using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeApi.IService
{
    public interface IUnitIngredientService
    {
        IEnumerable<UnitIngredientDto> GetUnits();
        UnitIngredientDto GetUnit(int id);
    }
}