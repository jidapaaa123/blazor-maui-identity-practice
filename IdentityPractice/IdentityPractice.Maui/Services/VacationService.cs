using IdentityPractice.Shared.Interfaces;
using IdentityPractice.Shared.Models;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace IdentityPractice.Maui.Services
{
    public class VacationService : IVacationService
    {
        private readonly HttpClient _http;

        public VacationService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<VacationDTO>> GetVacationsAsync()
        {
            return await _http.GetFromJsonAsync<List<VacationDTO>>("api/vacations") ?? new();
        }

        public async Task AddVacationAsync(VacationDTO vacation)
        {
            await _http.PostAsJsonAsync("api/vacations", vacation);
        }

        public async Task DeleteVacationAsync(int id)
        {
            await _http.DeleteAsync($"api/vacations/{id}");
        }
    }
}
