using System;
using System.Diagnostics;
using System.Linq;
using ActivityCenter.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ActivityCenter.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(MyContext context)
        {
            DbContext = context;
        }

        private MyContext DbContext { get; }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost("register")]
        public IActionResult RegisterUser(User newUser)
        {
            if (!ModelState.IsValid) return View("Index");
            if (DbContext.Users.Any(u => u.Email == newUser.Email))
            {
                ModelState.AddModelError("Email", "Email already in use!");
                return View("Index");
            }

            var hasher = new PasswordHasher<User>();
            var hash = hasher.HashPassword(newUser, newUser.Password);
            newUser.Password = hash;
            DbContext.Users.Add(newUser);
            DbContext.SaveChanges();

            HttpContext.Session.SetInt32("UserId", newUser.UserId);

            return RedirectToAction("Dashboard");
        }

        [HttpPost("login")]
        public IActionResult LoginUser(LogUser user)
        {
            if (!ModelState.IsValid) return View("Index");
            var check = DbContext.Users.FirstOrDefault(u => u.Email == user.LogEmail);

            if (check == null)
            {
                ModelState.AddModelError("LogEmail", "Invalid email or password");
                return View("Index");
            }

            var hasher = new PasswordHasher<LogUser>();
            var result = hasher.VerifyHashedPassword(user, check.Password, user.LogPassword);
            if (result == 0)
            {
                ModelState.AddModelError("LogEmail", "Invalid email or password");
                return View("Index");
            }

            HttpContext.Session.SetInt32("UserId", check.UserId);
            return RedirectToAction("Dashboard");
        }

        [HttpGet("/logout")]
        public IActionResult LogoutUser()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpGet("/dashboard")]
        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetInt32("UserId") == null) return RedirectToAction("Index");
            var dashboardInput = DbContext
                .Events
                .Include(p => p.Participants)
                .ThenInclude(u => u.Event)
                .Include(c => c.Creator)
                .OrderByDescending(a => a.CreatedAt)
                .Where(e => e.Date >= DateTime.Today)
                .ToList();
            var user = HttpContext.Session.GetInt32("UserId");
            ViewBag.User = DbContext.Users.FirstOrDefault(u => u.UserId == user);
            return View(dashboardInput);
        }

        public static bool IsOverlap(Event value1, Event value2)
        {

            if (value2.Date == value1.Date)
            {
                return true;
            }

            return false;
        }

        [HttpGet("/new")]
        public IActionResult NewEvent()
        {
            var user = HttpContext.Session.GetInt32("UserId");
            ViewBag.User = DbContext.Users.FirstOrDefault(u => u.UserId == user);
            return View("new");
        }

        [HttpPost("add")]
        public IActionResult AddEvent(Event newEvent)
        {
            if (ModelState.IsValid)
            {
                var newTime = newEvent.Date.ToString("yyyy-MM-dd") + " " + newEvent.Time.ToString("T");
                var finalTime = Convert.ToDateTime(newTime);

                newEvent.Date = finalTime;
                
                switch (newEvent.DurationUnits)
                {
                    case "days":
                        newEvent.NormalizedDuration = newEvent.Duration * 1440;
                        break;
                    case "hours":
                        newEvent.NormalizedDuration = newEvent.Duration * 60;
                        break;
                    default:
                        newEvent.NormalizedDuration = newEvent.Duration;
                        break;
                }

                newEvent.EndDate = newEvent.Date.AddMinutes(newEvent.NormalizedDuration);
                
                DbContext.Add(newEvent);
                DbContext.SaveChanges();
                var temp = newEvent.EventId;
                return RedirectToAction("Details", DbContext.Events.FirstOrDefault(a => a.EventId == temp));
            }

            var user = HttpContext.Session.GetInt32("UserId");
            ViewBag.User = DbContext.Users.FirstOrDefault(u => u.UserId == user);
            return View("New");
        }

        [HttpGet("/delete/{EventId}")]
        public IActionResult DeleteEvent(int? eventId)
        {
            var retrievedEvent = DbContext.Events.SingleOrDefault(e => e.EventId == eventId);
            DbContext.Events.Remove(retrievedEvent);
            DbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet("/details/{EventId}")]
        public IActionResult Details(int? eventId)
        {
            var anEvent = DbContext
                .Events
                .Include(c => c.Creator)
                .Include(p => p.Participants)
                .ThenInclude(z => z.User)
                .FirstOrDefault(e => e.EventId == eventId);

            return View(anEvent);
        }


        [HttpPost("/join")]
        public IActionResult JoinEvent(int eventId, int userId)
        {
            var newParticipant = new Participant
            {
                EventId = eventId,
                UserId = userId
            };
            DbContext.Participants.Add(newParticipant);
            DbContext.SaveChanges();
            return RedirectToAction("Dashboard", new {id = HttpContext.Session.GetInt32("UserId")});
        }

        [HttpPost("/leave")]
        public IActionResult LeaveEvent(int eventId, int userId)
        {
            var removeParticipant = DbContext
                .Participants
                .FirstOrDefault(p => p.UserId == userId && p.EventId == eventId);

            if (removeParticipant != null) DbContext.Participants.Remove(removeParticipant);
            DbContext.SaveChanges();
            return RedirectToAction("Dashboard", new {id = HttpContext.Session.GetInt32("UserId")});
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}