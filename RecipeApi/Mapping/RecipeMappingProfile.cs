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
        public RecipeMappingProfile()
        {
            //Database model to dto
            CreateMap<Ingredient, IngredientDto>();
            CreateMap<Recipe, RecipeDto>();
            CreateMap<Recipe, RecipeDetailsDto>();
            CreateMap<RecipeInstruction, RecipeInstructionDto>();
            CreateMap<UnitIngredient, UnitIngredientDto>();
            // CreateMap<RecipeIngredient, RecipeIngredientDto>();
            CreateMap<RecipeIngredient, RecipeIngredientDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Ingredient.Name))
                .ForMember(dest => dest.Verified, opt => opt.MapFrom(src => src.Ingredient.Verified))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.UnitIngredient.Type))
                .ForMember(dest => dest.UnitIngredientId, opt => opt.MapFrom(src => src.UnitIngredient.Id));
            // CreateMap<User, RecipeAuthorDto>();
            // CreateMap<User, UserPrivateDto>();
            // CreateMap<User, UserPublicDto>();


            //Dto to database model

            //ingredients
            CreateMap<CreateIngredientDto, Ingredient>();
            CreateMap<UpdateIngredientDto, Ingredient>();

            //recipe instructions
            CreateMap<CreateRecipeInstructionToExistingRecipeDto, RecipeInstruction>();
            CreateMap<CreateRecipeInstructionToNewRecipeDto, RecipeInstruction>();
            CreateMap<UpdateRecipeInstructionDto, RecipeInstruction>();

            //recipe ingredients
            CreateMap<CreateRecipeIngredientToExistingRecipeDto, RecipeIngredient>();
            CreateMap<CreateRecipeIngredientToNewRecipeDto, RecipeIngredient>();
            CreateMap<UpdateRecipeIngredientDto, RecipeIngredient>();

            //recipes
            CreateMap<CreateRecipeDto, Recipe>();
            CreateMap<UpdateRecipeDto, Recipe>();

        }
    }
}