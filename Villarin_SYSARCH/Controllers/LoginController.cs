using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Transactions;
using Villarin_SYSARCH.Data;
using Villarin_SYSARCH.Models;
using Villarin_SYSARCH.ViewModels;

namespace Villarin_SYSARCH.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _context;

        public LoginController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel account)
        {
            int adminId = 123;

            // ModelState.Remove or ViewModel
            /*
            ModelState.Remove("FirstName");
            ModelState.Remove("LastName");
            ModelState.Remove("MiddleName");
            ModelState.Remove("CourseLevel");
            ModelState.Remove("Course");
            ModelState.Remove("Email");
            ModelState.Remove("Address");
            ModelState.Remove("ConfirmPassword");
            */
            bool isEmpty = !_context.Accounts.Any();

            if (isEmpty == true) //checks if there are no accounts registered
            {
                ViewBag.IsAccountsEmpty = true;
            } else
            {
                var accounts = _context.Accounts.ToList();
                if (ModelState.IsValid)
                {
                    foreach (var s in accounts)
                    {
                        if (s.Id == account.Id && s.Password == account.Password)
                        {
                            ViewBag.IsLoginSuccessful = true;


                            //HttpContext.Session.SetString("AccountModel", System.Text.Json.JsonSerializer.Serialize(s));
                            //TempData["AccountModel"] = JsonConvert.SerializeObject(s);
                            if (account.Id == adminId) //if user is admin or student
                            {
                                return RedirectToAction("Dashboard", "Admin");
                            }
                            else
                            {
                                /*
                                var claims = new List<Claim>
                                {
                                    new Claim(ClaimTypes.Name, $"{s.FirstName} {s.MiddleName} {s.LastName}"),
                                    new Claim("StudentId", s.Id.ToString()),
                                    new Claim(ClaimTypes.Role, "Student")
                                }; 

                                var identity = new ClaimsIdentity
                                (
                                    claims,
                                    CookieAuthenticationDefaults.AuthenticationScheme, // This must match!
                                    ClaimTypes.Name,  // <--- Tells @User.Identity.Name where to look
                                    ClaimTypes.Role   // <--- Tells @User.IsInRole where to look
                                );

                                var principal = new ClaimsPrincipal(identity);
                                */
                                //HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                                HttpContext.Session.SetString("AccountModel", JsonConvert.SerializeObject(s)); //CHECK
                                return RedirectToAction("Home", "Student");
                            }
                        } else
                        {
                            ViewBag.IsLoginSuccessful = false;
                        }
                    }
                    /*if (account.Id == "123" && account.Password == "admin")
                    {
                        return RedirectToAction("Dashboard", "Student");
                    }*/
                }
            }
            return View(account);
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Account account)
        {

            if (ModelState.IsValid)
            {
                var accounts = _context.Accounts.ToList();
                foreach (var s in accounts)
                {
                    if (s.Id == account.Id)
                    {
                        return View(account);
                    }
                }
                _context.Accounts.Add(account);
                _context.SaveChanges();
                return RedirectToAction("Login");
            }
            return View(account);
        }
    }
}
