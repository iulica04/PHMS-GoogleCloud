using Google.Apis.Calendar.v3.Data;

namespace Domain.Repositories
{
    public interface IGoogleCalendarRepository
    {
        Task<Event> AddEventAsync(string summary, string location, string description, DateTime startTime, DateTime endTime);
        Task<bool> EventExistsAsync(DateTime startTime, DateTime endTimestring, string description);
        Task DeleteEventAsync(DateTime startTime, DateTime endTime, string description);
    }
}
