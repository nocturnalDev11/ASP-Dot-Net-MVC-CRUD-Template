using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ASP_Dot_Net_MVC_CRUD_Template.Models;
using System.Security.Claims; 
using System.Linq;

namespace ASP_Dot_Net_MVC_CRUD_Template.Controllers
{
    public class ItemController : Controller
    {
        private readonly AppDbContext _context;

        public ItemController(AppDbContext context)
        {
            _context = context;
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : throw new Exception("User ID not found in token.");
        }

        private IActionResult? CheckSession()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("JWT")))
            {
                return RedirectToAction("Login", "User");
            }

            return null;
        }

        public IActionResult Index()
        {
            var sessionCheck = CheckSession();
            if (sessionCheck != null) return sessionCheck;

            return View(_context.Items.ToList());
        }

        public IActionResult Create()
        {
            var sessionCheck = CheckSession();
            if (sessionCheck != null) return sessionCheck;

            return View();
        }

        [HttpPost]
        public IActionResult Create(Item item)
        {
            var sessionCheck = CheckSession();
            if (sessionCheck != null) return sessionCheck;

            if (ModelState.IsValid)
            {
                try
                {
                    item.UserId = GetCurrentUserId();
                    _context.Items.Add(item);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message;
                }
            }

            return View(item);
        }

        public IActionResult Edit(int id)
        {
            var sessionCheck = CheckSession();
            if (sessionCheck != null) return sessionCheck;

            var item = _context.Items.Find(id);
            return item == null ? NotFound() : View(item);
        }

        [HttpPost]
        public IActionResult Edit(Item item)
        {
            var sessionCheck = CheckSession();
            if (sessionCheck != null) return sessionCheck;

            _context.Items.Update(item);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var sessionCheck = CheckSession();
            if (sessionCheck != null) return sessionCheck;

            var item = _context.Items.Find(id);
            return item == null ? NotFound() : View(item);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var sessionCheck = CheckSession();
            if (sessionCheck != null) return sessionCheck;

            var item = _context.Items.Find(id);
            if (item != null)
            {
                _context.Items.Remove(item);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var sessionCheck = CheckSession();
            if (sessionCheck != null) return sessionCheck;

            var item = _context.Items.Find(id);
            return item == null ? NotFound() : View(item);
        }
    }
}
