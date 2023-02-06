using FuelIn.Data;
using Microsoft.AspNetCore.Mvc;

namespace FuelIn.Controllers
{
    public class SuperAdminDashboardController : Controller
    {
        [Authentication(requiredPrivilegeType = "SUPER_ADMIN")]
        public IActionResult Index()
        {
            return View("SuperAdminDashboard");
        }

        [Authentication(requiredPrivilegeType = "SUPER_ADMIN")]
        public RedirectToActionResult ShowManagers() 
        {
            return RedirectToAction("ViewManagers", "ManageManagers");
        }

        [Authentication(requiredPrivilegeType = "SUPER_ADMIN")]
        public RedirectToActionResult ShowDrivers()
        {
            return RedirectToAction("ViewDrivers", "ManageDrivers");
        }

        [Authentication(requiredPrivilegeType = "SUPER_ADMIN")]
        public RedirectToActionResult ShowCustomers()
        {
            return RedirectToAction("ViewCustomers", "ViewCustomers");
        }

        [Authentication(requiredPrivilegeType = "SUPER_ADMIN")]
        public RedirectToActionResult ShowSupplyRequests()
        {
            return RedirectToAction("ViewSupplyRequests", "ManageSupplyRequests");
        }
    }
}
