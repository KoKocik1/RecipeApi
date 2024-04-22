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
using RecipeApi.Tools;


namespace RecipeApi.Service
{
    public class AccountService : IAccountService
    {
        private readonly RecipeDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly AuthenticationSettings _authenticationSettings;

        private readonly IEmailService _emailService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenHelper _tokenHelper;

        public AccountService(RecipeDbContext dBContext,
            IMapper mapper,
            ILogger<AccountService> logger,
            AuthenticationSettings authenticationSettings,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailService emailService,
            ITokenHelper tokenHelper)
        {
            _dbContext = dBContext;
            _mapper = mapper;
            _logger = logger;
            _authenticationSettings = authenticationSettings;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _tokenHelper = tokenHelper;
        }
        public async Task RegisterUserAsync(RegisterUserDto dto)
        {
            if (dto is null) throw new BadRequestException("Empty user data");

            var newUser = _mapper.Map<ApplicationUser>(dto);

            //var claimDateOfBirth = new Claim("DateOfBirth", newUser.DateOfBirth.Value.ToString("yyyy-MM-dd"));
            var claimRole = new Claim(ClaimTypes.Role, "User");

            var result = await _userManager.CreateAsync(newUser, dto.Password);

            if (result.Succeeded)
            {

                await _userManager.AddClaimAsync(newUser, claimRole);

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

        public async Task<TokenData> LoginUserAsync(LoginDto dto)
        {

            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user is null) throw new BadRequestException("Invalid username or password");

            var result = await _signInManager.PasswordSignInAsync(
                user.UserName,
                dto.Password,
                false,
                false);

            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    throw new BadRequestException("User account locked out");
                }

                throw new BadRequestException("Invalid username or password");
            }

            List<string> userRoles = (List<string>)await _userManager.GetRolesAsync(user);

            TokenData tokenData = _tokenHelper.GenerateJwtToken(user, userRoles, _signInManager);
            return tokenData;

            // var claims = new List<Claim>()
            // {
            //     new Claim(ClaimTypes.NameIdentifier, user.Id),
            //     new Claim(ClaimTypes.Name, $"{user.UserName}"),
            //     //new Claim(ClaimTypes.Role, $"{user.Role.Name}"),
            //     new Claim("DateOfBirth", user.DateOfBirth.Value.ToString("yyyy-MM-dd")),
            // };

            // var expires = DateTime.Now.AddMinutes(1);

            // return new LoginResult
            // {
            //     Token = CreateToken(claims, expires),
            //     Expires_at = expires.ToString()
            // };
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
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

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

