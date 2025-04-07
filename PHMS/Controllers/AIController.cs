using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class MedicalSpecializationController : ControllerBase
{
    private readonly HttpClient _httpClient;
    // Setează aici URL-ul funcției din Cloud Functions
    private readonly string _cloudFunctionUrl = "https://mail-27694350835.europe-west3.run.app";

    public MedicalSpecializationController(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        // Dacă ai nevoie să folosești API Key-ul din appsettings sau variabile de mediu,
        // îl poți obține aici, însă pentru Cloud Function nu e necesar,
        // dacă autentificarea se face altfel.
    }

    [HttpGet("{specialization}")]
    public async Task<IActionResult> GetSpecializationDescription(string specialization)
    {
        if (string.IsNullOrWhiteSpace(specialization))
        {
            return BadRequest("Specializarea nu poate fi goală.");
        }

        // Construiește URL-ul pentru a apela funcția, adăugând parametrul "specialization"
        var requestUrl = $"{_cloudFunctionUrl}?specialization={specialization}";

        // Apelăm funcția cu o cerere POST (poți folosi și GET dacă funcția o acceptă)
        HttpResponseMessage response = await _httpClient.PostAsync(requestUrl, new StringContent("", Encoding.UTF8, "application/json"));
        string responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            return StatusCode((int)response.StatusCode, $"Eroare la apelul funcției: {responseContent}");
        }

        using JsonDocument doc = JsonDocument.Parse(responseContent);

        // Verificăm dacă răspunsul conține proprietatea "description"
        if (!doc.RootElement.TryGetProperty("description", out JsonElement descriptionElement) ||
            string.IsNullOrWhiteSpace(descriptionElement.GetString()))
        {
            return StatusCode(500, "Nu a fost returnată nicio descriere.");
        }

        string description = descriptionElement.GetString();

        // Returnează răspunsul către client
        return Ok(new { specialization, description });
    }
}