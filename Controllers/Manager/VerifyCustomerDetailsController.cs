using FuelIn.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FuelIn.Models.CustomerData;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Collections.Generic;

namespace FuelIn.Controllers.Manager
{
    public class VerifyCustomerDetailsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IEmailSender _mail;

        public VerifyCustomerDetailsController(AppDbContext context, IEmailSender mail)
        {
            _context = context;
            _mail = mail;
        }

        [Authentication(requiredPrivilegeType = "MANAGER")]
        public ActionResult VerifyCustomerDetails()
        {
            var approvedRequests = _context.customerRequests.Where(r => r.ReqStatus == "Approved" && r.Token==null)
                .Include(r => r.Customer).ToList();
            return View("../../Views/Manager/VerifyCustomerDetails/VerifyCustomerDetails", approvedRequests);
        }

        [Authentication(requiredPrivilegeType = "MANAGER")]
        [HttpPost]
        public async Task<IActionResult> VerifyToken(int Reqid)
        {
            var customerRequest = _context.customerRequests.FirstOrDefault(c => c.ReqId == Reqid);

            // Generate a token value and update the record
            string token = Guid.NewGuid().ToString();
            customerRequest.Token = token;
            _context.SaveChanges();

            var customers = await _context.customerRequests
                                .Include(c => c.Customer)
                                .FirstOrDefaultAsync(c => c.ReqId == Reqid);

            var email = customers.Customer.cusEmail;
            string subject = "Fuel Quota Token";
            string body = $"This is your token number: {token}. \nProvide this number to the filling station to get your requested fuel quota.\nThank you. \nFuelIn";
            if (customerRequest.Customer != null)
            {
                try 
                {
                    await _mail.SendEmailAsync(email, subject, body);
                }
                catch
                {
                    ModelState.AddModelError("", "All emails could not be sent due to an internal error!");
                }
                
            }

            return RedirectToAction("VerifyCustomerDetails");
        }

        public async Task<IActionResult> SendFuelReplenishedEmails()
        {
            var customers = await _context.customerRequests
                                .Include(c => c.Customer)
                                .Where(c => c.ReqStatus == "Approved" && c.Token != null)
                                .ToListAsync();

            foreach (var customer in customers)
            {
                var email = customer.Customer.cusEmail;
                var token = customer.Token;
                await _mail.SendEmailAsync(email, "Fuel Replenished at Station",
                    $"The fuel at the station has been replenished. Your token number is {token}. Please provide the token number at the filling station to get your requested fuel quota.");
            }

            return View("../../Views/Dashboard/ManagerDashboard");
            //return RedirectToAction("VerifyCustomerDetails");
        }

    }
}
