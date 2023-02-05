using FuelIn.Data;
using FuelIn.Models;
using Microsoft.AspNetCore.Mvc;

namespace FuelIn.Controllers
{
    public class CustomerRegDataModel
    {
        public List<StationModel> stations { get; set; }    
        public List<VehicleType> vehicleTypes { get; set; } 
    }
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;
        public List<StationModel> stationModels = new List<StationModel>();
        public List<VehicleType> vehicleTypes = new List<VehicleType>();
        public CustomerRegDataModel customerRegDataModel = new CustomerRegDataModel();

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
            return View(customerRegDataModel);
        }

        private void loadData()
        {
            StationModel model = new StationModel();
            model.StaId = 12;
            model.AvaFualQuota = 100;
            model.TotalFualQuota = 200;
            model.StaDistrict = "Colombo";
            stationModels.Add(model);
            StationModel model2 = new StationModel();
            model2.StaId = 12;
            model2.AvaFualQuota = 100;
            model2.TotalFualQuota = 200;
            model2.StaDistrict = "Matara";
            stationModels.Add(model2);
            customerRegDataModel.stations = stationModels;

            VehicleType vehicleType = new VehicleType();
            vehicleType.VehTypeID = 1;
            vehicleType.VehType = "Car";
            VehicleType vehicleType12 = new VehicleType();
            vehicleType12.VehTypeID = 2;
            vehicleType12.VehType = "Var";
            vehicleTypes.Add(vehicleType12);
            vehicleTypes.Add(vehicleType);
            customerRegDataModel.vehicleTypes = vehicleTypes;
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
