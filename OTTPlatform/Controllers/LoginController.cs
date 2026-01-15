using Microsoft.AspNetCore.Mvc;
using OTTPlatform.DAL;
using OTTPlatform.Models;
using System.Diagnostics;


namespace OTTPlatform.Controllers
{
    public class LoginController : Controller
    {

        private readonly Login_DAL _DAL;
        private string connectionString = "YourConnectionStringHere";

        public LoginController(Login_DAL dal)
        {
            _DAL = dal;
        }
        [HttpGet]
        public IActionResult LoginPage()
        {
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";

     

            return View();
        }
        [HttpGet]
        public IActionResult SignUpPage()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoginValidation (string UserName, string Password)
        {
            var login = _DAL.ValidateLogin(UserName, Password);

            if (login != null)
            {
                return Json(new
                {
                    success = true,
                    data = new
                    {
                        id = login.Id,
                        UserType = login.UserType,
                        UserName = login.UserName,
                        password = login.password
                    }
                });
            }

            return Json(new { success = false, message = "Login failed" });
        }

        [HttpPost]
        public IActionResult SignupValidation(string UserName)
        {
            var existingUser = _DAL.ValidateSignup(UserName);

            if (existingUser != null)
            {
                return Json(new { success = false, message = "Username already taken" });
            }

            return Json(new { success = true, message = "Username available" });
        }


        [HttpPost]
        public IActionResult SignupInsert(Login login)
        {

            var existingUser = _DAL.ValidateSignup(login.UserName);

            if (existingUser != null)
            {
                return Json(new { success = false, message = "Username already taken" });
            }

            var Login = _DAL.InsertSignUp(login);

            if (Login != null)
            {
                return Json(new
                {
                    success = true,
                    data = new
                    {
                        id = login.Id,
                        UserType = login.UserType,
                        UserName = login.UserName,
                        password = login.password
                    }
                });
            }

            return Json(new { success = false, message = "Login failed" });
        }

    }
}
