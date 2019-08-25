using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ActivityCenter.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace ActivityCenter.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext { get; set; }

        public HomeController(MyContext context)
        {
            dbContext = context;
        }
        
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost("register")]
        public IActionResult registerUser(User newUser)
        {
            if (ModelState.IsValid)
            {
                if (dbContext.Users.Any(u => u.Email == newUser.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use!");
                    return View("Index");
                }

                PasswordHasher<User> hasher = new PasswordHasher<User>();
                string hash = hasher.HashPassword(newUser, newUser.Password);
                newUser.Password = hash;
                dbContext.Users.Add(newUser);
                dbContext.SaveChanges();

                HttpContext.Session.SetInt32("UserId", newUser.UserId);

                return RedirectToAction("Dashboard");
            }
            else
            {
                return View("Index");
            }
        }

        [HttpPost("login")]
        public IActionResult loginUser(LogUser user)
        {
            if (ModelState.IsValid)
            {
                User check = dbContext.Users.FirstOrDefault(u => u.Email == user.LogEmail);

                if (check == null)
                {
                    ModelState.AddModelError("LogEmail", "Invalid email or password");
                    return View("Index");
                }

                PasswordHasher<LogUser> hasher = new PasswordHasher<LogUser>();
                var result = hasher.VerifyHashedPassword(user, check.Password, user.LogPassword);
                if (result == 0)
                {
                    ModelState.AddModelError("LogEmail", "Invalid email or password");
                    return View("Index");
                }

                HttpContext.Session.SetInt32("UserId", check.UserId);
                return RedirectToAction("Dashboard");
            }
            else
            {
                return View("Index");
            }
        }

        [HttpGet("/logout")]
        public IActionResult logoutUser()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpGet("/dashboard")]
        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index");
            }
            List<Event> TEST = dbContext
                .Events
                    .Include(p => p.Participants)
                    .Include(c => c.Creator)  
                    .OrderByDescending(a => a.created_at)
                    .Where(e => e.Date >= DateTime.Today)
                    .ToList(); 
            int? user = HttpContext.Session.GetInt32("UserId");
            ViewBag.User = dbContext.Users.FirstOrDefault(u => u.UserId == user);
            return View(TEST);
        }

        [HttpGet("/new")]
        public IActionResult newEvent()
        {
            int? user = HttpContext.Session.GetInt32("UserId");
            ViewBag.User = dbContext.Users.FirstOrDefault(u => u.UserId == user);
            return View("new");
        }

        [HttpPost("add")]
        public IActionResult addEvent(Event newEvent)
        {
            if (ModelState.IsValid)
            {
                var newTime = newEvent.Date.ToString("yyyy-MM-dd") + " " + newEvent.Time.ToString("T");
                DateTime finalTime = Convert.ToDateTime(newTime);
                
                newEvent.Date = finalTime;
                newEvent.Time = finalTime;

                dbContext.Add(newEvent);
                dbContext.SaveChanges();
                var temp = newEvent.EventId;
                return RedirectToAction("Details", dbContext.Events.FirstOrDefault(a => a.EventId == temp));
            }
            else
            {
                int? user = HttpContext.Session.GetInt32("UserId");
                ViewBag.User = dbContext.Users.FirstOrDefault(u => u.UserId == user);
                return View("New");
            }
        }

        [HttpGet("/delete/{EventId}")]
        public IActionResult deleteEvent(int? EventId)
        {
            Event retrievedEvent = dbContext.Events.SingleOrDefault(e => e.EventId == EventId);            
            dbContext.Events.Remove(retrievedEvent);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet("/details/{EventId}")]
        public IActionResult Details(int? EventId)
        {
            var anEvent = dbContext
                .Events
                    .Include(c => c.Creator)
                    .Include(p => p.Participants)
                            .ThenInclude(z => z.User)
                .FirstOrDefault(e => e.EventId == EventId);

            return View(anEvent);
        }


        [HttpPost("/join")]
        public IActionResult joinEvent(int eventId, int userId)
        {
            Participant newParticipant = new Participant()
            {
                EventId = eventId,
                UserId = (int)userId,

            };
            dbContext.Participants.Add(newParticipant);           
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard", new {id=HttpContext.Session.GetInt32("UserId")});
        }

        [HttpPost("/leave")]
        public IActionResult leaveEvent(int eventId, int userId)
        {
            int remPart = dbContext.Participants.FirstOrDefault(p => p.UserId == userId && p.EventId == eventId).ParticipantId;
            var test = dbContext.Participants.FirstOrDefault(p => p.ParticipantId == remPart);
            dbContext.Participants.Remove(test);         
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard", new {id=HttpContext.Session.GetInt32("UserId")});
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
