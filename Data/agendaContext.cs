using agendaEFD.Models;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace agendaEFD.Data
{
    public partial class agendaContext : DbContext
    {
        public agendaContext()
        {
        }

        public agendaContext(DbContextOptions<agendaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Appointment> Appointments { get; set; } = null!;
        public virtual DbSet<Broker> Brokers { get; set; } = null!;
        public virtual DbSet<Customer> Customers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

               
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-6IV8GIO\\SQLEXPRESS;Database=agenda;Trusted_Connection=True;MultipleActiveResultSets=True;");
            
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.HasKey(e => e.IdAppointment)
                    .HasName("appointments_PK");

                entity.ToTable("appointments");

                entity.Property(e => e.IdAppointment).HasColumnName("idAppointment");

                entity.Property(e => e.DateHour)
                    .HasColumnType("datetime")
                    .HasColumnName("dateHour");

                entity.Property(e => e.IdBroker).HasColumnName("idBroker");

                entity.Property(e => e.IdCustomer).HasColumnName("idCustomer");

                entity.Property(e => e.Subject)
                    .HasColumnType("text")
                    .HasColumnName("subject");

                entity.HasOne(d => d.IdBrokerNavigation)
                    .WithMany(p => p.Appointments)
                    .HasForeignKey(d => d.IdBroker)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("appointments_brokers0_FK");

                entity.HasOne(d => d.IdCustomerNavigation)
                    .WithMany(p => p.Appointments)
                    .HasForeignKey(d => d.IdCustomer)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("appointments_customers_FK");
            });

            modelBuilder.Entity<Broker>(entity =>
            {
                entity.HasKey(e => e.IdBroker)
                    .HasName("brokers_PK");

                entity.ToTable("brokers");

                entity.Property(e => e.IdBroker).HasColumnName("idBroker");

                entity.Property(e => e.Firstname)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("firstname");

                entity.Property(e => e.Lastname)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("lastname");

                entity.Property(e => e.Mail)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("mail");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("phoneNumber");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.IdCustomer)
                    .HasName("customers_PK");

                entity.ToTable("customers");

                entity.Property(e => e.IdCustomer).HasColumnName("idCustomer");

                entity.Property(e => e.Budget).HasColumnName("budget");

                entity.Property(e => e.Firstname)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("firstname");

                entity.Property(e => e.Lastname)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("lastname");

                entity.Property(e => e.Mail)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("mail");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("phoneNumber");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        /*

        protected override void Seed(agendaContext context)
        {
            context.Brokers.ExecuteSqlCommand("DBCC CHECKIDENT('brokers',RESEED)");
        } */

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
