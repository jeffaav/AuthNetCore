using AuthNetCore.Filters;
using AuthNetCore.Models;
using AuthNetCore.Models.ViewModels.Account;
using AuthNetCore.Utils.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AuthNetCore.Controllers
{
    public class AccountController : Controller
    {
        private readonly AccountModel _model;

        public AccountController(AccountModel model)
        {
            this._model = model;
        }

        [ImportModelState]
        [UserAuthenticated]
        public IActionResult Login(string returnUrl) => View(TempData.ContainsKey("ViewModel") ? TempData.GetViewModel<LoginVM>() : new LoginVM { ReturnUrl = returnUrl });

        [HttpPost]
        [ExportModelState]
        public async Task<IActionResult> Login(LoginVM viewModel)
        {
            if (ModelState.IsValid)
            {
                await _model.Login(viewModel, ModelState);

                if (ModelState.ErrorCount is 0)
                    return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            TempData.PutViewModel(viewModel);
            return RedirectToAction(nameof(AccountController.Login), new { returnUrl = viewModel.ReturnUrl });
        }

        [ImportModelState]
        [UserAuthenticated]
        public IActionResult Register() => View(TempData.ContainsKey("ViewModel") ? TempData.GetViewModel<RegisterVM>() : new RegisterVM());

        [HttpPost]
        [ExportModelState]
        public async Task<IActionResult> Register(RegisterVM viewModel)
        {
            if (ModelState.IsValid)
            {
                await _model.Register(viewModel, ModelState);

                if (ModelState.ErrorCount is 0)
                    return RedirectToAction(nameof(AccountController.Login));
            }

            TempData.PutViewModel(viewModel);
            return RedirectToAction(nameof(AccountController.Register));
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _model.Logout();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
