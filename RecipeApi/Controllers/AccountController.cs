using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RecipeApi.Authentication;
using RecipeApi.IService;
using RecipeApi.Models;
using RecipeApi.Models.User;

namespace RecipeApi.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountService _acconntService;


        public AccountController(
            IAccountService accountService,
            ILogger<AccountController> logger)
        {
            _acconntService = accountService;
            _logger = logger;
        }
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult> RegisterAccount([FromBody] RegisterUserDto dto)
        {
            await _acconntService.RegisterUserAsync(dto);
            return Ok();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login([FromBody] LoginDto dto)
        {
            TokenData result = await _acconntService.LoginUserAsync(dto);
            return Ok(new
            {
                access_token = result.Token
            });
        }

        [HttpGet("confirmEmail")]
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
        {
            var resutl=await _acconntService.ConfirmEmailAsync(userId, token);
            return Ok(resutl);
        }

        [HttpPost("logOut")]
        [Authorize]
        public async Task<ActionResult> LogOut()
        {
            await _acconntService.SingOutAsync();
            return Ok();
        }
    }
}