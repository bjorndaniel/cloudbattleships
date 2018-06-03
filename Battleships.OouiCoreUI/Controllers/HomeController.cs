using Battleships.XamFormsUI;
using Microsoft.AspNetCore.Mvc;
using Ooui.AspNetCore;
using Xamarin.Forms;

namespace Battleships.OouiCoreUi.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var page = new MainPage();
            return new ElementResult(page.GetOouiElement(), "Battleships!");
        }
    }
}