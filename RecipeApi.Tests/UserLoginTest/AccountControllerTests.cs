using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RecipeApi.Authentication;
using RecipeApi.Controllers;
using RecipeApi.Database;
using RecipeApi.Mapping;
using RecipeApi.Models;
using RecipeApi.Service;
using RecipeApi.Validators;
using Xunit;
namespace RecipeApi.Tests.UserLoginTest
{
    public class AccountControllerTests : IDisposable
    {

        private readonly IMapper _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<RecipeMappingProfile>()));
        private readonly ILogger<AccountService> _logger = new LoggerFactory().CreateLogger<AccountService>();
        private readonly ILogger<AccountController> _loggerController = new LoggerFactory().CreateLogger<AccountController>();
        private readonly IPasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

        private readonly AuthenticationSettings _authenticationSettings = new AuthenticationSettings()
        {
            JwtExpireDays = 30,
            JwtIssuer = "test",
            JwtKey = "TEST_KEY_TEST_KEY_TEST_KEY_TEST_KEY_TEST_KEY_"
        };

        private readonly DbContextOptions<RecipeDbContext> _options = new DbContextOptionsBuilder<RecipeDbContext>()
                        .UseInMemoryDatabase(databaseName: "DatabaseTest")
                        .Options;
        public AccountControllerTests()
        {
            using (var context = new RecipeDbContext(_options))
            {
                context.Users.RemoveRange(context.Users);
                context.Roles.RemoveRange(context.Roles);
                context.Roles.Add(new Role { Id = 1, Name = "User" });
                context.SaveChanges();
            }

        }

        public void Dispose()
        {

        }

        [Fact]
        public void CreateAccount_correctLogIn()
        {
            using (var context = new RecipeDbContext(_options))
            {

                // Arrange
                var service = new AccountService(context, _mapper, _logger, _passwordHasher, _authenticationSettings);
                var controller = new AccountController(service, _loggerController);

                var registerResponse = controller.RegisterAccount(new RegisterUserDto
                {
                    FirstName = "Test",
                    LastName = "Test",
                    UserName = "Test",
                    Email = "test@test.test",
                    DateOfBirth = DateTime.Now,
                    Nationality = "Test",
                    Password = "Test123!",
                    ConfirmedPassword = "Test123!"
                });

                Assert.NotNull(registerResponse);
                Assert.IsType<OkResult>(registerResponse);
                Console.WriteLine(context.Users.Count());
                var LoginResponse = controller.Login(new LoginDto
                {
                    Email = "test@test.test",
                    Password = "Test123!"
                });

                Assert.NotNull(LoginResponse);
                Assert.IsType<OkObjectResult>(LoginResponse);
            }
        }
        [Fact]
        public void CreateAccount_NotTheSamePassword()
        {
            using (var context = new RecipeDbContext(_options))
            {
                // Arrange
                var service = new AccountService(context, _mapper, _logger, _passwordHasher, _authenticationSettings);
                var controller = new AccountController(service, _loggerController);

                var userDto = new RegisterUserDto
                {
                    FirstName = "Test",
                    LastName = "Test",
                    UserName = "Test",
                    Email = "test@test.test",
                    DateOfBirth = DateTime.Now,
                    Nationality = "Test",
                    Password = "Test123!",
                    ConfirmedPassword = "Test1234!"
                };
                var _validator = new RegisterUserDtoValidator(context);

                var result = _validator.Validate(userDto);
                // Assert
                Assert.False(result.IsValid);
                Assert.Equal("Passwords do not match", result.Errors.First().ErrorMessage);
            }
        }
        [Fact]
        public void CreateAccount_invalidPasword()
        {
            using (var context = new RecipeDbContext(_options))
            {
                // Arrange
                var service = new AccountService(context, _mapper, _logger, _passwordHasher, _authenticationSettings);
                var controller = new AccountController(service, _loggerController);

                var userDto = new RegisterUserDto
                {
                    FirstName = "Test",
                    LastName = "Test",
                    UserName = "Test",
                    Email = "test@test.test",
                    DateOfBirth = DateTime.Now,
                    Nationality = "Test",
                    Password = "Testtttt!",
                    ConfirmedPassword = "Testtttt!"
                };
                var _validator = new RegisterUserDtoValidator(context);

                var result = _validator.Validate(userDto);
                // Assert
                Assert.False(result.IsValid);
                Assert.Equal("Password must contain a number", result.Errors.First().ErrorMessage);
            }
        }
        [Fact]
        public void CreateAccount_notInvalidEmail()
        {
            using (var context = new RecipeDbContext(_options))
            {
                // Arrange
                var service = new AccountService(context, _mapper, _logger, _passwordHasher, _authenticationSettings);
                var controller = new AccountController(service, _loggerController);

                var userDto = new RegisterUserDto
                {
                    FirstName = "Test",
                    LastName = "Test",
                    UserName = "Test",
                    Email = "testtest.test",
                    DateOfBirth = DateTime.Now,
                    Nationality = "Test",
                    Password = "Test1234!",
                    ConfirmedPassword = "Test1234!"
                };
                var _validator = new RegisterUserDtoValidator(context);

                var result = _validator.Validate(userDto);
                // Assert
                Assert.False(result.IsValid);
                Assert.Equal("Email is not valid", result.Errors.First().ErrorMessage);
            }
        }
        [Fact]
        public void CreateAccount_EmptyEmail()
        {
            using (var context = new RecipeDbContext(_options))
            {
                // Arrange
                var service = new AccountService(context, _mapper,_logger, _passwordHasher, _authenticationSettings);
                var controller = new AccountController(service, _loggerController);

                var userDto = new RegisterUserDto
                {
                    FirstName = "Test",
                    LastName = "Test",
                    UserName = "Test",
                    Email = "",
                    DateOfBirth = DateTime.Now,
                    Nationality = "Test",
                    Password = "Test1234!",
                    ConfirmedPassword = "Test1234!"
                };
                var _validator = new RegisterUserDtoValidator(context);

                var result = _validator.Validate(userDto);
                // Assert
                Assert.False(result.IsValid);
                Assert.Equal("Email is required", result.Errors.First().ErrorMessage);
            }
        }
        [Fact]
        public void CreateAccount_TooShortPassword()
        {
            using (var context = new RecipeDbContext(_options))
            {
                // Arrange
                var service = new AccountService(context, _mapper, _logger, _passwordHasher, _authenticationSettings);
                var controller = new AccountController(service, _loggerController);

                var userDto = new RegisterUserDto
                {
                    FirstName = "Test",
                    LastName = "Test",
                    UserName = "Test",
                    Email = "test@test.test",
                    DateOfBirth = DateTime.Now,
                    Nationality = "Test",
                    Password = "abc",
                    ConfirmedPassword = "abc"
                };
                var _validator = new RegisterUserDtoValidator(context);

                var result = _validator.Validate(userDto);
                // Assert
                Assert.False(result.IsValid);
                Assert.Equal("Password must be at least 8 characters", result.Errors.First().ErrorMessage);
            }
        }
        [Fact]
        public void CreateAccount_TooLongPassword()
        {
            using (var context = new RecipeDbContext(_options))
            {
                // Arrange
                var service = new AccountService(context, _mapper, _logger, _passwordHasher, _authenticationSettings);
                var controller = new AccountController(service, _loggerController);

                var userDto = new RegisterUserDto
                {
                    FirstName = "Test",
                    LastName = "Test",
                    UserName = "Test",
                    Email = "test@test.test",
                    DateOfBirth = DateTime.Now,
                    Nationality = "Test",
                    Password = "abcabcabcabcabcabcabcabc",
                    ConfirmedPassword = "abcabcabcabcabcabcabcabc"
                };
                var _validator = new RegisterUserDtoValidator(context);

                var result = _validator.Validate(userDto);
                // Assert
                Assert.False(result.IsValid);
                Assert.Equal("Password must be at most 20 characters", result.Errors.First().ErrorMessage);
            }
        }
        [Fact]
        public void CreateAccount_ExistingEmail()
        {
            using (var context = new RecipeDbContext(_options))
            {
                // Arrange
                var service = new AccountService(context, _mapper, _logger, _passwordHasher, _authenticationSettings);
                var controller = new AccountController(service, _loggerController);

                var userDto = new RegisterUserDto
                {
                    FirstName = "Test",
                    LastName = "Test",
                    UserName = "Test",
                    Email = "test@test.test",
                    DateOfBirth = DateTime.Now,
                    Nationality = "Test",
                    Password = "Test123!",
                    ConfirmedPassword = "Test123!"
                };
                var _validator = new RegisterUserDtoValidator(context);
                var addResult=controller.RegisterAccount(userDto);
                Assert.NotNull(addResult);
                Assert.IsType<OkResult>(addResult);

                userDto.UserName = "Test2";
                var result = _validator.Validate(userDto);
                // Assert
                Assert.False(result.IsValid);
                Assert.Equal("That email is taken", result.Errors.First().ErrorMessage);
            }
        }
         [Fact]
        public void CreateAccount_ExistingUsername()
        {
            using (var context = new RecipeDbContext(_options))
            {
                // Arrange
                var service = new AccountService(context, _mapper, _logger, _passwordHasher, _authenticationSettings);
                var controller = new AccountController(service, _loggerController);

                var userDto = new RegisterUserDto
                {
                    FirstName = "Test",
                    LastName = "Test",
                    UserName = "Test",
                    Email = "test@test.test",
                    DateOfBirth = DateTime.Now,
                    Nationality = "Test",
                    Password = "Test123!",
                    ConfirmedPassword = "Test123!"
                };
                var _validator = new RegisterUserDtoValidator(context);
                var addResult=controller.RegisterAccount(userDto);
                Assert.NotNull(addResult);
                Assert.IsType<OkResult>(addResult);

                userDto.Email = "test2@test.test";
                var result = _validator.Validate(userDto);
                // Assert
                Assert.False(result.IsValid);
                Assert.Equal("That username is taken", result.Errors.First().ErrorMessage);
            }
        }
    }
}