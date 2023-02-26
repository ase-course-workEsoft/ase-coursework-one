using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FuelIn.Data;
using FuelIn.Models.CustomerData;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace FuelIn.Controllers.Manager
{
    public class ManageRequestsController : Controller
    {
        private readonly AppDbContext _context;

        public ManageRequestsController(AppDbContext context)
        {
            _context = context;
        }


        [Authentication(requiredPrivilegeType = "MANAGER")]
        // GET: CustomerRequests
        public IActionResult ManageRequests()
        {
            var pendingRequests = _context.customerRequests.Where(r => r.ReqStatus == "Pending")
                .Include(r => r.Customer).ToList();
            return View("../../Views/Manager/ManageRequests/ManageRequests", pendingRequests);
        }

        [Authentication(requiredPrivilegeType = "MANAGER")]
        [HttpPost]
        public async Task<IActionResult> ApproveRequest(int Reqid)
        {
            using (_context)
            {
                var request = await _context.customerRequests.FindAsync(Reqid);
                request.ReqStatus = "Approved";
                request.Token = "1";
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("ManageRequests");
        }

        [Authentication(requiredPrivilegeType = "MANAGER")]
        [HttpPost]
        public async Task<IActionResult> RejectRequest(int Reqid)
        {
            using (_context)
            {
                var request = await _context.customerRequests.FindAsync(Reqid);
                request.ReqStatus = "Rejected";
                request.Token = "0";
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("ManageRequests");
        }
    }
}
