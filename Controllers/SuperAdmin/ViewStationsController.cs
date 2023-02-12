using FuelIn.Data;
using Microsoft.AspNetCore.Mvc;

namespace FuelIn.Controllers.SuperAdmin
{
    public class ViewStationsController : Controller
    {
        private readonly AppDbContext _context;
        public ViewStationsController(AppDbContext context)
        {
            _context = context;
        }

        [Authentication(requiredPrivilegeType = "SUPER_ADMIN")]
        public IActionResult ViewStations()
        {
            var stations = _context.stations
                            .ToList();
            return View("../../Views/SuperAdmin/ViewStations/ViewStations", stations);
        }
    }
}
