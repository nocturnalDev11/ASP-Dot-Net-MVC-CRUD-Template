using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ASP_Dot_Net_MVC_CRUD_Template.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System;

namespace ASP_Dot_Net_MVC_CRUD_Template.Controllers
{
    public class ItemController : Controller
    {
        private readonly AppDbContext _context;

        public ItemController(AppDbContext context)
        {
            _context = context;
        }

        private int GetCurrentUserIdFromJwt()
        {
            var jwt = HttpContext.Session.GetString("JWT");
            if (string.IsNullOrEmpty(jwt))
                throw new Exception("JWT not found in session.");

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);

            var userIdClaim = token.Claims.FirstOrDefault(c =>
                c.Type == "nameid" ||
                c.Type == "sub" ||
                c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
            );

            if (userIdClaim == null)
                throw new Exception("UserId claim not found in JWT.");

            return int.Parse(userIdClaim.Value);
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

            try
            {
                item.UserId = GetCurrentUserIdFromJwt();

                Console.WriteLine("Create POST reached");
                Console.WriteLine($"Item Name: {item.SampleString}, UserId: {item.UserId}");
                Console.WriteLine("JWT from session: " + HttpContext.Session.GetString("JWT"));

                if (ModelState.IsValid)
                {
                    _context.Items.Add(item);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                Console.WriteLine("ERROR: " + ex.Message);
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
