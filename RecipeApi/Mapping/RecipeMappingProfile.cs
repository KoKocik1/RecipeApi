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

            CreateMap<RegisterUserDto, ApplicationUser>();
            CreateMap<ApplicationUser, RecipeAuthorDto>();
            CreateMap<ApplicationUser, UserPrivateDto>();
            CreateMap<ApplicationUser, UserPublicDto>();


            //Dto to database model

            //ingredients
            CreateMap<CreateIngredientDto, Ingredient>();
            CreateMap<UpdateIngredientDto, Ingredient>();

            //recipe instructions
            CreateMap<CreateRecipeInstructionDto, RecipeInstruction>();

            CreateMap<UpdateRecipeInstructionDto, RecipeInstruction>();

            //recipe ingredients
            CreateMap<CreateRecipeIngredientDto, RecipeIngredient>();
            CreateMap<UpdateRecipeIngredientDto, RecipeIngredient>();

            //recipes
            CreateMap<UpdateRecipeDto, Recipe>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now.ToUniversalTime()))
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore());
            CreateMap<CreateRecipeDto, Recipe>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now.ToUniversalTime()))
                .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.Ingredients))
                .ForMember(dest => dest.Instructions, opt => opt.MapFrom(src => src.Instructions));

        }
    }
}