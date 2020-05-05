namespace MedReg.Models
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class MedRegContext : DbContext
    {
        public MedRegContext()
            : base("name=Models.MedRegContext")
        {
            Database.SetInitializer( new MigrateDatabaseToLatestVersion<Models.MedRegContext, Migrations.Configuration>());
        }

        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<Visit> Visits { get; set; }
    }
}