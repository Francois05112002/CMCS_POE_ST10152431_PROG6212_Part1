using CMCS_POE_ST10152431_PROG6212_Part1.Data;
using CMCS_POE_ST10152431_PROG6212_Part1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading.Tasks;

public class AccountController : Controller
{
    private readonly AppDbContext _context;

    public AccountController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Register(Lecturer lecturer, IFormFile Resume)
    {
        if (ModelState.IsValid)
        {
            if (Resume != null && Resume.Length > 0)
            {
                var filePath = Path.Combine("wwwroot/resumes", Resume.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await Resume.CopyToAsync(stream);
                }
                lecturer.ResumePath = filePath;
            }

            _context.Lecturers.Add(lecturer);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }
        return RedirectToAction("LogInUser", "Home", lecturer);
    }

    [HttpPost]
    public async Task<IActionResult> Login(string Surname, string Password)
    {
        var lecturer = await _context.Lecturers
            .FirstOrDefaultAsync(l => l.Surname == Surname && l.Password == Password);

        if (lecturer != null)
        {
            // Handle successful login
            return RedirectToAction("Index", "Home");
        }

        // Handle login failure
        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return View();
    }
}

