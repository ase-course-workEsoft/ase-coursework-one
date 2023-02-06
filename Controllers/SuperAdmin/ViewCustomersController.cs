using FuelIn.Data;
using FuelIn.Models.Auth;
using Microsoft.AspNetCore.Mvc;

namespace FuelIn.Controllers.SuperAdmin
{
    public class ViewCustomersController : Controller
    {
        private readonly AppDbContext _context;
        public ViewCustomersController(AppDbContext context)
        {
            _context = context;
        }

        [Authentication(requiredPrivilegeType = "SUPER_ADMIN")]
        public IActionResult ViewCustomers()
        {
            var customers = _context.customers
                            .ToList();
            ViewBag.users = _context.USER
                            .ToList();
            ViewBag.vehicleTypes = _context.vehicleTypes
                            .ToList();
            ViewBag.stations = _context.stations
                            .ToList();
            return View("../../Views/SuperAdmin/ViewCustomers/ViewCustomers", customers);
        }
    }
}
