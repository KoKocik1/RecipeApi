using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using RecipeApi.Database;
using RecipeApi.Models;

namespace RecipeApi.Validators
{
    public class UpdateRecipeIngredientDtoValidator : AbstractValidator<UpdateRecipeIngredientDto>
    {
        public UpdateRecipeIngredientDtoValidator(RecipeDbContext dbContext)
        {

            RuleFor(x => x.Quantity)
                .NotEmpty().WithMessage("Quantity is required")
                .Must(x => x > 0).WithMessage("Quantity must be greater than 0");

            RuleFor(x => x.UnitIngredientId)
                .NotEmpty().WithMessage("UnitIngredientId is required");

            RuleFor(x => x.IngredientId)
                .NotEmpty().WithMessage("IngredientId is required");

        }
    }

}