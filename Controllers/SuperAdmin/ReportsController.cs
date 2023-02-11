using FuelIn.Data;
using FuelIn.Models.CustomerData;
using FuelIn.Models.FuelData;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace FuelIn.Controllers.SuperAdmin
{
    public class ReportsController : Controller
    {
        private readonly AppDbContext _context;
        public ReportsController(AppDbContext context, IEmailSender mail)
        {
            _context = context;
        }

        [Authentication(requiredPrivilegeType = "SUPER_ADMIN")]
        public IActionResult ViewCustomerReport()
        {
            setStationViewBagData();
            return View("../../Views/SuperAdmin/Reports/CustomerReport");
        }

        [Authentication(requiredPrivilegeType = "SUPER_ADMIN")]
        public IActionResult ViewFualDistributionReport()
        {
            setStationViewBagData();
            return View("../../Views/SuperAdmin/Reports/FualDistributionReport");
        }

        [Authentication(requiredPrivilegeType = "SUPER_ADMIN")]
        [HttpPost]
        public FileResult GenerateCustomerReport(string staID)
        {
            //Get data required for the report. staID == -1 means the user wants the report for all stations
            List<customers> customerData;
            if (int.Parse(staID) != -1)
            {
                customerData = _context.customers
                    .Where(c => c.staID == int.Parse(staID))
                    .ToList();
            }
            else
            {
                customerData = _context.customers.ToList();
            }

            using (var stream = new MemoryStream())
            {
                using (var writer = new PdfWriter(stream))
                {
                    using (var pdf = new PdfDocument(writer))
                    {
                        var document = new Document(pdf);
                        var headerCellFont = PdfFontFactory.CreateFont(StandardFonts.TIMES_BOLD);

                        //If there is no customer data, the titles and table will not be printed
                        if (customerData.Count != 0)
                        {
                            //Create report title and subtitles
                            document.Add(new Paragraph("Customer Report").SetFontSize(24));
                            document.Add(new Paragraph("Date: " + DateTime.Now.ToString("dd/MM/yyyy")));
                            document.Add(new Paragraph("FuelIn E-solutions").SetFontSize(10));

                            //Create report table
                            var table = new Table(new float[] { 2, 2, 1, 1, 1, 1, 1, 1 });
                            table.AddHeaderCell(new Cell().Add(new Paragraph("Name").SetFont(headerCellFont).SetFontSize(14)));
                            table.AddHeaderCell(new Cell().Add(new Paragraph("NIC").SetFont(headerCellFont).SetFontSize(14)));
                            table.AddHeaderCell(new Cell().Add(new Paragraph("Station").SetFont(headerCellFont).SetFontSize(14)));
                            table.AddHeaderCell(new Cell().Add(new Paragraph("Vehicle Type").SetFont(headerCellFont).SetFontSize(14)));
                            table.AddHeaderCell(new Cell().Add(new Paragraph("Vehicle Registration No.").SetFont(headerCellFont).SetFontSize(14)));
                            table.AddHeaderCell(new Cell().Add(new Paragraph("Available Weekly Quota").SetFont(headerCellFont).SetFontSize(14)));
                            table.AddHeaderCell(new Cell().Add(new Paragraph("Username").SetFont(headerCellFont).SetFontSize(14)));
                            table.AddHeaderCell(new Cell().Add(new Paragraph("Email").SetFont(headerCellFont).SetFontSize(14)));
                            foreach (var customer in customerData)
                            {
                                table.AddCell(new Cell().Add(new Paragraph(customer.cusName)));
                                table.AddCell(new Cell().Add(new Paragraph(customer.cusNIC)));
                                table.AddCell(new Cell().Add(new Paragraph(getStationName(customer.staID))));
                                table.AddCell(new Cell().Add(new Paragraph(getVehicleType(customer.vehTypeID))));
                                table.AddCell(new Cell().Add(new Paragraph(customer.vehicleRegNum)));
                                table.AddCell(new Cell().Add(new Paragraph(customer.avaWeeklyQuota.ToString())));
                                table.AddCell(new Cell().Add(new Paragraph(getUsername(customer.USER_ID))));
                                table.AddCell(new Cell().Add(new Paragraph(handleCusEmail(customer))));
                            }
                            document.Add(table);
                        }
                        else
                        {
                            document.Add(new Paragraph("No customer data for the " + getStationName(int.Parse(staID)) + " station!").SetFontSize(24));
                        }

                        document.Close();
                    }
                }
                return File(stream.ToArray(), "application/pdf", "Customer Report.pdf");
            }

        }

        [Authentication(requiredPrivilegeType = "SUPER_ADMIN")]
        [HttpPost]
        public FileResult GenerateFualDistributionReport(string staID, string isAccepted)
        {
            var acceptedornot = isAccepted;
            //Get data required for the report. A parameter being -1 means the user wants the report for all of those parameter values
            List<fualDistributions> fualDistributionsData;
            if (int.Parse(staID) != -1)
            {
                if (int.Parse(isAccepted) != -1)
                {
                    fualDistributionsData = _context.fualDistributions
                                        .Where(fd => fd.staID == int.Parse(staID) && fd.accepted == int.Parse(isAccepted))
                                        .ToList();
                }
                else
                {
                    fualDistributionsData = _context.fualDistributions
                        .Where(c => c.staID == int.Parse(staID))
                        .ToList();
                }
            }
            else
            {
                if (int.Parse(isAccepted) != -1)
                {
                    fualDistributionsData = _context.fualDistributions
                                        .Where(fd => fd.accepted == int.Parse(isAccepted))
                                        .ToList();
                }
                else
                {
                    fualDistributionsData = _context.fualDistributions.ToList();
                }

            }

            using (var stream = new MemoryStream())
            {
                using (var writer = new PdfWriter(stream))
                {
                    using (var pdf = new PdfDocument(writer))
                    {
                        var document = new Document(pdf);
                        var headerCellFont = PdfFontFactory.CreateFont(StandardFonts.TIMES_BOLD);

                        //If there is no customer data, the titles and table will not be printed
                        if (fualDistributionsData.Count != 0)
                        {
                            //Create report title and subtitles
                            document.Add(new Paragraph("Fuel Supply Request Report").SetFontSize(24));
                            document.Add(new Paragraph("Date: " + DateTime.Now.ToString("dd/MM/yyyy")));
                            document.Add(new Paragraph("FuelIn E-solutions").SetFontSize(10));

                            //Create report table
                            var table = new Table(new float[] { 2, 1, 1, 1, 1, 1, 1, 2 });
                            table.AddHeaderCell(new Cell().Add(new Paragraph("Station").SetFont(headerCellFont).SetFontSize(14)));
                            table.AddHeaderCell(new Cell().Add(new Paragraph("Assigned Driver").SetFont(headerCellFont).SetFontSize(14)));
                            table.AddHeaderCell(new Cell().Add(new Paragraph("Distribution Start Date").SetFont(headerCellFont).SetFontSize(14)));
                            table.AddHeaderCell(new Cell().Add(new Paragraph("Expected End Date").SetFont(headerCellFont).SetFontSize(14)));
                            table.AddHeaderCell(new Cell().Add(new Paragraph("Being Distributed?").SetFont(headerCellFont).SetFontSize(14)));
                            table.AddHeaderCell(new Cell().Add(new Paragraph("Arrival ETA").SetFont(headerCellFont).SetFontSize(14)));
                            table.AddHeaderCell(new Cell().Add(new Paragraph("Accepted?").SetFont(headerCellFont).SetFontSize(14)));
                            table.AddHeaderCell(new Cell().Add(new Paragraph("Current Location").SetFont(headerCellFont).SetFontSize(14)));
                            foreach (var fualDistribution in fualDistributionsData)
                            {
                                table.AddCell(new Cell().Add(new Paragraph(getStationName(fualDistribution.staID))));
                                table.AddCell(new Cell().Add(new Paragraph(getUsername(fualDistribution.USER_ID))));
                                table.AddCell(new Cell().Add(new Paragraph(fualDistribution.distributionStartDate.ToString()))); ;
                                table.AddCell(new Cell().Add(new Paragraph(fualDistribution.expectedEndDate.ToString())));
                                table.AddCell(new Cell().Add(new Paragraph(getYesNoString(fualDistribution.isDistributionStatus))));
                                table.AddCell(new Cell().Add(new Paragraph(fualDistribution.arrivalHours.ToString())));
                                table.AddCell(new Cell().Add(new Paragraph(getYesNoString(fualDistribution.accepted))));
                                table.AddCell(new Cell().Add(new Paragraph(fualDistribution.disLocation)));
                            }
                            document.Add(table);
                        }
                        else
                        {
                            document.Add(new Paragraph("No Supply Request data for the given filters!").SetFontSize(24));
                        }

                        document.Close();
                    }
                }
                return File(stream.ToArray(), "application/pdf", "Supply Request Report.pdf");
            }

        }

        //Sets view bag data needed for the select boxes
        private void setStationViewBagData()
        {
            ViewBag.Stations = _context.stations
                    .ToList();
        }

        //Returns YES or NO based on the given integer
        private string getYesNoString(int yesNoInt)
        {
            if (yesNoInt == 1)
            {
                return "YES";
            }
            else
            {
                return "NO";
            }
        }

        //Gets the station name using the given ID
        private string getStationName(int ID)
        {
            var station = _context.stations
                .Where(s => s.staID == ID)
                .FirstOrDefault();
            if (station != null)
            {
                return station.staDistrict;
            }
            else
            {
                return "";
            }
        }

        //Gets the username using the given ID
        private string getUsername(int ID)
        {
            var user = _context.USER
                .Where(u => u.USER_ID == ID)
                .FirstOrDefault();
            if (user != null)
            {
                return user.USERNAME;
            }
            else
            {
                return "";
            }
        }

        //Gets the vehicle type using the given ID
        private string getVehicleType(int ID)
        {
            var vehicleType = _context.vehicleTypes
                .Where(vt => vt.vehTypeID == ID)
                .FirstOrDefault();
            if (vehicleType != null)
            {
                return vehicleType.vehType;
            }
            else
            {
                return "";
            }
        }

        //Returns an empty string if the customer's email is null
        private string handleCusEmail(customers customer)
        {
            if (customer.cusEmail == null)
            {
                return "";
            }
            else
            {
                return customer.cusEmail;
            }
        }
    }
}
