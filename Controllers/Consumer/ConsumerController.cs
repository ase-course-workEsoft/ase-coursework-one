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

namespace FuelIn.Controllers.Consumer
{
    public class ConsumerController : Controller
    {
        private readonly AppDbContext _context;
        public List<stations> stationModels = new List<stations>();
        public List<vehicleTypes> vehicleTypes = new List<vehicleTypes>();
        private IMemoryCache cache;
        public ConsumerController(AppDbContext context)
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
            loadData();
            int cID = int.Parse(HttpContext.Session.GetString("cId"));
            customers customerModel = _context.customers.Where(u => u.cusID == cID).FirstOrDefault();
            User user = _context.USER.Where(u => u.USER_ID == customerModel.USER_ID).FirstOrDefault();
            vehicleTypes type = vehicleTypes.Where(t => t.vehTypeID == customerModel.vehTypeID).FirstOrDefault();
            stations station = stationModels.Where(t => t.staID == customerModel.staID).FirstOrDefault();
            customerModel.User = user;
            customerModel.VehType = type.vehType;
            customerModel.StaDistrict = station.staDistrict;
            ViewBag.customer = customerModel;
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

                try
                {
                    int cID = int.Parse(HttpContext.Session.GetString("cId"));
                    customers model = _context.customers.Where(u => u.cusID == cID).FirstOrDefault();
                    User user = _context.USER.Where(u => u.USER_ID == model.USER_ID).FirstOrDefault();
                    model.User = user;
                    model.cusNIC = customerModel.cusNIC;
                    model.cusName = customerModel.cusName;
                    model.staID = customerModel.staID;
                    model.vehTypeID = customerModel.vehTypeID;
                    model.cusNIC = customerModel.cusNIC;
                    model.vehicleRegNum = customerModel.vehicleRegNum;
                    model.User.USERNAME = customerModel.User.USERNAME;
                    _context.SaveChanges();
                    ModelState.Clear();
                    ViewBag.customer = customerModel;
                    ViewBag.IsValid = "Success";
                    ViewBag.vehicleTypes = vehicleTypes;
                    ViewBag.stations = stationModels;
                    return View("../Home/ProfileEdit");
                }
                catch (Exception ex)
                {
                    ViewBag.customer = customerModel;
                    ViewBag.IsValid = customerModel.cusNIC + "  " + customerModel.StaDistrict;
                    ViewBag.vehicleTypes = vehicleTypes;
                    ViewBag.stations = stationModels;
                    return View("../Home/ProfileEdit");
                }

            }
            else
            {
                ViewBag.customer = customerModel;
                ViewBag.IsValid = "Notttt";
                ViewBag.vehicleTypes = vehicleTypes;
                ViewBag.stations = stationModels;
                string cusID = HttpContext.Items["cusID"] as string;
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