

using Microsoft.EntityFrameworkCore;
using TruDoseAPI.Models;
using TrudoseAdminPortalAPI.Models;
using System.Data;
using TrudoseAdminPortalAPI.Model;
using Admin.Models;


namespace TrudoseAdminPortalAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Account> accounts { get; set; }
        public DbSet<Clinic> clinics { get; set; }
        public DbSet<ClinicAdminContact> clinic_admin_contacts { get; set; }
        public DbSet<ClinicBillingContact> clinic_billing_contacts { get; set; }
        public DbSet<Device> devices { get; set; }
        public DbSet<FirmwareUpdate> firmware_updates { get; set; }

        public DbSet<ClinicUser> clinic_users { get; set; }


        public DbSet<ClinicNotification> clinic_notifications { get; set; }


        public DbSet<SystemSetting> system_settings { get; set; }

        public DbSet<TrudoseUsers> trudose_users { get; set; }

        public DbSet<Role> roles { get; set; }

        public DbSet<SymptomsMaster> symptoms_master { get; set; }
        public DbSet<SurveyMaster> surveys_master { get; set; }

        public DbSet<QuestionsMaster> questions_master { get; set; }

        public DbSet<PatientSurveyQuestionMap> survey_question_symptom_map { get; set; }
        public DbSet<SurveyClinicMap> survey_clinic_map { get; set; }






        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<SymptomsMaster>(entity =>
            {
                entity.ToTable("symptoms_master"); // Optional: Map to a specific table name
                entity.HasKey(e => e.id); // Define primary key


            });

            modelBuilder.Entity<SurveyMaster>(entity =>
            {
                entity.ToTable("surveys_master"); // Optional: Map to a specific table name
                entity.HasKey(e => e.id); // Define primary key


            });

            modelBuilder.Entity<QuestionsMaster>(entity =>
            {
                entity.ToTable("questions_master"); // Optional: Map to a specific table name
                entity.HasKey(e => e.id); // Define primary key


            });

            modelBuilder.Entity<PatientSurveyQuestionMap>(entity =>
            {
                entity.ToTable("survey_question_symptom_map"); // Optional: Map to a specific table name
                entity.HasKey(e => e.id); // Define primary key
                //entity.HasIndex(e => new { e.QuestionId, e.SurveyId }).IsUnique();

            });


            modelBuilder.Entity<SurveyClinicMap>(entity =>
            {
                entity.ToTable("survey_clinic_map"); // Optional: Map to a specific table name
                entity.HasKey(e => e.id); // Define primary key
                //entity.HasIndex(e => new { e.QuestionId, e.SurveyId }).IsUnique();

            });

        }
    }









   
}
