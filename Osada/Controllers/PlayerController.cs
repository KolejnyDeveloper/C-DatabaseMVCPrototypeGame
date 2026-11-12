
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Osada.Data;
using Osada.Models.ViewModels;
using System.Security.Claims;
using Osada.Models;
using Microsoft.Data.SqlClient;
using System.Data;


namespace Osada.Controllers;

[Authorize(Roles = RoleNames.Player)]
public class PlayerController : Controller
{
    private readonly ApplicationDbContext _context;

    public PlayerController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var userIdValue = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdValue, out int userId))
            return Forbid();

        var villages = await _context.Villages
            .AsNoTracking()
            .Where(v => v.PlayerId == userId)
            .OrderBy(v => v.Name)
            .ToListAsync();

        return View(villages);
    }

    public async Task<IActionResult> Village(
        int id,
        string section = "TownHall")
    {
        var userIdValue = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdValue, out int userId))
            return Forbid();

        var village = await _context.Villages
            .AsNoTracking()
            .Include(v => v.VillageResource)
            .Include(v => v.VillageBuildings)
                .ThenInclude(vb => vb.BuildingType)
            .FirstOrDefaultAsync(v =>
                v.VillageId == id &&
                v.PlayerId == userId);

        if (village == null)
            return NotFound();

        var allowedSections = new[]
        {
            "TownHall",
            "BuildPlan",
            "Barracks",
            "Map",
            "BattleHistory",
            "Settings"
        };

        if (!allowedSections.Contains(section))
            section = "TownHall";

        bool hasBarracks = village.VillageBuildings.Any(
            vb =>
                vb.BuildingType.Name.Contains(
                    "Koszary",
                    StringComparison.OrdinalIgnoreCase)
        );

        if (section == "Barracks" && !hasBarracks)
            section = "TownHall";

        var resources = village.VillageResource;

        var model = new VillageDashboardViewModel
        {
            VillageId = village.VillageId,
            Name = village.Name,
            X = village.X,
            Y = village.Y,
            Points = village.Points,
            Section = section,
            HasBarracks = hasBarracks,

            WoodAmount = resources?.WoodAmount ?? 0,
            ClayAmount = resources?.ClayAmount ?? 0,
            IronAmount = resources?.IronAmount ?? 0,
            FoodAmount = resources?.FoodAmount ?? 0,

            WoodProduction = resources?.WoodProduction ?? 0,
            ClayProduction = resources?.ClayProduction ?? 0,
            IronProduction = resources?.IronProduction ?? 0,
            FoodProduction = resources?.FoodProduction ?? 0,

            Buildings = village.VillageBuildings
                .Select(vb => new BuildingItemViewModel
                {
                    Name = vb.BuildingType.Name,
                    Level = vb.Level
                })
                .OrderBy(b => b.Name)
                .ToList()
        };

        if (section == "Barracks")
        {
            model.Barracks = new BarracksViewModel
            {
                VillageId = village.VillageId,

                WoodAmount = resources?.WoodAmount ?? 0,
                ClayAmount = resources?.ClayAmount ?? 0,
                IronAmount = resources?.IronAmount ?? 0,
                FoodAmount = resources?.FoodAmount ?? 0,

                OwnedUnits = await _context.VillageUnits
                    .AsNoTracking()
                    .Where(u => u.VillageId == village.VillageId)
                    .Include(u => u.UnitType)
                    .OrderBy(u => u.UnitType.Name)
                    .Select(u => new VillageUnitItemViewModel
                    {
                        UnitTypeId = u.UnitTypeId,
                        Name = u.UnitType.Name,
                        Quantity = u.Quantity,
                        Attack = u.UnitType.Attack,
                        Defense = u.UnitType.Defense
                    })
                    .ToListAsync(),

                RecruitableUnits = await _context.UnitTypes
                    .AsNoTracking()
                    .OrderBy(u => u.Name)
                    .Select(u => new RecruitableUnitViewModel
                    {
                        UnitTypeId = u.UnitTypeId,
                        Name = u.Name,
                        Attack = u.Attack,
                        Defense = u.Defense,
                        RecruitmentCostWood = u.RecruitmentCostWood,
                        RecruitmentCostClay = u.RecruitmentCostClay,
                        RecruitmentCostIron = u.RecruitmentCostIron
                    })
                    .ToListAsync()
            };
        }


        if (section == "BuildPlan")
        {
            var existingBuildings = await _context.VillageBuildings
                .AsNoTracking()
                .Where(vb => vb.VillageId == village.VillageId)
                .ToListAsync();

            var allBuildings = await _context.BuildingTypes
                .AsNoTracking()
                .OrderBy(b => b.Name)
                .ToListAsync();

            model.BuildPlan = new BuildPlanViewModel
            {
                VillageId = village.VillageId,

                WoodAmount = resources?.WoodAmount ?? 0,
                ClayAmount = resources?.ClayAmount ?? 0,
                IronAmount = resources?.IronAmount ?? 0,
                FoodAmount = resources?.FoodAmount ?? 0,

                Buildings = allBuildings.Select(building =>
                {
                    var existing = existingBuildings.FirstOrDefault(
                        vb => vb.BuildingTypeId == building.BuildingTypeId);

                    int currentLevel = existing?.Level ?? 0;
                    int nextLevel = currentLevel + 1;

                    return new BuildPlanItemViewModel
                    {
                        BuildingTypeId = building.BuildingTypeId,
                        Name = building.Name,
                        CurrentLevel = currentLevel,

                        CostWood = building.BaseCostWood * nextLevel,
                        CostClay = building.BaseCostClay * nextLevel,
                        CostIron = building.BaseCostIron * nextLevel
                    };
                }).ToList()
            };
        }


        if (section == "Map")
        {
            model.WorldMap = new WorldMapViewModel
            {
                CurrentVillageId = village.VillageId,

                Villages = await _context.Villages
                    .AsNoTracking()
                    .Include(v => v.Player)
                    .OrderBy(v => v.Name)
                    .Select(v => new WorldMapVillageItemViewModel
                    {
                        VillageId = v.VillageId,
                        Name = v.Name,

                        OwnerName = v.Player != null
                            ? v.Player.Username
                            : "Brak właściciela",

                        X = v.X,
                        Y = v.Y,
                        Points = v.Points,

                        IsOwnedByCurrentPlayer = v.PlayerId == userId
                    })
                    .ToListAsync()
            };
        }




        return View("Village/Index", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RenameVillage(int id, string newName)
    {
        var userIdValue = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdValue, out int userId))
            return Forbid();

        var village = await _context.Villages
            .FirstOrDefaultAsync(v =>
                v.VillageId == id &&
                v.PlayerId == userId);

        if (village == null)
            return NotFound();

        newName = newName?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(newName))
        {
            TempData["Error"] = "Nazwa wioski nie może być pusta";
            return RedirectToAction(
                nameof(Village),
                new { id, section = "Settings" });
        }

        if (newName.Length > 50)
        {
            TempData["Error"] = "Nazwa może mieć maksymalnie 50 znaków";
            return RedirectToAction(
                nameof(Village),
                new { id, section = "Settings" });
        }

        village.Name = newName;

        await _context.SaveChangesAsync();

        TempData["Success"] = "Nazwa wioski została zmieniona";

        return RedirectToAction(
            nameof(Village),
            new { id, section = "Settings" });
    }


    [HttpGet]
    public async Task<IActionResult> CreateVillage()
    {
        var userIdValue = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdValue, out int userId))
            return Forbid();

        bool hasVillage = await _context.Villages
            .AnyAsync(v => v.PlayerId == userId);

        if (hasVillage)
            return RedirectToAction(nameof(Index));

        return View(new CreateVillageViewModel());
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateVillage(
        CreateVillageViewModel model)
    {
        var userIdValue = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdValue, out int userId))
            return Forbid();

        bool hasVillage = await _context.Villages
            .AnyAsync(v => v.PlayerId == userId);

        if (hasVillage)
            return RedirectToAction(nameof(Index));

        model.Name = model.Name?.Trim() ?? string.Empty;

        if (model.Name.Length < 3 || model.Name.Length > 50)
        {
            ModelState.AddModelError(
                nameof(model.Name),
                "Nazwa musi mieć od 3 do 50 znaków");
        }

        if (!ModelState.IsValid)
            return View(model);

        for (int attempt = 0; attempt < 100; attempt++)
        {
            int x = Random.Shared.Next(0, 5001);
            int y = Random.Shared.Next(0, 5001);

            bool coordinatesTaken = await _context.Villages
                .AnyAsync(v => v.X == x && v.Y == y);

            if (coordinatesTaken)
                continue;

            var village = new Village
            {
                PlayerId = userId,
                Name = model.Name,
                X = x,
                Y = y,
                Points = 0,
                CreatedAt = DateTime.UtcNow
            };

            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                _context.Villages.Add(village);

                await _context.SaveChangesAsync();

                var resources = new VillageResource
                {
                    VillageId = village.VillageId,

                    WoodAmount = 1000,
                    ClayAmount = 1000,
                    IronAmount = 500,
                    FoodAmount = 1000,

                    WoodProduction = 100,
                    ClayProduction = 100,
                    IronProduction = 50,
                    FoodProduction = 100,

                    LastUpdatedAt = DateTime.UtcNow
                };

                _context.VillageResources.Add(resources);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return RedirectToAction(
                    nameof(Village),
                    new
                    {
                        id = village.VillageId,
                        section = "TownHall"
                    });
            }
            catch (DbUpdateException ex)
                when (ex.InnerException is SqlException sql &&
                      (sql.Number == 2601 || sql.Number == 2627))
            {
                await transaction.RollbackAsync();

                _context.Entry(village).State =
                    EntityState.Detached;

                continue;
            }
        }

        ModelState.AddModelError(
            string.Empty,
            "Mapa jest zapchana, do nastepnego sezonu");

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RecruitUnit(
        int villageId,
        int unitTypeId,
        int quantity)
    {
        var userIdValue = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdValue, out int userId))
            return Forbid();

        bool hasAccess = await _context.Villages
            .AnyAsync(v =>
                v.VillageId == villageId &&
                v.PlayerId == userId &&
                v.VillageBuildings.Any(vb =>
                    EF.Functions.Like(
                        vb.BuildingType.Name, "%Koszary%")));

        if (!hasAccess)
            return NotFound();

        if (quantity <= 0)
        {
            TempData["Error"] = "Podaj prawidłową liczbę jednostek.";

            return RedirectToAction(
                nameof(Village),
                new { id = villageId, section = "Barracks" });
        }

        try
        {
            await _context.Database.ExecuteSqlInterpolatedAsync(
                $"EXEC dbo.RecruitVillageUnits @VillageId={villageId}, @UnitTypeId={unitTypeId}, @Quantity={quantity}");

            TempData["Success"] = "Rekrutacja zakończona pomyślnie!";
        }
        catch (SqlException ex)
        {
            TempData["Error"] = ex.Number switch
            {
                50011 => "Nie można rekrutować takiej liczby jednostek.",
                50012 => "Wioska nie istnieje.",
                50013 => "Nie znaleziono takiej jednostki.",
                50014 => "Nie znaleziono zasobów wioski.",
                50015 => "Masz za mało surowców na rekrutację.",
                _ => "Wystąpił błąd podczas rekrutacji."
            };
        }

        return RedirectToAction(
            nameof(Village),
            new { id = villageId, section = "Barracks" });
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveUnit(
        int villageId,
        int unitTypeId)
    {
        var userIdValue = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdValue, out int userId))
            return Forbid();

        bool hasAccess = await _context.Villages
            .AnyAsync(v =>
                v.VillageId == villageId &&
                v.PlayerId == userId &&
                v.VillageBuildings.Any(vb =>
                    EF.Functions.Like(
                        vb.BuildingType.Name, "%Koszary%")));

        if (!hasAccess)
            return NotFound();

        var unit = await _context.VillageUnits
            .FirstOrDefaultAsync(u =>
                u.VillageId == villageId &&
                u.UnitTypeId == unitTypeId);

        if (unit == null)
        {
            TempData["Error"] = "Nie znaleziono jednostki.";
        }
        else
        {
            _context.VillageUnits.Remove(unit);

            await _context.SaveChangesAsync();

            TempData["Success"] = "Jednostka została usunięta.";
        }

        return RedirectToAction(
            nameof(Village),
            new { id = villageId, section = "Barracks" });
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpgradeBuilding(
        int villageId,
        int buildingTypeId)
    {
        var userIdValue = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdValue, out int userId))
            return Forbid();

        bool hasAccess = await _context.Villages
            .AnyAsync(v =>
                v.VillageId == villageId &&
                v.PlayerId == userId);

        if (!hasAccess)
            return NotFound();

        bool buildingExists = await _context.BuildingTypes
            .AnyAsync(b => b.BuildingTypeId == buildingTypeId);

        if (!buildingExists)
        {
            TempData["Error"] = "Nie znaleziono takiego budynku.";

            return RedirectToAction(
                nameof(Village),
                new { id = villageId, section = "BuildPlan" });
        }

        try
        {
            await _context.Database.ExecuteSqlInterpolatedAsync(
                $"EXEC dbo.UpgradeVillageBuilding @VillageId={villageId}, @BuildingTypeId={buildingTypeId}");

            TempData["Success"] =
                "Budowa lub ulepszenie budynku zakończone pomyślnie!";
        }
        catch (SqlException ex)
        {
            TempData["Error"] = ex.Number switch
            {
                50001 => "Wioska nie istnieje.",
                50002 => "Nie znaleziono takiego budynku.",
                50003 => "Nie znaleziono zasobów wioski.",
                50004 => "Masz za mało surowców na budowę lub ulepszenie.",
                _ => "Wystąpił błąd podczas budowania."
            };
        }

        return RedirectToAction(
            nameof(Village),
            new { id = villageId, section = "BuildPlan" });
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TransportResources(
        TransportResourcesViewModel model)
    {
        var userIdValue = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdValue, out int userId))
            return Forbid();

        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Podano nieprawidłowe ilości surowców.";

            return RedirectToAction(
                nameof(Village),
                new { id = model.SourceVillageId, section = "Map" });
        }

        if (model.SourceVillageId == model.DestinationVillageId)
        {
            TempData["Error"] = "Nie można transportować surowców do tej samej wioski.";

            return RedirectToAction(
                nameof(Village),
                new { id = model.SourceVillageId, section = "Map" });
        }

        if (model.Wood == 0 &&
            model.Clay == 0 &&
            model.Iron == 0 &&
            model.Food == 0)
        {
            TempData["Error"] = "Podaj ilość przynajmniej jednego surowca.";

            return RedirectToAction(
                nameof(Village),
                new { id = model.SourceVillageId, section = "Map" });
        }

        await using var transaction =
            await _context.Database.BeginTransactionAsync(
                IsolationLevel.Serializable);

        try
        {
            var sourceVillage = await _context.Villages
                .FirstOrDefaultAsync(v =>
                    v.VillageId == model.SourceVillageId &&
                    v.PlayerId == userId);

            if (sourceVillage == null)
                return NotFound();

            var destinationVillage = await _context.Villages
                .FirstOrDefaultAsync(v =>
                    v.VillageId == model.DestinationVillageId);

            if (destinationVillage == null)
            {
                await transaction.RollbackAsync();

                TempData["Error"] = "Nie znaleziono wioski docelowej.";

                return RedirectToAction(
                    nameof(Village),
                    new { id = model.SourceVillageId, section = "Map" });
            }

            var resources = await _context.VillageResources
                .Where(r =>
                    r.VillageId == model.SourceVillageId ||
                    r.VillageId == model.DestinationVillageId)
                .ToListAsync();

            var sourceResources = resources.FirstOrDefault(
                r => r.VillageId == model.SourceVillageId);

            var destinationResources = resources.FirstOrDefault(
                r => r.VillageId == model.DestinationVillageId);

            if (sourceResources == null ||
                destinationResources == null)
            {
                await transaction.RollbackAsync();

                TempData["Error"] = "Nie znaleziono zasobów jednej z wiosek.";

                return RedirectToAction(
                    nameof(Village),
                    new { id = model.SourceVillageId, section = "Map" });
            }

            if (sourceResources.WoodAmount < model.Wood ||
                sourceResources.ClayAmount < model.Clay ||
                sourceResources.IronAmount < model.Iron ||
                sourceResources.FoodAmount < model.Food)
            {
                await transaction.RollbackAsync();

                TempData["Error"] = "Nie posiadasz wystarczającej ilości surowców.";

                return RedirectToAction(
                    nameof(Village),
                    new { id = model.SourceVillageId, section = "Map" });
            }

            if ((long)destinationResources.WoodAmount + model.Wood > int.MaxValue ||
                (long)destinationResources.ClayAmount + model.Clay > int.MaxValue ||
                (long)destinationResources.IronAmount + model.Iron > int.MaxValue ||
                (long)destinationResources.FoodAmount + model.Food > int.MaxValue)
            {
                await transaction.RollbackAsync();

                TempData["Error"] = "Wioska docelowa nie może pomieścić takiej ilości surowców.";

                return RedirectToAction(
                    nameof(Village),
                    new { id = model.SourceVillageId, section = "Map" });
            }

            sourceResources.WoodAmount -= model.Wood;
            sourceResources.ClayAmount -= model.Clay;
            sourceResources.IronAmount -= model.Iron;
            sourceResources.FoodAmount -= model.Food;

            destinationResources.WoodAmount += model.Wood;
            destinationResources.ClayAmount += model.Clay;
            destinationResources.IronAmount += model.Iron;
            destinationResources.FoodAmount += model.Food;

            sourceResources.LastUpdatedAt = DateTime.UtcNow;
            destinationResources.LastUpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            TempData["Success"] =
                $"Transport do wioski {destinationVillage.Name} zakończony pomyślnie.";

            return RedirectToAction(
                nameof(Village),
                new { id = model.SourceVillageId, section = "Map" });
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }



}
