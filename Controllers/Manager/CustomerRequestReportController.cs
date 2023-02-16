using Microsoft.AspNetCore.Mvc;
using FuelIn.Data;
using FuelIn.Models.CustomerData;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.AspNetCore.Identity.UI.Services;
using iText.Layout.Properties;

namespace FuelIn.Controllers.Manager
{
    public class CustomerRequestReportController : Controller
    {
        private readonly AppDbContext _context;

        public CustomerRequestReportController(AppDbContext context)
        {
            _context = context;
        }

        public ActionResult GenerateReport()
        {
            var customerData = _context.customerRequests
                .Select(req => new
                {
                    cusName = req.Customer.cusName,
                    cusNIC = req.Customer.cusNIC,
                    token = req.Token,
                    requestsStatus = req.ReqStatus,
                    expectedFillingTime = req.ExpectedFillingTime,
                    requestedQuota = req.RequestedQuota
                })
                .ToList();

            using (var stream = new MemoryStream())
            {
                using (var writer = new PdfWriter(stream))
                {
                    using (var pdf = new PdfDocument(writer))
                    {
                        var document = new Document(pdf);

                        if (customerData.Count != 0)
                        {
                            //Create report title and subtitles
                            document.Add(new Paragraph("Customer Requests Report").SetTextAlignment(TextAlignment.CENTER).SetFontSize(18));
                            document.Add(new Paragraph("Date: " + DateTime.Now.ToString("dd/MM/yyyy")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(10));

                            //Create report table
                            var table = new Table(new float[] { 1, 1, 1, 1, 1, 1 });
                            table.AddHeaderCell(new Cell().Add(new Paragraph("Customer Name")));
                            table.AddHeaderCell(new Cell().Add(new Paragraph("Customer NIC")));
                            table.AddHeaderCell(new Cell().Add(new Paragraph("Token")));
                            table.AddHeaderCell(new Cell().Add(new Paragraph("Status")));
                            table.AddHeaderCell(new Cell().Add(new Paragraph("Expected Filling Time")));
                            table.AddHeaderCell(new Cell().Add(new Paragraph("Requested Quota")));

                            foreach (var customer in customerData)
                            {
                                table.AddCell(new Cell().Add(new Paragraph(customer.cusName)));
                                table.AddCell(new Cell().Add(new Paragraph(customer.cusNIC)));
                                table.AddCell(new Cell().Add(new Paragraph(customer.token)));
                                table.AddCell(new Cell().Add(new Paragraph(customer.requestsStatus)));
                                table.AddCell(new Cell().Add(new Paragraph(customer.expectedFillingTime.ToString())));
                                table.AddCell(new Cell().Add(new Paragraph(customer.requestedQuota.ToString())));
                            }
                            document.Add(table);
                        }
                        else
                        {
                            document.Add(new Paragraph("No customer requests data!").SetTextAlignment(TextAlignment.CENTER).SetFontSize(18));
                        }

                        document.Close();
                    }
                }
                return File(stream.ToArray(), "application/pdf", "Customer Requests Report.pdf");
            }
        }

    }
}
