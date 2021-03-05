using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.BusinessLogicLayer.Servises.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : Controller
    {
        private readonly IAccountService _accountService;

        public TestController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("test")]
        public async Task<IActionResult> TestAction()
        {
            return Ok("work");
        }

        [HttpGet("testauth")]
        [Authorize]
        public async Task<IActionResult> TestAuth()
        {
            return Ok("auth");
        }

        [HttpGet("signin")]
        public async Task<IActionResult> SignIn()
        {
            var result = await _accountService.SignInAsync(new BusinessLogicLayer.Models.Users.UserModel());
            return Ok(result);
        }
    }
}
