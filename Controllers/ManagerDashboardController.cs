using FuelIn.Data;
using Microsoft.AspNetCore.Mvc;

namespace FuelIn.Controllers
{
    public class ManagerDashboardController : Controller
    {
        [Authentication(requiredPrivilegeType = "MANAGER")]
        public IActionResult Index()
        {
            return View("../../Views/Dashboard/ManagerDashboard");
        }

        [Authentication(requiredPrivilegeType = "MANAGER")]
        public IActionResult LogoutManager()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("privilegeType");
            return View("../../Views/Auth/Login");
        }
    }
}
