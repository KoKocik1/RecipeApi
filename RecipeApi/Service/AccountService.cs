using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.IdentityModel.Tokens;
using RecipeApi.Authentication;
using RecipeApi.Database;
using RecipeApi.Exceptions;
using RecipeApi.IService;
using RecipeApi.Models;
using RecipeApi.Models.User;


namespace RecipeApi.Service
{
    public class AccountService : IAccountService
    {
        private readonly RecipeDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly AuthenticationSettings _authenticationSettings;

        private readonly IEmailService _emailService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountService(RecipeDbContext dBContext,
            IMapper mapper,
            ILogger<AccountService> logger,
            AuthenticationSettings authenticationSettings,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IEmailService emailService)
        {
            _dbContext = dBContext;
            _mapper = mapper;
            _logger = logger;
            _authenticationSettings = authenticationSettings;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }
        public async Task RegisterUserAsync(RegisterUserDto dto)
        {
            if (dto is null) throw new BadRequestException("Empty user data");

            var newUser = new IdentityUser
            {
                UserName = dto.UserName,
                Email = dto.Email
            };

            var result = await _userManager.CreateAsync(newUser, dto.Password);

            if (result.Succeeded)
            {
                var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);

                var userIdEncoded = HttpUtility.UrlEncode(newUser.Id);
                var tokenEncoded = HttpUtility.UrlEncode(confirmationToken);
                var confirmationTokenLink = new Uri($"http://localhost:5284/api/account/confirmEmail?userId={userIdEncoded}&token={tokenEncoded}");

                await _emailService.SendAsync(
                    "recipeapiconfirm@gmail.com",
                    newUser.Email,
                    "Confirm your email",
                    $"Click <a href=\"{confirmationTokenLink}\">here</a> to confirm your email");

                
            }
            else
            {
                var errors = result.Errors.Select(e => e.Description);
                throw new BadRequestException($"User registration failed: {string.Join(", ", errors)}");
            }
        }

        public async Task<LoginResult> LoginUserAsync(LoginDto dto)
        {

            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user is null) throw new BadRequestException("Invalid username or password");

            var result = await _signInManager.PasswordSignInAsync(
                user.UserName,
                dto.Password,
                dto.RememberMe,
                false);

            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    throw new BadRequestException("User account locked out");
                }

                throw new BadRequestException("Invalid username or password");
            }


            var userDetails = await _dbContext.UsersDetails.FirstOrDefaultAsync(x => x.UserId == user.Id);
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, $"{user.UserName}"),
                //new Claim(ClaimTypes.Role, $"{user.Role.Name}"),
                // new Claim("DateOfBirth", userDetails.DateOfBirth.Value.ToString("yyyy-MM-dd")),
            };

            var expires = DateTime.Now.AddMinutes(10);

            return new LoginResult
            {
                Token = CreateToken(claims, expires),
                Expires_at = expires.ToString()
            };
        }

        public async Task<string> ConfirmEmailAsync(string userId, string token)
        {

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) throw new BadRequestException("User not found");

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                throw new BadRequestException($"Email confirmation failed: {string.Join(", ", errors)}");
            }

            return "Email confirmed";
        }

        public async Task SingOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
        private string CreateToken(IEnumerable<Claim> claims, DateTime expires)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims,
                expires,
                signingCredentials: cred);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
    }
}

