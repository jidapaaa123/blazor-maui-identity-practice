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
            var response = await _http.GetAsync("api/vacations");

            if (!response.IsSuccessStatusCode)
                throw new Exception(await response.Content.ReadAsStringAsync());

            return await response.Content.ReadFromJsonAsync<List<VacationDTO>>() ?? new();
        }

        public async Task AddVacationAsync(VacationDTO vacation)
        {
            var response = await _http.PostAsJsonAsync("api/vacations", vacation);
            if (!response.IsSuccessStatusCode)
                throw new Exception(await response.Content.ReadAsStringAsync());
        }

        public async Task DeleteVacationAsync(int id)
        {
            await _http.DeleteAsync($"api/vacations/{id}");
        }
    }
}
