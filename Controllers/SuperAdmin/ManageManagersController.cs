using FuelIn.Data;
using FuelIn.Models.Auth;
using Microsoft.AspNetCore.Mvc;

namespace FuelIn.Controllers.SuperAdmin.ManageManagers
{
    public class ManageManagersController : Controller
    {
        private readonly AppDbContext _context;
        public ManageManagersController(AppDbContext context)
        {
            _context = context;
        }

        [Authentication(requiredPrivilegeType = "SUPER_ADMIN")]
        public IActionResult ViewManagers()
        {
            var managers = _context.USER
                .Where(u => u.PRIVILEGE_TYPE.Equals("MANAGER"))
                .ToList();
            return View("../../Views/SuperAdmin/ManageManagers/ViewManagers", managers);
        }

        [Authentication(requiredPrivilegeType = "SUPER_ADMIN")]
        public IActionResult ShowAddEditManager(int userId, string encryptedPassword) 
        {
            if (encryptedPassword != "-1")
            {
                EncryptDecryptText encryptDecryptText = new();
                ViewBag.User = _context.USER
                    .Where(u => u.USER_ID == userId)
                    .FirstOrDefault();
                ViewBag.decryptPassword = encryptDecryptText.DecryptText(encryptedPassword);
            }
            return View("../../Views/SuperAdmin/ManageManagers/AddEditManager");
        }

        [Authentication(requiredPrivilegeType = "SUPER_ADMIN")]
        [HttpPost]
        public IActionResult EditManager(string USER_ID, string USERNAME, string PASSWORD, string confirmPassword, string USER_STATUS) 
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
                    var managers = _context.USER
                        .Where(u => u.PRIVILEGE_TYPE.Equals("MANAGER"))
                        .ToList();
                    return View("../../Views/SuperAdmin/ManageManagers/ViewManagers", managers);
            }
            else 
            {
                ModelState.AddModelError("", "Password and Confirm Password must match!");
                ViewBag.User = foundUser;
                ViewBag.decryptPassword = encryptDecryptText.DecryptText(foundUser.PASSWORD);
                return View("../../Views/SuperAdmin/ManageManagers/AddEditManager");
            }
        }

        [Authentication(requiredPrivilegeType = "SUPER_ADMIN")]
        [HttpPost]
        public IActionResult AddManager(string USERNAME, string PASSWORD, string confirmPassword)
        {
            EncryptDecryptText encryptDecryptText = new();            
            if (USERNAME != null && PASSWORD != null && confirmPassword == PASSWORD)
            {
                User newManager = new();
                newManager.USERNAME = USERNAME;
                newManager.PASSWORD = encryptDecryptText.EncryptText(PASSWORD);
                newManager.PRIVILEGE_TYPE = "MANAGER";
                newManager.USER_STATUS = "ACT";
                _context.USER.Add(newManager);
                _context.SaveChanges();
                var managers = _context.USER
                    .Where(u => u.PRIVILEGE_TYPE.Equals("MANAGER"))
                    .ToList();
                return View("../../Views/SuperAdmin/ManageManagers/ViewManagers", managers);
            }
            else
            {
                ModelState.AddModelError("", "Password and Confirm Password must match!");
                return View("../../Views/SuperAdmin/ManageManagers/AddEditManager");
            }
        }
    }
}
