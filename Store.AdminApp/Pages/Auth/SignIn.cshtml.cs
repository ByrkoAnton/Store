using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AdminApp.Entities;
using AdminApp.Models;
using AdminApp.Providers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Store.BusinessLogicLayer;

namespace AdminApp.SignIn
{
    public class SignInModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ITokenProvider _tokenProvider;

        public SignInModel(SignInManager<User> signInManager, UserManager<User> userManager, ITokenProvider jwtProvider)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenProvider = jwtProvider;
        }
        [BindProperty]
        public modelSignIn AuthModel { get; set; }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(AuthModel.Email);
                if (user is null)
                {
                    throw new CustomException("No user",HttpStatusCode.Unauthorized);
                }

                var signIn = await _signInManager.PasswordSignInAsync(AuthModel.Email, AuthModel.Password, false, false);

                if (!signIn.Succeeded)
                {
                    throw new CustomException("Wrong password", HttpStatusCode.Unauthorized);
                }
                var roleList = await _userManager.GetRolesAsync(user);

                string token = _tokenProvider.GenerateJwt(AuthModel.Email, roleList.ToList(), user.Id.ToString());

                var handler = new JwtSecurityTokenHandler().ReadJwtToken(token);

                List<Claim> claims = handler.Claims.ToList();
                ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

                //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
                //await _signInManager.SignInAsync(user, true);
            }
            return Page();
        }
    }
}
