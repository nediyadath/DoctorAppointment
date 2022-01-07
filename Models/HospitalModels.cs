using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DoctorAppointment.Models
{
    public class Hospital
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Doctor
    {
        public int id { get; set; }
        public string name { get; set; }
        public string speciality { get; set; }
        [ForeignKey("hosp")]
        public int hospID { get; set; }
        public Hospital hosp { get; set; }
        public string Slots { get; set; }
    }

    public class Patient
    {
        public int id { get; set; }
        public string name { get; set; }
        public string mob { get; set; }

    }

    public class Appointment
    {
        public int id { get; set; }
        [ForeignKey("pat")]
        public int ptid { get; set; }
        public Patient pat { get; set; }
        [ForeignKey("doc")]
        public int docid { get; set; }
        public Doctor doc { get; set; }
        public DateTime appDate { get; set; }
        public string appSlot { get; set; }

    }

    public class HospitalContext : DbContext
    {
        public DbSet<Patient> patient { get; set; }
        public DbSet<Doctor> doctror { get; set; }
        public DbSet<Hospital> hospital { get; set; }
        public DbSet<Appointment> appointment { get; set; }
    }
}