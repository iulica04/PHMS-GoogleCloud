using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NewStructureForMedicationModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "admins",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Gender = table.Column<string>(type: "TEXT", maxLength: 6, nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", maxLength: 15, nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_admins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "medics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Rank = table.Column<string>(type: "TEXT", nullable: false),
                    Specialization = table.Column<string>(type: "TEXT", nullable: false),
                    Hospital = table.Column<string>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Gender = table.Column<string>(type: "TEXT", maxLength: 6, nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", maxLength: 15, nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_medics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "password_reset_tokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Token = table.Column<string>(type: "TEXT", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_password_reset_tokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "patients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Gender = table.Column<string>(type: "TEXT", maxLength: 6, nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", maxLength: 15, nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_patients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "consultations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    patient_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    medic_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Location = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_consultations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_consultations_medics_medic_id",
                        column: x => x.medic_id,
                        principalTable: "medics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_consultations_patients_patient_id",
                        column: x => x.patient_id,
                        principalTable: "patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "medical_conditions",
                columns: table => new
                {
                    MedicalConditionId = table.Column<Guid>(type: "TEXT", nullable: false),
                    patient_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    start_date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    end_date = table.Column<DateTime>(type: "TEXT", nullable: true),
                    current_status = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    is_genetic = table.Column<bool>(type: "INTEGER", nullable: false),
                    recommendations = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_medical_conditions", x => x.MedicalConditionId);
                    table.ForeignKey(
                        name: "FK_medical_conditions_patients_patient_id",
                        column: x => x.patient_id,
                        principalTable: "patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "treatments",
                columns: table => new
                {
                    TreatmentId = table.Column<Guid>(type: "TEXT", nullable: false),
                    MedicalConditionId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Location = table.Column<string>(type: "TEXT", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Duration = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_treatments", x => x.TreatmentId);
                    table.ForeignKey(
                        name: "FK_treatments_medical_conditions_MedicalConditionId",
                        column: x => x.MedicalConditionId,
                        principalTable: "medical_conditions",
                        principalColumn: "MedicalConditionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "medications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TreatmentId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: false),
                    Dosage = table.Column<string>(type: "TEXT", nullable: false),
                    Administration = table.Column<string>(type: "TEXT", nullable: false),
                    Ingredients = table.Column<string>(type: "TEXT", nullable: false),
                    AdverseEffects = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_medications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_medications_treatments_TreatmentId",
                        column: x => x.TreatmentId,
                        principalTable: "treatments",
                        principalColumn: "TreatmentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "admins",
                columns: new[] { "Id", "Address", "BirthDate", "Email", "FirstName", "Gender", "LastName", "PasswordHash", "PhoneNumber" },
                values: new object[,]
                {
                    { new Guid("ba13f438-053a-44ba-bfe1-c52d45657cc1"), "Strada Libertatii nr.10, Iasi", new DateTime(2003, 7, 20, 0, 0, 0, 0, DateTimeKind.Utc), "admin2@gmail.com", "Admin2", "Male", "User", "$2a$11$aM9FGC/bVPxTxqUtq5slrOkbqwbbA3nw5ntiDAbgsFU2fuw8QCOxi", "0751234567" },
                    { new Guid("d0b47f36-741d-41de-a0e2-c3acbd2ee898"), "Piata Unirii nr.3, Iasi", new DateTime(2004, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), "admin1@gmail.com", "Admin1", "Female", "User", "$2a$11$pj1LMweP8m6rWtG/ItGK/eZ2xGRXjLjUVHF01I.BXledynoKBGDH.", "0757732675" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_consultations_medic_id",
                table: "consultations",
                column: "medic_id");

            migrationBuilder.CreateIndex(
                name: "IX_consultations_patient_id",
                table: "consultations",
                column: "patient_id");

            migrationBuilder.CreateIndex(
                name: "IX_medical_conditions_patient_id",
                table: "medical_conditions",
                column: "patient_id");

            migrationBuilder.CreateIndex(
                name: "IX_medications_TreatmentId",
                table: "medications",
                column: "TreatmentId");

            migrationBuilder.CreateIndex(
                name: "IX_treatments_MedicalConditionId",
                table: "treatments",
                column: "MedicalConditionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "admins");

            migrationBuilder.DropTable(
                name: "consultations");

            migrationBuilder.DropTable(
                name: "medications");

            migrationBuilder.DropTable(
                name: "password_reset_tokens");

            migrationBuilder.DropTable(
                name: "medics");

            migrationBuilder.DropTable(
                name: "treatments");

            migrationBuilder.DropTable(
                name: "medical_conditions");

            migrationBuilder.DropTable(
                name: "patients");
        }
    }
}
