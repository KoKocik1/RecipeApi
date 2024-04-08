using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RecipeApi.IService;
using RecipeApi.Models;

namespace RecipeApi.Controllers
{
    [Route("api/account")]
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
        public ActionResult RegisterAccount([FromBody] RegisterUserDto dto)
        {
            _acconntService.RegisterUser(dto);
            return Ok();
        }
        [HttpPost]
        public ActionResult Login([FromBody] LoginDto dto)
        {
            string token = _acconntService.GenerateJwt(dto);
            return Ok(token);
        }
    }
}