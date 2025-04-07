using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Consultation> Consultations { get; set; }
        public DbSet<MedicalCondition> MedicalConditions { get; set; }
        public DbSet<Treatment> Treatments { get; set; }
        public DbSet<Medic> Medics { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Medication> Medications { get; set; }
        public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PasswordResetToken>(entity =>
            {
                entity.ToTable("password_reset_tokens");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id)
                      .HasColumnName("id")
                      .ValueGeneratedOnAdd();
                entity.Property(prt => prt.Email).HasColumnName("email");
                entity.Property(prt => prt.Token).IsRequired().HasColumnName("token");
                entity.Property(prt => prt.ExpirationDate).IsRequired().HasColumnName("expirationdate");
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.ToTable("patients");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id)
                .HasColumnName("id")
                      .ValueGeneratedOnAdd();
                entity.Property(p => p.FirstName).IsRequired().HasMaxLength(30).HasColumnName("firstname");
                entity.Property(p => p.LastName).IsRequired().HasMaxLength(30).HasColumnName("lastname");
                entity.Property(p => p.BirthDate).IsRequired().HasColumnName("birthdate");
                entity.Property(p => p.Gender).IsRequired().HasMaxLength(6).HasColumnName("gender");
                entity.Property(p => p.Email).IsRequired().HasColumnName("email");
                entity.Property(p => p.PasswordHash).IsRequired().HasColumnName("passwordhash");
                entity.Property(p => p.PhoneNumber).IsRequired().HasMaxLength(15).HasColumnName("phonenumber");
                entity.Property(p => p.Address).IsRequired().HasColumnName("address");

                entity.HasMany(p => p.MedicalConditions)
                      .WithOne()
                      .HasForeignKey(p => p.PatientId);
            });

            modelBuilder.Entity<MedicalCondition>(entity =>
            {
                entity.ToTable("medical_conditions");
                entity.HasKey(mc => mc.MedicalConditionId);
                entity.Property(mc => mc.MedicalConditionId).HasColumnName("medicalconditionid")
                      .ValueGeneratedOnAdd();
                entity.Property(mc => mc.PatientId).HasColumnName("patient_id").IsRequired();
                entity.Property(mc => mc.Name).HasColumnName("name").HasMaxLength(50).IsRequired();
                entity.Property(mc => mc.StartDate).HasColumnName("start_date").IsRequired();
                entity.Property(mc => mc.EndDate).HasColumnName("end_date").IsRequired(false);
                entity.Property(mc => mc.CurrentStatus).HasColumnName("current_status").HasMaxLength(20).IsRequired();
                entity.Property(mc => mc.IsGenetic).HasColumnName("is_genetic").IsRequired();
                entity.Property(mc => mc.Description).HasColumnName("description").HasMaxLength(500).IsRequired(false);
                entity.Property(mc => mc.Recommendation).HasColumnName("recommendations").HasMaxLength(500).IsRequired(false);
                entity.HasMany(mc => mc.Treatments)
                     .WithOne()
                     .HasForeignKey(t => t.MedicalConditionId);
            });

            modelBuilder.Entity<Consultation>(entity =>
            {
                entity.ToTable("consultations");
                entity.HasKey(c => c.Id);

                entity.Property(c => c.Id).HasColumnName("id")
                      .ValueGeneratedOnAdd();

                entity.Property(c => c.Date).HasColumnName("date")
                      .IsRequired();

                entity.Property(c => c.Location).HasColumnName("location")
                      .IsRequired();

                entity.Property(c => c.Status).HasColumnName("status")
                      .IsRequired();

                entity.Property(mc => mc.PatientId).HasColumnName("patient_id").IsRequired();
                entity.Property(mc => mc.MedicId).HasColumnName("medic_id").IsRequired();

            });


            modelBuilder.Entity<Medic>(entity =>
            {
                entity.ToTable("medics");
                entity.HasKey(m => m.Id);
                entity.Property(m => m.Id)
                      .HasColumnName("id")
                      .ValueGeneratedOnAdd();
                entity.Property(m => m.Rank).IsRequired()
                .HasColumnName("rank");
                entity.Property(m => m.Specialization).IsRequired()
                .HasColumnName("specialization");
                entity.Property(m => m.Hospital).IsRequired().HasColumnName("hospital");
                entity.Property(p => p.FirstName).IsRequired().HasMaxLength(30).HasColumnName("firstname");
                entity.Property(p => p.LastName).IsRequired().HasMaxLength(30).HasColumnName("lastname");
                entity.Property(p => p.BirthDate).IsRequired().HasColumnName("birthdate");
                entity.Property(p => p.Gender).IsRequired().HasMaxLength(6).HasColumnName("gender");
                entity.Property(p => p.Email).IsRequired().HasColumnName("email");
                entity.Property(p => p.PasswordHash).IsRequired().HasColumnName("passwordhash");
                entity.Property(p => p.PhoneNumber).IsRequired().HasMaxLength(15).HasColumnName("phonenumber");
                entity.Property(p => p.Address).IsRequired().HasColumnName("address");
            });

            modelBuilder.Entity<Medication>(entity =>
            {
                entity.ToTable("medications");
                entity.HasKey(m => m.Id);
                entity.Property(m => m.Id).HasColumnName("medicationid")
                      .ValueGeneratedOnAdd();
                entity.Property(m => m.Name).IsRequired().HasColumnName("medicationname");
                entity.Property(m => m.Type)
                      .HasColumnName("type")
                      .HasConversion<string>()
                      .IsRequired();
                entity.Property(m => m.Dosage).IsRequired().HasColumnName("dosage");
                entity.Property(m => m.Ingredients).IsRequired().HasColumnName("ingredients");
                entity.Property(m => m.TreatmentId).HasColumnName("treatmentid").IsRequired();
                entity.Property(m => m.AdverseEffects).IsRequired().HasColumnName("adverse_effects");
                entity.Property(m => m.Administration).IsRequired().HasColumnName("administration");
                entity.HasOne<Treatment>()
                     .WithMany(t => t.Medications)
                     .HasForeignKey(m => m.TreatmentId)
                     .IsRequired();
            });


            modelBuilder.Entity<Treatment>(entity =>
            {
                entity.ToTable("treatments");
                entity.HasKey(t => t.TreatmentId);
                entity.Property(t => t.TreatmentId).HasColumnName("treatmentid").ValueGeneratedOnAdd();
                entity.Property(t => t.Type).HasColumnName("type")
                      .HasConversion<string>()
                      .IsRequired();
                entity.Property(t => t.Name).IsRequired().HasColumnName("name");
                entity.Property(t => t.Location).IsRequired().HasColumnName("location");
                entity.Property(t => t.StartDate).IsRequired().HasColumnName("startdate");
                entity.Property(t => t.Duration).IsRequired().HasColumnName("duration");
                entity.Property(t => t.MedicalConditionId).IsRequired().HasColumnName("medicalconditionid");
                
            });

            modelBuilder.Entity<Admin>(entity =>
            {
                entity.ToTable("admins");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id)
                .HasColumnName("id")
                      .ValueGeneratedOnAdd();
                entity.Property(p => p.FirstName).IsRequired().HasMaxLength(30).HasColumnName("firstname");
                entity.Property(p => p.LastName).IsRequired().HasMaxLength(30).HasColumnName("lastname");
                entity.Property(p => p.BirthDate).IsRequired().HasColumnName("birthdate");
                entity.Property(p => p.Gender).IsRequired().HasMaxLength(6).HasColumnName("gender");
                entity.Property(p => p.Email).IsRequired().HasColumnName("email");
                entity.Property(p => p.PasswordHash).IsRequired().HasColumnName("passwordhash");
                entity.Property(p => p.PhoneNumber).IsRequired().HasMaxLength(15).HasColumnName("phonenumber");
                entity.Property(p => p.Address).IsRequired().HasColumnName("address");

                // Seeder pentru un administrator predefinit
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword("parola123");
                string hashedPassword2 = BCrypt.Net.BCrypt.HashPassword("parola456");
                var birthDate = new DateTime(2004, 2, 15, 0, 0, 0, DateTimeKind.Utc);
                var birthDate2 = new DateTime(2003, 7, 20, 0, 0, 0, DateTimeKind.Utc);
                entity.HasData(
                    new Admin
                    {
                        Id = Guid.NewGuid(),
                        FirstName = "Admin1",
                        LastName = "User",
                        BirthDate = birthDate,
                        Gender = "Female",
                        Email = "admin1@gmail.com",
                        PasswordHash = hashedPassword,
                        PhoneNumber = "0757732675",
                        Address = "Piata Unirii nr.3, Iasi"
                    },
                    new Admin
                    {
                        Id = Guid.NewGuid(),
                        FirstName = "Admin2",
                        LastName = "User",
                        BirthDate = birthDate2,
                        Gender = "Male",
                        Email = "admin2@gmail.com",
                        PasswordHash = hashedPassword2,
                        PhoneNumber = "0751234567",
                        Address = "Strada Libertatii nr.10, Iasi"
                    }
                );
            });
        }
    }
}