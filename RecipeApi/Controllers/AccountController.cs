using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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


        public AccountController(IAccountService accountService, ILogger<AccountController> logger)
        {
            _acconntService = accountService;
            _logger = logger;
        }
        [HttpPost("register")]
        public async Task<ActionResult> RegisterAccount([FromBody] RegisterUserDto dto)
        {
            await _acconntService.RegisterUserAsync(dto);
            return Ok();
        }
        [HttpPost]
        public async Task<ActionResult> Login([FromBody] LoginDto dto)
        {
            LoginResult result = await _acconntService.LoginUserAsync(dto);
            return Ok(new
            {
                access_token = result.Token,
                expires_at = result.Expires_at
            });
        }

        [HttpGet("confirmEmail")]
        public async Task<ActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
        {
            var resutl=await _acconntService.ConfirmEmailAsync(userId, token);
            return Ok(resutl);
        }

        [HttpPost("signOut")]
        public async Task<ActionResult> SignOut()
        {
            await _acconntService.SingOutAsync();
            return Ok();
        }
    }
}