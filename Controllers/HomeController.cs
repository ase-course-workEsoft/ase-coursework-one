using FuelIn.Data;
using FuelIn.Models;
using FuelIn.Models.Auth;
using FuelIn.Models.CustomerData;
using FuelIn.Models.StationData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Diagnostics;

namespace FuelIn.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public List<stations> stationModels = new List<stations>();
        public List<vehicleTypes> vehicleTypes = new List<vehicleTypes>();
        private IMemoryCache cache;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        private void loadData()
        {
            stationModels = _context.stations.ToList();
            vehicleTypes = _context.vehicleTypes.ToList();
        }

        public IActionResult ProfileEdit()
        {
            int cID = int.Parse(HttpContext.Session.GetString("cId"));
            customers customerModel = _context.customers.Where(u => u.cusID == cID).FirstOrDefault();
            User user = _context.USER.Where(u => u.USER_ID == customerModel.USER_ID).FirstOrDefault();
            customerModel.User = user;
            ViewBag.customer = customerModel;
            loadData();
            ViewBag.vehicleTypes = vehicleTypes;
            ViewBag.stations = stationModels;
            string cusID = HttpContext.Items["cusID"] as string;
            return View("../Home/ProfileEdit");
        }

        [HttpPost]
        public IActionResult ProfileEdit(customers customerModel)
        {
            if (customerModel.User != null && customerModel.vehicleRegNum != null
                && customerModel.cusName != null && customerModel.User.PASSWORD == customerModel.User.PASSWORD_CON)
            {
                loadData();
                List<stations> stationOne = stationModels.Where(s => s.staDistrict == customerModel.StaDistrict).ToList();
                customerModel.staID = stationOne[0].staID;

                List<vehicleTypes> vehicleOne = vehicleTypes.Where(s => s.vehType == customerModel.VehType).ToList();
                customerModel.vehTypeID = vehicleOne[0].vehTypeID;
                customerModel.avaWeeklyQuota = vehicleOne[0].weeklyQuota;

                EncryptDecryptText encryptDecryptText = new EncryptDecryptText();
                string encryptedPassword = encryptDecryptText.EncryptText(customerModel.User.PASSWORD);
                customerModel.User.PASSWORD = encryptedPassword;
                customerModel.User.PRIVILEGE_TYPE = "CONSUMER";
                customerModel.User.USER_STATUS = "ACT";
                try
                {
                    List<customers> cusList = _context.customers.Where(a => a.vehicleRegNum == customerModel.vehicleRegNum).ToList();
                    ViewBag.IsValid = cusList.Count;
                    if (cusList.Count == 0)
                    {
                        _context.customers.Add(customerModel);
                        //_context.SaveChanges();
                        ModelState.Clear();
                        loadData();
                        ViewBag.vehicleTypes = vehicleTypes;
                        ViewBag.stations = stationModels;
                        ViewBag.customer = customerModel;
                        return View("../Home/ProfileEdit");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Vehicle registration number is already used.");
                        loadData();
                        ViewBag.vehicleTypes = vehicleTypes;
                        ViewBag.IsValid = "Vehicle registration number is already used.";
                        ViewBag.stations = stationModels;
                        ViewBag.customer = customerModel;
                        return View("../Home/ProfileEdit");
                    }
                }
                catch (Exception ex)
                {
                    loadData();
                    ModelState.AddModelError("", ex.Message);
                    ViewBag.vehicleTypes = vehicleTypes;
                    ViewBag.IsValid = "Vehicle registration number is already used.";
                    ViewBag.stations = stationModels;
                    ViewBag.customer = customerModel;
                    return View("../Home/ProfileEdit");
                }

            }
            else
            {
                loadData();
                ViewBag.customer = customerModel;
                ViewBag.vehicleTypes = vehicleTypes;
                ViewBag.stations = stationModels;
                return View("../Home/ProfileEdit");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}