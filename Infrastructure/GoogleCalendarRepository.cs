using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using System;
using System.Linq;
using System.Threading.Tasks;
using Domain.Repositories;

namespace Infrastructure
{
    public class GoogleCalendarRepository : IGoogleCalendarRepository
    {
        private readonly CalendarService _calendarService;
        private const string CalendarId = "predictsmarthealth@gmail.com";

        public GoogleCalendarRepository(GoogleCalendarService googleCalendarService)
        {
            _calendarService = googleCalendarService.GetCalendarService();
        }

        // Adaugă un eveniment în Google Calendar
        public async Task<Event> AddEventAsync(string summary, string location, string description, DateTime startTime, DateTime endTime)
        {
            // Verificăm dacă există deja un eveniment la aceleași date
            Console.Write("[GoogleCalendarRepository] START TIME : " + startTime + " END TIME: " + endTime);
            bool eventExists = await EventExistsAsync(startTime, endTime, description);
            if (eventExists)
            {
                throw new InvalidOperationException("An event already exists at the given time slot.");
            }

            // Creăm un nou eveniment
            Event newEvent = new Event()
            {
                Summary = summary,
                Location = location,
                Description = description,
                Start = new EventDateTime()
                {
                    DateTime = startTime.ToUniversalTime(),
                    TimeZone = "UTC",
                },
                End = new EventDateTime()
                {
                    DateTime = endTime.ToUniversalTime(),
                    TimeZone = "UTC",
                }
            };

            // Adăugăm evenimentul în calendarul Google
            var request = _calendarService.Events.Insert(newEvent, CalendarId);
            return await Task.Run(() => request.Execute());
        }

        // Verifică dacă există un eveniment în calendar la aceleași date și descriere
        public async Task<bool> EventExistsAsync(DateTime startTime, DateTime endTime, string description)
        {
            var request = _calendarService.Events.List(CalendarId);
            request.TimeMin = startTime.ToUniversalTime().AddMinutes(-5);
            request.TimeMax = endTime.ToUniversalTime().AddMinutes(5);
            request.ShowDeleted = false;
            request.SingleEvents = true;

            var events = await Task.Run(() => request.Execute().Items);

            if (events == null)
            {
                Console.WriteLine("[EventExistsAsync] No events found.");
                return false;
            }

            Console.WriteLine("[EventExistsAsync] Checking events...");
            foreach (var e in events)
            {
                Console.WriteLine($"[EventExistsAsync] Found event: {e.Summary}, {e.Description}, " +
                                  $"Start: {e.Start.DateTime}, End: {e.End.DateTime}");
            }

            return events.Any(e =>
                e.Description != null &&
                e.Description.Equals(description, StringComparison.OrdinalIgnoreCase) &&
                e.Start.DateTime.HasValue &&
                e.End.DateTime.HasValue &&
                e.Start.DateTime.Value == startTime.ToUniversalTime() &&
                e.End.DateTime.Value == endTime.ToUniversalTime()
            );
        }

        // Șterge un eveniment din Google Calendar
        public async Task DeleteEventAsync(DateTime startTime, DateTime endTime, string description)
        {
            Console.WriteLine($"[GoogleCalendarRepository] Attempting to delete event: {description} between {startTime} and {endTime}");

            var request = _calendarService.Events.List(CalendarId);
            request.TimeMin = startTime.ToUniversalTime().AddMinutes(-5); // Extindem intervalul pentru siguranță
            request.TimeMax = endTime.ToUniversalTime().AddMinutes(5);
            request.ShowDeleted = false;
            request.SingleEvents = true;

            var events = await request.ExecuteAsync();

            if (events.Items == null || !events.Items.Any())
            {
                Console.WriteLine($"[GoogleCalendarRepository] No events found for deletion.");
                return;
            }

            foreach (var ev in events.Items)
            {
                Console.WriteLine($"[GoogleCalendarRepository] Checking event: {ev.Summary}, {ev.Description}, {ev.Start.DateTime} - {ev.End.DateTime}");

                if (ev.Description != null && ev.Description.Contains(description, StringComparison.OrdinalIgnoreCase) &&
                    ev.Start.DateTime.HasValue && ev.End.DateTime.HasValue &&
                    ev.Start.DateTime.Value >= startTime.ToUniversalTime().AddMinutes(-5) &&
                    ev.End.DateTime.Value <= endTime.ToUniversalTime().AddMinutes(5))
                {
                    Console.WriteLine($"[GoogleCalendarRepository] Deleting event with ID: {ev.Id}");
                    await _calendarService.Events.Delete(CalendarId, ev.Id).ExecuteAsync();
                    Console.WriteLine($"[GoogleCalendarRepository] Event {ev.Id} deleted successfully.");
                }
            }
        }

        // Actualizează un eveniment în Google Calendar
        public async Task UpdateEventAsync(string oldDescription, DateTime oldStartTime, DateTime oldEndTime, string newSummary, string newLocation, string newDescription, DateTime newStartTime, DateTime newEndTime)
        {
            // Ștergem evenimentul vechi
            await DeleteEventAsync(oldStartTime, oldEndTime, oldDescription);

            // Adăugăm noul eveniment
            await AddEventAsync(newSummary, newLocation, newDescription, newStartTime, newEndTime);
        }
    }
}