using IdentityPractice.Shared.Interfaces;
using IdentityPractice.Shared.Models;
using IdentityPractice.Web.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace IdentityPractice.Web.Services;

public class VacationService : IVacationService
{
    private readonly ApplicationDbContext _db;
    private readonly AuthenticationStateProvider _authState;

    public VacationService(ApplicationDbContext db, AuthenticationStateProvider authState)
    {
        _db = db;
        _authState = authState;
    }

    private async Task<string?> GetUserIdAsync()
    {
        var state = await _authState.GetAuthenticationStateAsync();
        return state.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }

    public async Task<List<VacationDTO>> GetVacationsAsync()
    {
        var userId = await GetUserIdAsync();
        if (userId is null) return new();

        return await _db.Vacations
            .Where(v => v.TravelerId == userId)
            .Select(v => new VacationDTO
            {
                Id = v.Id,
                Place = v.Place,
                StartDate = v.StartDate,
                EndDate = v.EndDate
            })
            .ToListAsync();
    }

    public async Task AddVacationAsync(VacationDTO vacation)
    {
        var userId = await GetUserIdAsync();
        if (userId is null) return;

        _db.Vacations.Add(new Vacation
        {
            Place = vacation.Place,
            StartDate = vacation.StartDate,
            EndDate = vacation.EndDate,
            TravelerId = userId
        });

        await _db.SaveChangesAsync();
    }

    public async Task DeleteVacationAsync(int id)
    {
        var userId = await GetUserIdAsync();
        if (userId is null) return;

        var vacation = await _db.Vacations
            .FirstOrDefaultAsync(v => v.Id == id && v.TravelerId == userId);

        if (vacation is null) return;

        _db.Vacations.Remove(vacation);
        await _db.SaveChangesAsync();
    }
}