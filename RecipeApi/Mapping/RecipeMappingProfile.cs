using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using RecipeApi.Database;
using RecipeApi.Models;

namespace RecipeApi.Mapping
{
    public class RecipeMappingProfile : Profile
    {
        public RecipeMappingProfile(){
            CreateMap<Ingredient, IngredientDto>();
            CreateMap<CreateIngredientDto, Ingredient>();
        }
    }
}