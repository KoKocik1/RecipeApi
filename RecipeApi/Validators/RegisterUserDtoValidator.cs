using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using RecipeApi.Database;
using RecipeApi.Models;

namespace RecipeApi.Validators
{
    public class RegisterUserDtoValidator: AbstractValidator<RegisterUserDto>
	{
        public RegisterUserDtoValidator(RecipeDbContext dbContext) { 
		
			RuleFor(x=>x.Email)
				.NotEmpty().WithMessage("Email is required")
				.EmailAddress().WithMessage("Email is not valid");

            
			RuleFor(x => x.Password)
				.MinimumLength(8).WithMessage("Password must be at least 8 characters")
                .MaximumLength(20).WithMessage("Password must be at most 20 characters")
                .Matches("[A-Z]").WithMessage("Password must contain 1 uppercase letter")
                .Matches("[a-z]").WithMessage("Password must contain 1 lowercase letter")
                .Matches("[0-9]").WithMessage("Password must contain a number")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain special character");

			RuleFor(x => x.ConfirmedPassword)
            .Equal(x => x.Password).WithMessage("Passwords do not match");


			RuleFor(x => x.Email)
				.Custom((value, context) =>
				{
					var emailInUse=dbContext.Users.Any(u => u.Email == value);
					if (emailInUse)
					{
						context.AddFailure("Email", "That email is taken");
					}
                });
		}
    }
}