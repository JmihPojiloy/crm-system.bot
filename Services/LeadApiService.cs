using System.Net.Http.Json;
using bot.Models;

namespace bot.Services
{
    public class LeadApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string? _apiBaseAddress;

        public LeadApiService(string apiBaseAddress)
        {
            _httpClient = new HttpClient();
            _apiBaseAddress = apiBaseAddress;
        }

        public async Task<bool> CreateLeadAsync(LeadCreateModel model)
        {
            var response = await _httpClient.PostAsJsonAsync(_apiBaseAddress, model);
            return response.IsSuccessStatusCode;
        }
    }
}