using Application.DTOs;
using Domain.Entities;
using Domain.Services;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class MedicationExternalService : IMedicationExternalService
    {
        private readonly HttpClient _httpClient;
        private readonly string? _apiKey;
        private readonly string? _baseUrl;

        public MedicationExternalService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["ExternalApi:ApiKey"];
            _baseUrl = configuration["ExternalApi:BaseUrl"];
        }

        public async Task<List<ExternalMedication>> GetMedicationsByConditionAsync(string condition)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUrl}/medications/disease/{condition}");
            request.Headers.Add("Authorization", $"Bearer {_apiKey}");

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return new List<ExternalMedication>();
            }

            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Raw API Response:\n" + content);

            var medications2 = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(content);
            foreach (var med in medications2)
            {
                Console.WriteLine($"Med Name: {med["name"]}, AdverseEffects: {med["adverse_effects"]}");
            }

            var medications = JsonSerializer.Deserialize<List<ExternalMedication>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            }) ?? new List<ExternalMedication>();

            // Afișează lista de medicamente într-un format clar
            Console.WriteLine("Deserialized Medications:\n" + JsonSerializer.Serialize(medications, new JsonSerializerOptions { WriteIndented = true }));

            return medications;
        }
    }
}