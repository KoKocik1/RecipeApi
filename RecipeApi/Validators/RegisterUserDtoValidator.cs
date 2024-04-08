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
				.NotEmpty()
				.EmailAddress();

			RuleFor(x => x.Password)
				.MinimumLength(8)
                .MaximumLength(20)
                .Matches("[A-Z]").WithMessage("Password must contain 1 uppercase letter")
                .Matches("[a-z]").WithMessage("Password must contain 1 lowercase letter")
                .Matches("[0-9]").WithMessage("Password must contain a number")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain special character");

			RuleFor(x => x.ConfirmedPassword == x.Password);

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