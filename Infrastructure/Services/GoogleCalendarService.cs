using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using System.IO;

public class GoogleCalendarService
{
    private static string ApplicationName = "Clinica Calendar";

    public CalendarService GetCalendarService()
    {
        GoogleCredential credential;
        using (var stream = new FileStream("service-account.json", FileMode.Open, FileAccess.Read))
        {
            credential = GoogleCredential.FromStream(stream)
                .CreateScoped(CalendarService.Scope.Calendar);
        }

        return new CalendarService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = ApplicationName,
        });
    }
}
