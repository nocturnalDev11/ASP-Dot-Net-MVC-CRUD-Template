using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ASP_Dot_Net_MVC_CRUD_Template.Models;

namespace ASP_Dot_Net_MVC_CRUD_Template.Controllers;

public class ItemController : Controller
{
    private readonly AppDbContext _context;

    public ItemController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("JWT")))
            return RedirectToAction("Login", "User");

        return View(_context.Items.ToList());
    }

    public IActionResult Create() => View();

    [HttpPost]
    public IActionResult Create(Item item)
    {
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
        var item = _context.Items.Find(id);
        return item == null ? NotFound() : View(item);
    }

    [HttpPost]
    public IActionResult Edit(Item item)
    {
        _context.Items.Update(item);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    public IActionResult Delete(int id)
    {
        var item = _context.Items.Find(id);
        return item == null ? NotFound() : View(item);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id)
    {
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
        var item = _context.Items.Find(id);
        return item == null ? NotFound() : View(item);
    }
}
