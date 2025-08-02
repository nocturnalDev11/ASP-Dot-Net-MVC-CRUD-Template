using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ASP_Dot_Net_MVC_CRUD_Template.Models;

namespace ASP_Dot_Net_MVC_CRUD_Template.Controllers
{
    public class ItemController : Controller
    {
        private readonly AppDbContext _context;

        public ItemController(AppDbContext context)
        {
            _context = context;
        }

        private IActionResult CheckSession()
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
                _context.Items.Add(item);
                _context.SaveChanges();
                return RedirectToAction("Index");
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
