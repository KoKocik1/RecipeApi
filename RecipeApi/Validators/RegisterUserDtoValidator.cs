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
				.EmailAddress().WithMessage("Email is not valid")
				.Custom((value, context) =>
				{
					var emailInUse=dbContext.Users.Any(u => u.Email == value);
					if (emailInUse)
					{
						context.AddFailure("Email", "That email is taken");
					}
                });

            RuleFor(x=>x.UserName)
                .NotEmpty().WithMessage("Username is required")
                .MinimumLength(5).WithMessage("Username must be at least 5 characters")
                .MaximumLength(20).WithMessage("Username must be at most 20 characters")
                .Matches("^[a-zA-Z0-9]*$").WithMessage("Username must contain only letters and numbers")
                .Custom((value, context) =>
                {
                    var usernameInUse=dbContext.Users.Any(u => u.UserName == value);
                    if (usernameInUse)
                    {
                        context.AddFailure("Username", "That username is taken");
                    }
                });
            
			RuleFor(x => x.Password)
				.MinimumLength(8).WithMessage("Password must be at least 8 characters")
                .MaximumLength(20).WithMessage("Password must be at most 20 characters")
                .Matches("[A-Z]").WithMessage("Password must contain 1 uppercase letter")
                .Matches("[a-z]").WithMessage("Password must contain 1 lowercase letter")
                .Matches("[0-9]").WithMessage("Password must contain a number")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain special character");

			RuleFor(x => x.ConfirmedPassword)
            .Equal(x => x.Password).WithMessage("Passwords do not match");

            RuleFor(x=>x.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .MaximumLength(50).WithMessage("First name must be at most 50 characters");
            
            RuleFor(x=>x.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .MaximumLength(50).WithMessage("Last name must be at most 50 characters");

            RuleFor(x=>x.DateOfBirth)
                .NotEmpty().WithMessage("Date of birth is required")
                .Must(x=>x<DateTime.Now).WithMessage("Date of birth must be in the past");
			
		}
    }
}