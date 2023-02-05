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
            return View("../../Views/SuperAdmin/ViewCustomers/ViewCustomers", customers);
        }
    }
}
