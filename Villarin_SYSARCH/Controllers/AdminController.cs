using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Villarin_SYSARCH.Data;
using Villarin_SYSARCH.Models;
using Villarin_SYSARCH.ViewModels;

namespace Villarin_SYSARCH.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            var purposeCounts = _context.CurrentSitIns
                          .GroupBy(s => s.Purpose)
                          .Select(g => new {
                              PurposeName = g.Key,
                              Count = g.Count()
                          })
                          .ToList();
            ViewBag.Labels = purposeCounts.Select(x => x.PurposeName).ToArray();
            ViewBag.Data = purposeCounts.Select(x => x.Count).ToArray();

            var sitInCounts = _context.CurrentSitIns.Count();
            ViewBag.CurrentSitInCounts = sitInCounts;

            var totalStudents = _context.Accounts.Count();
            ViewBag.TotalStudents = totalStudents;

            var activeSitIns = _context.CurrentSitIns.Count(s => s.Status == "Sitting In");
            ViewBag.ActiveSitIns = activeSitIns;

            var announcements = _context.Announcements.ToList();
            var announcementList = new DashboardViewModel
            {
                AnnouncementsList = announcements,
            };

            return View(announcementList);
        }

        [HttpPost]
        public IActionResult Dashboard(DashboardViewModel announcement)
        {
            var purposeCounts = _context.CurrentSitIns
                          .GroupBy(s => s.Purpose)
                          .Select(g => new {
                              PurposeName = g.Key,
                              Count = g.Count()
                          })
                          .ToList();
            ViewBag.Labels = purposeCounts.Select(x => x.PurposeName).ToArray();
            ViewBag.Data = purposeCounts.Select(x => x.Count).ToArray();

            var sitInCounts = _context.CurrentSitIns.Count();
            ViewBag.SitInCounts = sitInCounts;

            var totalStudents = _context.Accounts.Count();
            ViewBag.TotalStudents = totalStudents;

            var activeSitIns = _context.CurrentSitIns.Count(s => s.Status == "Sitting In");
            ViewBag.ActiveSitIns = activeSitIns;

            var announcements = _context.Announcements.ToList();
            var announcementList = new DashboardViewModel
            {
                AnnouncementsList = announcements,
            };
            if (ModelState.IsValid)
            {

                var newAnnouncement = new Announcement
                {
                    Description = announcement.Description,
                    DateCreated = DateTime.Now,
                    Author = announcement.Author
                };

                _context.Announcements.Add(newAnnouncement);
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            return View(announcement);
        }

        public async Task<IActionResult> StudentInfo(string searchString)
        {
            // needs revision to retrieve STUDENTS ONLY (probably a ViewModel for students)
            var students = _context.Accounts.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.Id.ToString().Contains(searchString));
            }

            return View(await students.ToListAsync());

        }

        [HttpGet]
        public IActionResult SitInForm()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SitInForm(CurrentSitIn student)
        {
            ModelState.Remove("Status");

            if (ModelState.IsValid) //if all input fields have input
            {
                var findStudent = await _context.Accounts.FirstOrDefaultAsync(s => s.Id == student.Id);

                if (findStudent == null) //checks if student isn't registered
                {
                    ViewBag.IsStudentRegistered = false;
                    return View(student);
                } else // student is registered
                {
                    var editStudentRemainingSession = await _context.Accounts.FirstOrDefaultAsync(e => e.Id == student.Id);

                    ViewBag.IsStudentRegistered = true;
                    var sitInStudent = await _context.CurrentSitIns.FirstOrDefaultAsync(s => s.Id == student.Id);

                    if (sitInStudent == null) //checks if student isn't in the CurrentSitIn table
                    {
                        ViewBag.IsStudentFound = false;
                        student.Status = "sitting in";
                        --student.SessionRemaining;
                        --editStudentRemainingSession.SessionsRemaining;
                        _context.CurrentSitIns.Add(student);
                        await _context.SaveChangesAsync();
                        return View();
                    } else //student is in the table
                    {
                        ViewBag.IsStudentFound = true;
                        bool isSittingIn = sitInStudent.Status.Equals("sitting in", StringComparison.OrdinalIgnoreCase);

                        if (isSittingIn == true) //checks if student is currently sitting in
                        {
                            ViewBag.IsCurrentlySittingIn = true;
                            ModelState.Clear();
                            return View();
                        } else //student isn't currently sitting in
                        {
                            ViewBag.IsCurrentlySittingIn = false;
                            student.Status = "sitting in";
                            --student.SessionRemaining;
                            --editStudentRemainingSession.SessionsRemaining;
                            _context.CurrentSitIns.Add(student);
                            await _context.SaveChangesAsync();
                        }
                    }
                }
                return RedirectToAction("SitInForm");
            } else
            {
                return View(student);
            }

                /*if (findStudent == null) //checks if student id isn't found in the sit in table, then proceed to adding student to CurrentSitIn table
                {
                    ViewBag.IsStudentFound = false;
                    student.Status = "sitting in";
                    _context.CurrentSitIns.Add(student);
                    _context.SaveChanges();
                } else //student is found in the sit in table, check if they're currently sitting in or not
                {
                    var foundStudent = _context.CurrentSitIns.Find(student.Id);
                    ViewBag.IsStudentFound = true;
                    bool isSittingIn = foundStudent.Status.Equals("sitting in", StringComparison.OrdinalIgnoreCase);

                    if (isSittingIn == true) //student is currently sitting in
                    {
                        ViewBag.IsCurrentlySittingIn = true;
                        return View(student);
                    } else //student is not currently sitting in
                    {
                        ViewBag.IsCurrentlySittingIn = false;
                        foundStudent.Status = "sitting in";
                        _context.CurrentSitIns.Add(foundStudent);
                        _context.SaveChanges();
                    }
                }
                return View();

            }else
            {
                return View(student);
            }*/

        }

        [HttpGet]
        public IActionResult SitInRecord()
        {
            var currentStudents = _context.CurrentSitIns
                                            .OrderByDescending(c => c.Status)
                                            .ThenByDescending(c => c.SitId)
                                            .ToList();

            return View(currentStudents);
        }

        
        public IActionResult SitInRecord(CurrentSitIn studentId)
        {
            return View();
        }

        [HttpGet]
        public IActionResult ViewFeedback(int sitId)
        {
            var record = _context.CurrentSitIns.FirstOrDefault(s => s.SitId == sitId);

            if (record == null) return NotFound();

            // Ensure only the correct student can see their own feedback
            // (Assuming you still have the logic to check the session/account)

            return View(record);
        }

        [HttpPost] // Good practice to use POST for state-changing operations
        public async Task<IActionResult> FinishSitIn(int sitId)
        {
            var record = await _context.CurrentSitIns.FindAsync(sitId);

            if (record == null)
            {
                return NotFound();
            }

            // Update the status
            record.Status = "Finished";

            // Optionally: if you have a separate history table, you might move/copy this record here

            _context.CurrentSitIns.Update(record);
            await _context.SaveChangesAsync();

            return RedirectToAction("SitInRecord"); // Redirect back to the list
        }

        [HttpGet]
        public IActionResult SearchStudent()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SearchStudent(SearchDetailsViewModel search)
        {
            ModelState.Remove("Name");
            ModelState.Remove("Course");
            ModelState.Remove("CourseLevel");
            ModelState.Remove("Id");

            var accounts = _context.Accounts.ToList();

            if (ModelState.IsValid)
            {

                foreach (var s in accounts)
                {
                    if (s.Id == search.SearchId)
                    {
                        var searchedStudent = new SearchDetailsViewModel
                        {
                            Id = s.Id,
                            Name = $"{s.FirstName} {s.MiddleName} {s.LastName}",
                            CourseLevel = s.CourseLevel,
                            Course = s.Course,
                            SessionRemaining = s.SessionsRemaining
                        };
                        return View(searchedStudent);

                    }

                }
            }
            ViewBag.isIdFound = false;
            return View(search);
        }
    }
}
