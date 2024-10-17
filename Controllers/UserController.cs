using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using DoAnCoSo.Models; // Thay thế bằng namespace của model người dùng trong ứng dụng của bạn

[Route("[controller]")]
public class UserController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;

    public UserController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }
    //public async Task<IActionResult> Login(string username, string password)
    //{
    //    var user = await _context.Users
    //        .Where(u => u.Username == username && u.Password == password)
    //        .FirstOrDefaultAsync();

    //    return RedirectToAction("Index", "Home");
    //}

    [HttpGet]
    public async Task<IActionResult> GetUser(string id)
    {
        if (id == null)
        {
            return BadRequest("User ID must be provided.");
        }

        var user = await _userManager.FindByIdAsync(id);

        if (user == null)
        {
            return NotFound("User not found.");
        }

        return View(user);
    }

    public IActionResult DecodePassword(string base64EncodedPassword)
    {
        string clearTextPassword = PasswordUtility.DecodeBase64Password(base64EncodedPassword);
        ViewBag.ClearTextPassword = clearTextPassword; // Truyền vào ViewBag để hiển thị lên view
        return View();
    }
}
