using Microsoft.AspNetCore.Mvc;
using CleanArchitecture.Identity.ViewModels;
using Microsoft.AspNetCore.Identity;
using CleanArchitecture.Identity.Models;
using IdentityServer4.Services;
using System.Threading.Tasks;

namespace CleanArchitecture.Identity.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IIdentityServerInteractionService _interactionService;
        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IIdentityServerInteractionService interactionService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _interactionService = interactionService;
        }
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            var viewModel = new LoginViewModel
            {
                ReturnUrl = returnUrl
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if(!ModelState.IsValid)
                return View(loginViewModel);

            var user = await _userManager.FindByNameAsync(loginViewModel.Username);
            
            if(user == null)
            {
                ModelState.AddModelError(string.Empty, "User isn't found");
                return View(loginViewModel);
            }
            var loginResult = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
            
            if(loginResult.Succeeded)
                return Redirect(loginViewModel.ReturnUrl);

            ModelState.AddModelError(string.Empty, "Login error");
            return View(loginViewModel);
        }

        [HttpGet]
        public IActionResult Register(string returnUrl)
        {
            var viewModel = new RegisterViewModel
            {
                ReturnUrl = returnUrl
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
                return View(registerViewModel);

            var user = new AppUser
            {
                UserName = registerViewModel.Username
            };
            var result = await _userManager.CreateAsync(user, registerViewModel.Password);
            if(result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return Redirect(registerViewModel.ReturnUrl);
            }    

            ModelState.AddModelError(string.Empty, "Register error");
            return View(registerViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            await _signInManager.SignOutAsync();
            var logoutResult = await _interactionService.GetLogoutContextAsync(logoutId);
            return Redirect(logoutResult.PostLogoutRedirectUri);
        }
    }
}
