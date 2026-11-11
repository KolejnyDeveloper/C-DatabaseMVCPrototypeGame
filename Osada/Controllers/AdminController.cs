using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Osada.Data;

namespace Osada.Controllers;

[Authorize(Roles = RoleNames.Admin)]
public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;

    public AdminController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.UserCount = await _context.Users.CountAsync();

        ViewBag.ActiveUserCount = await _context.Users
            .CountAsync(u => u.IsActive);

        ViewBag.VillageCount = await _context.Villages.CountAsync();

        ViewBag.AttackCount = await _context.Attacks.CountAsync();

        ViewBag.TransactionCount =
            await _context.ResourceTransactions.CountAsync();

        return View();
    }
}