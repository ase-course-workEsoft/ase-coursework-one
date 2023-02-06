using FuelIn.Data;
using FuelIn.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FuelIn.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;
        public List<StationModel> stationModels = new List<StationModel>();
        public List<VehicleType> vehicleTypes = new List<VehicleType>();

        public AuthController(AppDbContext context)
        {
            _context = context;
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
        public IActionResult CustomerRegView(Customer customerModel)
        {
   
            if (customerModel.User != null && customerModel.VehicleRegNum != null 
                && customerModel.CusName != null)
            {
             loadData();
            int uId = _context.USER.Count() + 1;
            int cId = _context.Customers.Count() + 1;
            List<StationModel> stationOne = stationModels.Where(s => s.StaDistrict == customerModel.StaDistrict).ToList();
            customerModel.StaId = stationOne[0].StaId;
            customerModel.CusId = cId;
            customerModel.USER_ID = uId;

            List<VehicleType> vehicleOne = vehicleTypes.Where(s => s.VehType == customerModel.VehType).ToList();
            customerModel.VehTypeId = vehicleOne[0].VehTypeID;
            customerModel.AvaWeeklyQuota = vehicleOne[0].WeeklyQuota;

            EncryptDecryptText encryptDecryptText = new EncryptDecryptText();
            string encryptedPassword = encryptDecryptText.EncryptText(customerModel.User.PASSWORD);
            customerModel.User.USER_ID = uId;
            customerModel.User.PASSWORD = encryptedPassword;
            customerModel.User.PRIVILEGE_TYPE = "CONSUMER";
            customerModel.User.USER_STATUS = "ACT";
                try {
                    List<Customer> cusList = _context.Customers.Where(a => a.VehicleRegNum == customerModel.VehicleRegNum).ToList();
                    if (cusList.Count == 0)
                    {
                        _context.USER.Add(customerModel.User);
                        _context.SaveChanges();
                        _context.Customers.Add(customerModel);
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
            stationModels = _context.Stations.ToList();
            vehicleTypes = _context.VehicleTypes.ToList();
        }

        [HttpPost]
        public IActionResult Index(string userName, string password) 
        {
            EncryptDecryptText encryptDecryptText = new EncryptDecryptText();
            string encryptedPassword = encryptDecryptText.EncryptText(password);
            var foundAccount = _context.USER
                .Where(u => u.USERNAME == userName && u.PASSWORD == encryptedPassword && u.USER_STATUS == "ACT")
                .FirstOrDefault();
            if (foundAccount != null)
            {
                if (foundAccount.PRIVILEGE_TYPE == "SUPER_ADMIN")
                {
                    return View("../Dashboard/SuperAdminDashboard");
                }
                else if (foundAccount.PRIVILEGE_TYPE == "MANAGER")
                {
                    return View("../Dashboard/ManagerDashboard");
                }
                else if (foundAccount.PRIVILEGE_TYPE == "DRIVER")
                {
                    return View("../Dashboard/DriverDashboard");
                }
                else
                {
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
