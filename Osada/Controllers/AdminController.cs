using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Osada.Data;

namespace Osada.Controllers;

[Authorize(Roles = RoleNames.Admin)]
public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<AdminController> _logger;

    public AdminController(
        ApplicationDbContext context,
        ILogger<AdminController> logger)
    {
        _context = context;
        _logger = logger;
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

        ViewBag.Villages = await _context.AdminVillageOverviews
            .AsNoTracking()
            .OrderBy(v => v.VillageId)
            .ToListAsync();

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> NewDay()
    {
        await using var transaction =
            await _context.Database.BeginTransactionAsync();

        try
        {
            var villages = await _context.Villages
                .Include(v => v.VillageResource)
                .ToListAsync();

            foreach (var village in villages)
            {
                long power = await _context.Database
                    .SqlQuery<long>(
                        $"SELECT dbo.fn_GetVillagePower({village.VillageId}) AS [Value]")
                    .SingleAsync();

                village.Points = checked((int)power);

                var resources = village.VillageResource;

                if (resources == null)
                {
                    throw new InvalidOperationException(
                        $"Wioska {village.VillageId} nie posiada rekordu zasobów.");
                }

                resources.WoodAmount = checked(
                    resources.WoodAmount + resources.WoodProduction);

                resources.ClayAmount = checked(
                    resources.ClayAmount + resources.ClayProduction);

                resources.IronAmount = checked(
                    resources.IronAmount + resources.IronProduction);

                resources.FoodAmount = checked(
                    resources.FoodAmount + resources.FoodProduction);
            }

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            _logger.LogInformation(
                "Administrator {Admin} rozpoczął nowy dzień. Zaktualizowano {Count} wiosek",
                User.Identity?.Name,
                villages.Count);

            TempData["Success"] =
                $"Nowy dzień rozpoczęty! Zaktualizowano {villages.Count} wiosek";
        }
        catch (OverflowException ex)
        {
            await transaction.RollbackAsync();

            _logger.LogError(
                ex,
                "Przepełnienie wartości podczas rozpoczynania nowego dnia");

            TempData["Error"] =
                "Nie udało się rozpocząć nowego dnia. Jedna z wartości przekracza dopuszczalny zakres";
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();

            _logger.LogError(
                ex,
                "Błąd podczas rozpoczynania nowego dnia");

            TempData["Error"] =
                "Wystąpił błąd podczas rozpoczynania nowego dnia. Żadne zmiany nie zostały zapisane";
        }

        return RedirectToAction(nameof(Index));
    }
}