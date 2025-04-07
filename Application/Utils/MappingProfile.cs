using Application.Commands.Administrator;
using Application.Commands.Medic;
using Application.Commands.MedicalConditionCommands;
using Application.Commands.MedicationCommand;
using Application.Commands.Patient;
using Application.Commands.TreatmentCommands;
using Application.DTOs;
using Application.Use_Cases.Commands.ConsultationCommands;
using AutoMapper;
using Domain.Entities;

namespace Application.Utils
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Patient, PatientDto>().ReverseMap();
            CreateMap<CreatePatientCommand, Patient>().ReverseMap();
            CreateMap<Medic, MedicDto>().ReverseMap();
            CreateMap<CreateMedicCommand, Medic>().ReverseMap();
            CreateMap<UpdateMedicCommand, Medic>().ReverseMap();
            CreateMap<UpdatePatientCommand, Patient>().ReverseMap();
            CreateMap<Admin, AdminDto>().ReverseMap();
            CreateMap<UpdateAdminCommand, Admin>().ReverseMap();
            CreateMap<MedicalCondition, MedicalConditionDto>().ReverseMap();
            CreateMap<CreateMedicalConditionCommand, MedicalCondition>().ReverseMap();
            CreateMap<UpdateMedicalConditionCommand, MedicalCondition>().ReverseMap();
            CreateMap<Treatment, TreatmentDto>().ReverseMap();
            CreateMap<CreateTreatmentCommand, Treatment>().ReverseMap();
            CreateMap<UpdateTreatmentCommand, Treatment>().ReverseMap();
            CreateMap<Medication, MedicationDto>().ReverseMap();
            CreateMap<CreateMedicationCommand, Medication>().ReverseMap();
            CreateMap<UpdateMedicationCommand, Medication>().ReverseMap();
            CreateMap<Consultation, ConsultationDto>().ReverseMap();
            CreateMap<CreateConsultationCommand, Consultation>().ReverseMap().ForMember(dest => dest.Status, opt => opt.MapFrom(src => ConsultationStatus.Pending));
            CreateMap<UpdateConsultationCommand, Consultation>().ReverseMap();
            CreateMap<ExternalMedication, ExternalMedicationDto>().ReverseMap();

            // CreateMap<UpdatePatientRecordCommand, PatientRecord>().ReverseMap();
        }
    }
}
