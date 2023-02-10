using FuelIn.Data;
using FuelIn.Models.FuelData;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace FuelIn.Controllers.SuperAdmin
{
    public class ManageSupplyRequestsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IEmailSender _mail;
        public ManageSupplyRequestsController(AppDbContext context, IEmailSender mail)
        {
            _context = context;
            _mail = mail;
        }

        [Authentication(requiredPrivilegeType = "SUPER_ADMIN")]
        public IActionResult ViewSupplyRequests()
        {
            var supplyRequests = setViewBagsForViewPage();
            return View("../../Views/SuperAdmin/ManageSupplyRequests/ViewSupplyRequests", supplyRequests);
        }

        [Authentication(requiredPrivilegeType = "SUPER_ADMIN")]
        public IActionResult ShowAddEditSupplyRequest(int disID)
        {
            setViewBagsForEditPage(disID);
            return View("../../Views/SuperAdmin/ManageSupplyRequests/AddEditSupplyRequest");
        }

        [Authentication(requiredPrivilegeType = "SUPER_ADMIN")]
        [HttpPost]
        public async Task<IActionResult> EditSupplyRequest(string disID, string driverID, string arrivalHours, string disLocation)
        {
            var foundSupplyRequest = _context.fualDistributions
                        .Where(fd => fd.disID == int.Parse(disID))
                        .FirstOrDefault();
            if (driverID != null && arrivalHours != null && disLocation != null)
            {
                foundSupplyRequest.USER_ID = int.Parse(driverID);
                foundSupplyRequest.User = _context.USER.Where(u => u.USER_ID == int.Parse(driverID)).FirstOrDefault();
                foundSupplyRequest.arrivalHours = int.Parse(arrivalHours);
                foundSupplyRequest.disLocation = disLocation;

                //Email function - send emails if request was not previously accepted (accepted == 0)
                if (foundSupplyRequest.accepted == 0) 
                {
                    var stationCustomerEmails = _context.customers
                        .Where(c => c.staID == foundSupplyRequest.staID)
                        .Select(c => c.cusEmail)
                        .ToList();
                    if (stationCustomerEmails != null)
                    {
                        string subject = "Notice of Fuel Supply";
                        string message = "Dear customer,\nWe are pleased to inform you that the station you are registered to will be refuelling its supply soon. A fuel supply vehicle has been dispatched to the station, and will arive shortly.\nThank you,\nFuelIn";
                        foreach (string email in stationCustomerEmails)
                        {
                            await _mail.SendEmailAsync(email, subject, message);
                        }
                    }
                }

                foundSupplyRequest.accepted = 1;
                _context.SaveChanges();
                var supplyRequests = setViewBagsForViewPage();
                return View("../../Views/SuperAdmin/ManageSupplyRequests/ViewSupplyRequests", supplyRequests);
            }
            else
            {
                ModelState.AddModelError("", "Please fill in the required fields!");
                setViewBagsForEditPage(int.Parse(disID));
                return View("../../Views/SuperAdmin/ManageSupplyRequests/AddEditSupplyRequest");
            }
        }

        [Authentication(requiredPrivilegeType = "SUPER_ADMIN")]
        [HttpPost]
        public async Task<IActionResult> AddSupplyRequest(string staID, string driverID, string distributionStartDate, string expectedEndDate)
        {
            if (staID != null && driverID != null && distributionStartDate != null && expectedEndDate != null)
            {
                //Handle start/end dates validation
                if (DateTime.Parse(distributionStartDate) > DateTime.Parse(expectedEndDate) 
                    || DateTime.Parse(distributionStartDate) < DateTime.Now 
                    || DateTime.Parse(expectedEndDate) < DateTime.Now) 
                {
                    ModelState.AddModelError("", "Please enter valid start and end dates!");
                    setViewBagsForEditPage(0);
                    return View("../../Views/SuperAdmin/ManageSupplyRequests/AddEditSupplyRequest");
                }

                fualDistributions newSupplyRequest = new();
                newSupplyRequest.staID = int.Parse(staID);
                newSupplyRequest.station = _context.stations.Where(s => s.staID == int.Parse(staID)).FirstOrDefault();
                newSupplyRequest.USER_ID = int.Parse(driverID);
                newSupplyRequest.User = _context.USER.Where(u => u.USER_ID == int.Parse(driverID)).FirstOrDefault();
                newSupplyRequest.distributionStartDate = DateTime.Parse(distributionStartDate);
                newSupplyRequest.expectedEndDate = DateTime.Parse(expectedEndDate);
                newSupplyRequest.isDistributionStatus = 1;
                newSupplyRequest.arrivalHours = 99;
                newSupplyRequest.accepted = 1;
                newSupplyRequest.disLocation = "Supply center";
                _context.fualDistributions.Add(newSupplyRequest);

                //Email function - send email to all customers of station
                var stationCustomerEmails = _context.customers
                        .Where(c => c.staID == int.Parse(staID))
                        .Select(c => c.cusEmail)
                        .ToList();
                if (stationCustomerEmails != null)
                {
                    string subject = "Notice of Fuel Supply";
                    string message = "Dear customer,\nWe are pleased to inform you that the station you are registered to will be refuelling its supply soon. A fuel supply vehicle has been dispatched to the station, and will arive shortly.\nThank you,\nFuelIn";
                    foreach (string email in stationCustomerEmails) 
                    {
                        await _mail.SendEmailAsync(email, subject, message);
                    }
                }

                _context.SaveChanges();
                var supplyRequests = setViewBagsForViewPage();
                return View("../../Views/SuperAdmin/ManageSupplyRequests/ViewSupplyRequests", supplyRequests);
            }
            else
            {
                ModelState.AddModelError("", "Please fill in the required fields!");
                setViewBagsForEditPage(0);
                return View("../../Views/SuperAdmin/ManageSupplyRequests/AddEditSupplyRequest");
            }
        }

        private List<fualDistributions> setViewBagsForViewPage() 
        {
            var supplyRequests = _context.fualDistributions
                .ToList();
            ViewBag.users = _context.USER
                .ToList();
            ViewBag.stations = _context.stations
                .ToList();
            return supplyRequests;
        }

        private void setViewBagsForEditPage(int disID) 
        {
            ViewBag.Stations = _context.stations
                    .ToList();
            ViewBag.Drivers = _context.USER
                .Where(u => u.PRIVILEGE_TYPE == "DRIVER")
                .ToList();
            if (disID != 0)
            {
                var supplyRequest = _context.fualDistributions
                    .Where(fd => fd.disID == disID)
                    .FirstOrDefault();
                ViewBag.SupplyRequest = supplyRequest;
                if (supplyRequest != null) 
                {
                    ViewBag.requestStation = _context.stations.Where(s => s.staID == supplyRequest.staID).FirstOrDefault();
                }
            }
        }
    }
}
