using IdentityPractice.Shared.Models;
using IdentityPractice.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

public static class VacationEndpoints
{
    public static void MapVacationEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("api/vacations")
                       .RequireAuthorization(); // rejects requests with no valid token

        group.MapGet("/", GetVacations);
        group.MapPost("/", AddVacation);
        group.MapDelete("/{id:int}", DeleteVacation);
    }

    static async Task<IResult> GetVacations(
        ClaimsPrincipal user,
        ApplicationDbContext db)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null) return Results.Unauthorized();

        var vacations = await db.Vacations
            .Where(v => v.TravelerId == userId)
            .Select(v => new VacationDTO
            {
                Id = v.Id,
                Place = v.Place,
                StartDate = v.StartDate,
                EndDate = v.EndDate
            })
            .ToListAsync();

        return Results.Ok(vacations);
    }

    static async Task<IResult> AddVacation(
        VacationDTO model,
        ClaimsPrincipal user,
        ApplicationDbContext db)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null) return Results.Unauthorized();

        var vacation = new Vacation
        {
            Place = model.Place,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            TravelerId = userId  // sourced from token, not client
        };

        db.Vacations.Add(vacation);
        await db.SaveChangesAsync();

        return Results.Ok();
    }

    static async Task<IResult> DeleteVacation(
        int id,
        ClaimsPrincipal user,
        ApplicationDbContext db)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null) return Results.Unauthorized();

        var vacation = await db.Vacations
            .FirstOrDefaultAsync(v => v.Id == id && v.TravelerId == userId);
        // the TravelerId check is important — prevents a user
        // from deleting someone else's vacation by guessing an ID

        if (vacation is null) return Results.NotFound();

        db.Vacations.Remove(vacation);
        await db.SaveChangesAsync();

        return Results.Ok();
    }
}