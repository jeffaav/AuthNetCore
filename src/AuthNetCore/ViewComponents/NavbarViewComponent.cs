using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AuthNetCore.ViewComponents
{
    [ViewComponent(Name = "Navbar")]
    public class NavbarViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
