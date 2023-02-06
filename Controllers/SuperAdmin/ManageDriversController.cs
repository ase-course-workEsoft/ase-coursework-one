using FuelIn.Data;
using FuelIn.Models.Auth;
using Microsoft.AspNetCore.Mvc;

namespace FuelIn.Controllers.SuperAdmin
{
    public class ManageDriversController : Controller
    {
        private readonly AppDbContext _context;
        public ManageDriversController(AppDbContext context)
        {
            _context = context;
        }

        [Authentication(requiredPrivilegeType = "SUPER_ADMIN")]
        public IActionResult ViewDrivers()
        {
            var drivers = _context.USER
                .Where(u => u.PRIVILEGE_TYPE.Equals("DRIVER"))
                .ToList();
            return View("../../Views/SuperAdmin/ManageDrivers/ViewDrivers", drivers);
        }

        [Authentication(requiredPrivilegeType = "SUPER_ADMIN")]
        public IActionResult ShowAddEditDriver(int userId, string encryptedPassword)
        {
            if (encryptedPassword != "-1")
            {
                EncryptDecryptText encryptDecryptText = new();
                ViewBag.User = _context.USER
                    .Where(u => u.USER_ID == userId)
                    .FirstOrDefault();
                ViewBag.decryptPassword = encryptDecryptText.DecryptText(encryptedPassword);
            }
            return View("../../Views/SuperAdmin/ManageDrivers/AddEditDriver");
        }

        [Authentication(requiredPrivilegeType = "SUPER_ADMIN")]
        [HttpPost]
        public IActionResult EditDriver(string USER_ID, string USERNAME, string PASSWORD, string confirmPassword, string USER_STATUS)
        {
            EncryptDecryptText encryptDecryptText = new();
            var foundUser = _context.USER
                        .Where(u => u.USER_ID == int.Parse(USER_ID))
                        .FirstOrDefault();
            if (USERNAME != null && PASSWORD != null && confirmPassword == PASSWORD)
            {

                foundUser.USERNAME = USERNAME;
                foundUser.PASSWORD = encryptDecryptText.EncryptText(PASSWORD);
                foundUser.USER_STATUS = USER_STATUS;
                _context.SaveChanges();
                var drivers = _context.USER
                    .Where(u => u.PRIVILEGE_TYPE.Equals("DRIVER"))
                    .ToList();
                return View("../../Views/SuperAdmin/ManageDrivers/ViewDrivers", drivers);
            }
            else
            {
                ModelState.AddModelError("", "Password and Confirm Password must match!");
                ViewBag.User = foundUser;
                ViewBag.decryptPassword = encryptDecryptText.DecryptText(foundUser.PASSWORD);
                return View("../../Views/SuperAdmin/ManageDrivers/AddEditDriver");
            }
        }

        [Authentication(requiredPrivilegeType = "SUPER_ADMIN")]
        [HttpPost]
        public IActionResult AddDriver(string USERNAME, string PASSWORD, string confirmPassword)
        {
            EncryptDecryptText encryptDecryptText = new();
            if (USERNAME != null && PASSWORD != null && confirmPassword == PASSWORD)
            {
                User newDriver = new();
                newDriver.USERNAME = USERNAME;
                newDriver.PASSWORD = encryptDecryptText.EncryptText(PASSWORD);
                newDriver.PRIVILEGE_TYPE = "DRIVER";
                newDriver.USER_STATUS = "ACT";
                _context.USER.Add(newDriver);
                _context.SaveChanges();
                var drivers = _context.USER
                    .Where(u => u.PRIVILEGE_TYPE.Equals("DRIVER"))
                    .ToList();
                return View("../../Views/SuperAdmin/ManageDrivers/ViewDrivers", drivers);
            }
            else
            {
                ModelState.AddModelError("", "Password and Confirm Password must match!");
                return View("../../Views/SuperAdmin/ManageDrivers/AddEditDriver");
            }
        }
    }
}
