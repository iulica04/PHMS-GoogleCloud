using Application.Use_Cases.Commands.ConsultationCommands;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Use_Cases.CommandHandlers.ConsultationCommandHandler
{
    public class UpdateConsultationCommandHandler : IRequestHandler<UpdateConsultationCommand, Result<Unit>>
    {
        private readonly IConsultationRepository repository;
        private readonly IGoogleCalendarRepository googleCalendarRepository;
        private readonly IMedicRepository medicRepository;
        private readonly IMapper mapper;

        public UpdateConsultationCommandHandler(IConsultationRepository repository, IMapper mapper, IGoogleCalendarRepository googleCalendarRepository, IMedicRepository medicRepository)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.googleCalendarRepository = googleCalendarRepository;
            this.medicRepository = medicRepository;
        }

        public async Task<Result<Unit>> Handle(UpdateConsultationCommand request, CancellationToken cancellationToken)
        {
            Console.WriteLine("[DEBUG] Received UpdateConsultationCommand for ID: " + request.Id);
            Console.WriteLine("[DEBUG] Request.Date from Frontend: " + request.Date + " (Kind: " + request.Date.Kind + ")");

            var consultation = await repository.GetByIdAsync(request.Id);
            if (consultation == null)
            {
                Console.WriteLine("[ERROR] Consultation not found.");
                return Result<Unit>.Failure("Consultation not found.");
            }

            bool statusChangedToAccepted = consultation.Status != ConsultationStatus.Accepted && request.Status == ConsultationStatus.Accepted;
            bool statusChangedFromAccepted = consultation.Status == ConsultationStatus.Accepted && request.Status != ConsultationStatus.Accepted;
            bool dateChanged = consultation.Date != request.Date;

            Console.WriteLine("[DEBUG] Current consultation date: " + consultation.Date + " (Kind: " + consultation.Date.Kind + ")");
            Console.WriteLine("[DEBUG] Status changed to accepted: " + statusChangedToAccepted);
            Console.WriteLine("[DEBUG] Status changed from accepted: " + statusChangedFromAccepted);
            Console.WriteLine("[DEBUG] Date changed: " + dateChanged);

            mapper.Map(request, consultation);

            var medic = await medicRepository.GetByIdAsync(consultation.MedicId);
            if (medic == null)
            {
                Console.WriteLine("[ERROR] Medic not found.");
                return Result<Unit>.Failure("Medic not found.");
            }

            DateTime newStartTime = DateTime.SpecifyKind(request.Date, DateTimeKind.Local).ToUniversalTime();
            DateTime newEndTime = newStartTime.AddHours(2);
            string description = $"Consultație susținută de {medic.FirstName} {medic.LastName}";

            Console.WriteLine("[DEBUG] New Start Time: " + newStartTime + " New End Time: " + newEndTime);

            if (statusChangedToAccepted)
            {
                bool newEventExists = await googleCalendarRepository.EventExistsAsync(newStartTime, newEndTime, description);
                Console.WriteLine("[DEBUG] New event exists: " + newEventExists);
                if (newEventExists)
                {
                    return Result<Unit>.Failure("An event already exists at this time with the same description.");
                }
                Console.WriteLine("[UpdateConsultation] Adding new event...");
                await googleCalendarRepository.AddEventAsync("Consultație pacient", "Clinica Medicală", description, newStartTime, newEndTime);
            }

            if (!statusChangedToAccepted && !statusChangedFromAccepted && dateChanged)
            {
                DateTime oldStartTime = DateTime.SpecifyKind(consultation.Date, DateTimeKind.Local).ToUniversalTime();
                DateTime oldEndTime = oldStartTime.AddHours(2);

                Console.WriteLine("[DEBUG] Checking if the new time slot is available...");
                bool newEventExists = await googleCalendarRepository.EventExistsAsync(newStartTime, newEndTime, description);
                if (newEventExists)
                {
                    return Result<Unit>.Failure("An event already exists at the new time slot.");
                }

                Console.WriteLine("[DEBUG] Deleting old event if it exists...");
                bool oldEventExists = await googleCalendarRepository.EventExistsAsync(oldStartTime, oldEndTime, description);
                if (oldEventExists)
                {
                    await googleCalendarRepository.DeleteEventAsync(oldStartTime, oldEndTime, description);
                }

                Console.WriteLine("[DEBUG] Adding updated event...");
                await googleCalendarRepository.AddEventAsync("Consultație pacient", "Clinica Medicală", description, newStartTime, newEndTime);
            }

            if (statusChangedFromAccepted)
            {
                DateTime oldStartTime = DateTime.SpecifyKind(consultation.Date, DateTimeKind.Local).ToUniversalTime();
                DateTime oldEndTime = oldStartTime.AddHours(2);

                Console.WriteLine("[DEBUG] Deleting event since status is no longer 'Accepted'...");
                bool oldEventExists = await googleCalendarRepository.EventExistsAsync(oldStartTime, oldEndTime, description);
                if (oldEventExists)
                {
                    await googleCalendarRepository.DeleteEventAsync(oldStartTime, oldEndTime, description);
                }
            }

            await repository.UpdateAsync(consultation);
            Console.WriteLine("[SUCCESS] Consultation updated successfully.");
            return Result<Unit>.Success(Unit.Value);
        }
    }
}