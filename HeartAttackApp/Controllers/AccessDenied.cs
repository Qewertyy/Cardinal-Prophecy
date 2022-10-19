using Microsoft.AspNetCore.Mvc;

namespace HeartAttackApp.Controllers
{
    public class AccessDenied : Controller
    {
        public IActionResult UnAuthorized()
        {
            return View();
        }
    }
}
