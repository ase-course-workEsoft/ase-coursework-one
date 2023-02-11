using FuelIn.Data;
using FuelIn.Models.Auth;
using FuelIn.Models.CustomerData;
using FuelIn.Models.StationData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace FuelIn.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;
        private IMemoryCache cache;
        public List<stations> stationModels = new List<stations>();
        public List<vehicleTypes> vehicleTypes = new List<vehicleTypes>();

        public AuthController(AppDbContext context, IMemoryCache memoryCache)
        {
            _context = context;
            cache = memoryCache;
        }

        public IActionResult Index()
        {

            return View("Login");
        }

        public IActionResult CustomerRegView()
        {
            loadData();
            ViewBag.vehicleTypes = vehicleTypes;
            ViewBag.stations = stationModels;
            return View("CustomerRegView");
        }

        [HttpPost]
        public IActionResult CustomerRegView(customers customerModel)
        {
   
            if (customerModel.User != null && customerModel.vehicleRegNum != null 
                && customerModel.cusName != null && customerModel.User.PASSWORD == customerModel.User.PASSWORD_CON)
            {
             loadData();
            int uId = _context.USER.Count() + 1;
            int cId = _context.customers.Count() + 1;
            List<stations> stationOne = stationModels.Where(s => s.staDistrict == customerModel.StaDistrict).ToList();
            customerModel.staID = stationOne[0].staID;
            customerModel.cusID = cId;
            customerModel.USER_ID = uId;

            List<vehicleTypes> vehicleOne = vehicleTypes.Where(s => s.vehType == customerModel.VehType).ToList();
            customerModel.vehTypeID = vehicleOne[0].vehTypeID;
            customerModel.avaWeeklyQuota = vehicleOne[0].weeklyQuota;

            EncryptDecryptText encryptDecryptText = new EncryptDecryptText();
            string encryptedPassword = encryptDecryptText.EncryptText(customerModel.User.PASSWORD);
            customerModel.User.USER_ID = uId;
            customerModel.User.PASSWORD = encryptedPassword;
            customerModel.User.PRIVILEGE_TYPE = "CONSUMER";
            customerModel.User.USER_STATUS = "ACT";
                try {
                    List<customers> cusList = _context.customers.Where(a => a.vehicleRegNum == customerModel.vehicleRegNum).ToList();
                    ViewBag.IsValid = cusList.Count;
                    if (cusList.Count == 0)
                    {
                        _context.USER.Add(customerModel.User);
                        _context.SaveChanges();
                        _context.customers.Add(customerModel);
                        _context.SaveChanges();
                        ModelState.Clear();
                        return View("../Auth/Login");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Vehicle registration number is already used.");
                        loadData();
                        ViewBag.vehicleTypes = vehicleTypes;
                        ViewBag.IsValid = "Vehicle registration number is already used.";
                        ViewBag.stations = stationModels;
                        return View("CustomerRegView");
                    }
                }
                catch (Exception ex)
                {
                    loadData();
                    ModelState.AddModelError("", ex.Message);
                    ViewBag.vehicleTypes = vehicleTypes;
                    ViewBag.IsValid = "Vehicle registration number is already used.";
                    ViewBag.stations = stationModels;
                    return View("CustomerRegView");
                }

            }
            else
            {
                loadData();
                ViewBag.vehicleTypes = vehicleTypes;
                ViewBag.stations = stationModels;
                return View("CustomerRegView");
            }

        }

        private void loadData()
        {
            stationModels = _context.stations.ToList();
            vehicleTypes = _context.vehicleTypes.ToList();
        }

        [HttpPost]
        public IActionResult Index(string userName, string password) 
        {
            EncryptDecryptText encryptDecryptText = new EncryptDecryptText();
            string encryptedPassword = "";
            if (password != null)
            {
                encryptedPassword = encryptDecryptText.EncryptText(password);
            }
            User foundAccount = _context.USER
                .Where(u => u.USERNAME == userName && u.PASSWORD == encryptedPassword && u.USER_STATUS == "ACT")
                .FirstOrDefault();
            if (foundAccount != null)
            {
                HttpContext.Session.SetString("userName", userName);
                if (foundAccount.PRIVILEGE_TYPE == "SUPER_ADMIN")
                {
                    HttpContext.Session.SetString("privilegeType", "SUPER_ADMIN");
                    return View("../Dashboard/SuperAdminDashboard");
                }
                else if (foundAccount.PRIVILEGE_TYPE == "MANAGER")
                {
                    HttpContext.Session.SetString("privilegeType", "MANAGER");
                    return View("../Dashboard/ManagerDashboard");
                }
                else if (foundAccount.PRIVILEGE_TYPE == "DRIVER")
                {
                    HttpContext.Session.SetString("privilegeType", "DRIVER");
                    return View("../Dashboard/DriverDashboard");
                }
                else
                {
                    HttpContext.Session.SetString("privilegeType", "CONSUMER");
                    customers customer = _context.customers.Where(c => c.USER_ID == foundAccount.USER_ID).FirstOrDefault();
                    stations station = _context.stations.Where(c => c.staID == customer.staID).FirstOrDefault();
                    vehicleTypes type = _context.vehicleTypes.Where(c => c.vehTypeID == customer.vehTypeID).FirstOrDefault();
                    CustomerRequest customerReq = _context.customerRequests.Where(c => c.cusID == customer.cusID).Where(c => c.ReqStatus == "Pending").FirstOrDefault();
                    if(customerReq == null)
                    {
                        ViewBag.customerReq = customerReq;
                    }else
                    {
                        ViewBag.customerReq = customerReq;
                    }
                    customer.station = station;
                    customer.User = foundAccount;
                    customer.vehicleTypes = type;
                    ViewBag.customer = customer;
                    HttpContext.Session.SetString("cId", customer.cusID.ToString());
                    return View("../Dashboard/ConsumerDashboard");

                }
            }
            else 
            {
                ModelState.AddModelError("", "Invalid username or password");
                return View("Login");
            }
        }


    }
}
