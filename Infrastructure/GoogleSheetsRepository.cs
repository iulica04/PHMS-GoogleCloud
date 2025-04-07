using Application.DTOs;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Sheets.v4;
using Domain.Repositories;
using Google.Apis.Drive.v3;
using Domain.Entities;

namespace Infrastructure
{
    public class GoogleSheetsRepository : IGoogleSheetsRepository
    {
        private string _spreadsheetId = "1bCdYQptoRu5NgCQmLr1QgPYEdEkrgv470f15kzGQ8_4"; // ID-ul Google Sheet-ului
        private readonly SheetsService _sheetsService;
        private readonly DriveService _driveService;

        public GoogleSheetsRepository()
        {
            GoogleCredential credential;
            using (var stream = new FileStream("service-account.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                    .CreateScoped(new[] { SheetsService.Scope.Spreadsheets, DriveService.Scope.Drive });
            }

            _sheetsService = new SheetsService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "Pacienti Export"
            });

            _driveService = new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "Pacienti Export"
            });
        }

        public async Task<string> ExportPatientsToSheetAsync(List<Patient> patients)
        {
            try
            {
                // Crearea unui nou Spreadsheet
                var newSpreadsheet = new Spreadsheet
                {
                    Properties = new SpreadsheetProperties { Title = $"Pacienti-{DateTime.Now:yyyyMMddHHmmss}" }
                };

                var createdSpreadsheet = await _sheetsService.Spreadsheets.Create(newSpreadsheet).ExecuteAsync();
                _spreadsheetId = createdSpreadsheet.SpreadsheetId; // Salvăm noul ID al foii

                // Renunțăm la adăugarea unei foi suplimentare și redenumim foaia implicită
                string sheetName = "Pacienti"; // Numele dorit pentru foaie
                await RenameDefaultSheet(sheetName); // Redenumim foaia implicită

                // Scriem pacienții în foaia redenumită
                await WritePatientsToSheet(sheetName, patients);
                return await GetShareableLink();
            }
            catch (Google.GoogleApiException ex)
            {
                Console.WriteLine($"Eroare API Google: {ex.Message}");
                Console.WriteLine($"Detalii eroare: {ex.Error?.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eroare necunoscută: {ex.Message}");
                throw;
            }
        }

        private async Task RenameDefaultSheet(string newSheetName)
        {
            // Obținem lista de foi din spreadsheet
            var sheets = await _sheetsService.Spreadsheets.Get(_spreadsheetId).ExecuteAsync();

            // Găsim foaia implicită (de obicei este prima foaie)
            var sheet = sheets.Sheets.FirstOrDefault();
            if (sheet != null)
            {
                // Modificăm titlul foii
                var updateRequest = new Request
                {
                    UpdateSheetProperties = new UpdateSheetPropertiesRequest
                    {
                        Properties = new SheetProperties
                        {
                            Title = newSheetName
                        },
                        Fields = "title"
                    }
                };

                // Trimitem cererea de actualizare
                var batchUpdateRequest = new BatchUpdateSpreadsheetRequest
                {
                    Requests = new List<Request> { updateRequest }
                };
                await _sheetsService.Spreadsheets.BatchUpdate(batchUpdateRequest, _spreadsheetId).ExecuteAsync();
            }
        }

        private async Task WritePatientsToSheet(string sheetName, List<Patient> patients)
        {
            var values = new List<IList<object>> { new List<object> {  "FirstName", "LastName", "Birthdate", "Gender", "Email", "PhoneNumber", "Diseases" } };

            foreach (var patient in patients)
            {
                var medicalConditions = string.Join(", ", patient.MedicalConditions.Select(mc => mc.Name));
                values.Add(new List<object> {  patient.FirstName, patient.LastName, patient.BirthDate,patient.Gender, patient.Email, patient.PhoneNumber, medicalConditions });
            }

            var valueRange = new ValueRange { Values = values };
            string range = $"{sheetName}!A1";
            var updateRequest = _sheetsService.Spreadsheets.Values.Update(valueRange, _spreadsheetId, range);
            updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
            await updateRequest.ExecuteAsync();
        }

        private async Task<string> GetShareableLink()
        {
            // Creează permisiuni pentru acces public
            var permission = new Google.Apis.Drive.v3.Data.Permission
            {
                Type = "anyone",
                Role = "writer"
            };

            await _driveService.Permissions.Create(permission, _spreadsheetId).ExecuteAsync();
            return $"https://docs.google.com/spreadsheets/d/{_spreadsheetId}";
        }
    }
}
