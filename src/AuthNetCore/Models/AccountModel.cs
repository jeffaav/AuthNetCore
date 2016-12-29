using AuthNetCore.Models.Identity;
using AuthNetCore.Models.Mappings;
using AuthNetCore.Models.ViewModels.Account;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;
using System.Threading.Tasks;

namespace AuthNetCore.Models
{
    public class AccountModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;

        public AccountModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;

            this._mapper = new MapperConfiguration(config =>
            {
                ApplicationUserMappings.RegisterMappings(config);
            }).CreateMapper();
        }

        public async Task Login(LoginVM viewModel, ModelStateDictionary modelState)
        {
            var user = await _userManager.FindByEmailAsync(viewModel.Email);
            if (user is null)
            {
                modelState.AddModelError("Email", "El correo no existe.");
                return;
            }

            var match = await _userManager.CheckPasswordAsync(user, viewModel.Password);
            if (!match)
            {
                modelState.AddModelError("Password", "Contraseña incorrecta");
                return;
            }

            var canSignIn = await _signInManager.CanSignInAsync(user);
            if (!canSignIn)
            {
                modelState.AddModelError("Email", "El usuario no puede ingresar al sistema");
                return;
            }

            var result = await _signInManager.PasswordSignInAsync(user, viewModel.Password, false, false);

            if (!result.Succeeded)
                modelState.AddModelError("Email", "Credenciales inválidas");
        }

        public Task Logout() => _signInManager.SignOutAsync();

        public async Task Register(RegisterVM viewModel, ModelStateDictionary modelState)
        {
            var user = _mapper.Map<ApplicationUser>(viewModel);

            var result = await _userManager.CreateAsync(user, viewModel.Password);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                    modelState.AddModelError("Errors", item.Description);
                return;
            }

            await _userManager.AddToRoleAsync(user, "User");                 
        }
    }
}
