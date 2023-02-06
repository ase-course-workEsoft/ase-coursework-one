using FuelIn.Data;
using Microsoft.AspNetCore.Mvc;

namespace FuelIn.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;
        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {

            return View("Login");
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
            var foundAccount = _context.USER
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
