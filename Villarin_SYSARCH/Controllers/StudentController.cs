using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Newtonsoft.Json;
using Villarin_SYSARCH.Data;
using Villarin_SYSARCH.Models;
using Villarin_SYSARCH.ViewModels;

namespace Villarin_SYSARCH.Controllers
{
    public class StudentController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly AppDbContext _context;
        public StudentController(IWebHostEnvironment webHostEnvironment, AppDbContext context)
        {
            _webHostEnvironment = webHostEnvironment;
            _context = context;
        }

        [HttpGet]
        public IActionResult Home()
        {
            var announcements = _context.Announcements.OrderByDescending(a => a.DateCreated).ToList();
            var studentVM = new StudentDashboardViewModel
            {
                AnnouncementsList = announcements,
                ProfilePicture = "ccs_logo.png" // Default if everything else fails
            };

            // 1. Get the JSON string from session
            var modelJson = HttpContext.Session.GetString("AccountModel");

            if (!string.IsNullOrEmpty(modelJson))
            {
                // 2. Turn the JSON back into an Account object
                var account = JsonConvert.DeserializeObject<Account>(modelJson);

                // 3. Set the picture from the account data
                if (!string.IsNullOrEmpty(account.ProfilePicture))
                {
                    studentVM.ProfilePicture = account.ProfilePicture;
                }
                studentVM.Name = $"{account.FirstName} {account.MiddleName} {account.LastName}";
                studentVM.Id = account.Id;
                studentVM.Course = account.Course;
                studentVM.CourseLevel = account.CourseLevel;
                studentVM.Email = account.Email;
                studentVM.Address = account.Address;
                studentVM.SessionsRemaining = account.SessionsRemaining;
                studentVM.Points = account.Points;
            }

            return View(studentVM);

            /*var AccountJson = TempData["AccountModel"] as string;
            Account model = null;
            if (!string.IsNullOrEmpty(AccountJson))
            {
                model = JsonConvert.DeserializeObject<Account>(AccountJson);
            }*/
            /*
            var modelJson = HttpContext.Session.GetString("AccountModel");
            var userModel = modelJson == null ? null : JsonConvert.DeserializeObject<Account>(modelJson);
            if (modelJson != null)
            {
                var model = System.Text.Json.JsonSerializer.Deserialize<Account>(modelJson);
                return View(model);
            }
            */
            //  return RedirectToAction("Login", "Login");
        }

        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Home(Student student)
        {
            return View(student);
        }*/

        public IActionResult Search()
        {
            /*
            var modelJson = HttpContext.Session.GetString("AccountModel");
            var userModel = modelJson == null ? null : JsonConvert.DeserializeObject<Account>(modelJson);
            if (modelJson != null)
            {
                var model = System.Text.Json.JsonSerializer.Deserialize<Account>(modelJson);
                return View(model);
            }
            */
            return View();
        }
        public IActionResult Notifications()
        {
            return View();
        }
        public IActionResult Reservation()
        {
            return View();
        }
        public IActionResult History()
        {
            return View();
        }
        [HttpGet]
        public IActionResult EditProfile()
        {

            var modelJson = HttpContext.Session.GetString("AccountModel");
            var userModel = modelJson == null ? null : JsonConvert.DeserializeObject<Account>(modelJson);
            if (modelJson != null)
            {
                var model = System.Text.Json.JsonSerializer.Deserialize<Account>(modelJson);
                return View(model);
            }

            return View();
        }

        [HttpPost]
        public IActionResult EditProfile(Account updatedModel, IFormFile? profileImage)
        {

            if (ModelState.IsValid)
            {
                if (profileImage != null && profileImage.Length > 0)
                {
                    // 1. Create a unique filename
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(profileImage.FileName);

                    // 2. Define the save path (wwwroot/images)
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

                    // 3. Use synchronous CopyTo
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        profileImage.CopyTo(stream);
                    }

                    // 4. Update the model with the new filename
                    updatedModel.ProfilePicture = fileName;
                }
                _context.Accounts.Update(updatedModel);
                _context.SaveChanges();

                var updatedJson = JsonConvert.SerializeObject(updatedModel);
                HttpContext.Session.SetString("AccountModel", updatedJson);

                return RedirectToAction("EditProfile");
            }

            /*var modelJson = HttpContext.Session.GetString("AccountModel");
            var userModel = modelJson == null ? null : JsonConvert.DeserializeObject<Account>(modelJson);
            if (modelJson != null)
            {
                var model = System.Text.Json.JsonSerializer.Deserialize<Account>(modelJson);
                return View(model);
            }*/

            return View(updatedModel);
        }

        [HttpGet]
        public IActionResult SitInHistory()
        {
            var modelJson = HttpContext.Session.GetString("AccountModel");
            var loggedInStudent = JsonConvert.DeserializeObject<Account>(modelJson);
            var studentId = loggedInStudent.Id;

            var sitInRecords = _context.CurrentSitIns
                                            .OrderByDescending(s => s.SitId)
                                            .Where(s => s.Id == studentId)
                                            .AsQueryable();

            return View(sitInRecords);
        }

        [HttpGet]
        public IActionResult Feedback(int sitInId)
        {
            var record = _context.CurrentSitIns.FirstOrDefault(s => s.SitId == sitInId);
            if (record == null) return NotFound();

            return View(record);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitFeedback(int sitId, string feedbackText)
        {
            var record = await _context.CurrentSitIns.FindAsync(sitId);
            if (record == null) return NotFound();

            // Update the feedback field
            record.Feedback = feedbackText;

            _context.CurrentSitIns.Update(record);
            await _context.SaveChangesAsync();

            return RedirectToAction("SitInHistory");
        }

        public IActionResult ViewRemainingSession()
        {
            var modelJson = HttpContext.Session.GetString("AccountModel");
            var loggedInStudent = JsonConvert.DeserializeObject<Account>(modelJson);

            var studentId = loggedInStudent.Id;

            var student = _context.Accounts.FirstOrDefault(s => s.Id == studentId);
            return View(student);
        }
    }
}
