using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BusinessObjects.Models;

public partial class FootballFieldManagerContext : DbContext
{
    public FootballFieldManagerContext()
    {
    }

    public FootballFieldManagerContext(DbContextOptions<FootballFieldManagerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<BookingTime> BookingTimes { get; set; }

    public virtual DbSet<FeedbackPitch> FeedbackPitches { get; set; }

    public virtual DbSet<InvoicePitch> InvoicePitches { get; set; }

    public virtual DbSet<Pitch> Pitches { get; set; }

    public virtual DbSet<PricePitch> PricePitches { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<TotalInvoicePitch> TotalInvoicePitches { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer(" Data Source=localhost;Database=FootballFieldManager;User Id=sa;Password=sa;TrustServerCertificate=true;Trusted_Connection=SSPI;Encrypt=false;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Account__CB9A1CDF54EA3A4F");

            entity.ToTable("Account");

            entity.Property(e => e.UserId).HasColumnName("userID");
            entity.Property(e => e.Avata)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("avata");
            entity.Property(e => e.DateOfBirth)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("dateOfBirth");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Gender)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("gender");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Otp)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("otp");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("phoneNumber");
            entity.Property(e => e.RoleId).HasColumnName("roleID");
            entity.Property(e => e.StatusOtp).HasColumnName("statusOtp");
            entity.Property(e => e.TimeEffective)
                .HasColumnType("datetime")
                .HasColumnName("timeEffective");
            entity.Property(e => e.UserName)
                .HasMaxLength(255)
                .HasColumnName("userName");

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__Account__roleID__44FF419A");
        });

        modelBuilder.Entity<BookingTime>(entity =>
        {
            entity.HasKey(e => e.BookingTimeId).HasName("PK__BookingT__6C38743E2A178358");

            entity.ToTable("BookingTime");

            entity.Property(e => e.BookingTimeId).HasColumnName("bookingTimeID");
            entity.Property(e => e.Time)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("time");
        });

        modelBuilder.Entity<FeedbackPitch>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("PK__Feedback__2613FDC4B6E84662");

            entity.ToTable("FeedbackPitch");

            entity.Property(e => e.FeedbackId).HasColumnName("feedbackID");
            entity.Property(e => e.Content)
                .HasMaxLength(1000)
                .HasColumnName("content");
            entity.Property(e => e.PitchId)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("pitchID");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.TimeFeedback).HasColumnName("timeFeedback");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasOne(d => d.Pitch).WithMany(p => p.FeedbackPitches)
                .HasForeignKey(d => d.PitchId)
                .HasConstraintName("FK__FeedbackP__pitch__45F365D3");

            entity.HasOne(d => d.User).WithMany(p => p.FeedbackPitches)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__FeedbackP__userI__46E78A0C");
        });

        modelBuilder.Entity<InvoicePitch>(entity =>
        {
            entity.HasKey(e => e.InvoicePitchId).HasName("PK__InvoiceP__0EEBECD3BB4819B2");

            entity.ToTable("InvoicePitch");

            entity.Property(e => e.InvoicePitchId).HasColumnName("invoicePitchID");
            entity.Property(e => e.BookingTimeId).HasColumnName("bookingTimeID");
            entity.Property(e => e.PitchId)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("pitchID");
            entity.Property(e => e.PricePitchId).HasColumnName("pricePitchID");
            entity.Property(e => e.TotalInvoiceId).HasColumnName("totalInvoiceID");

            entity.HasOne(d => d.BookingTime).WithMany(p => p.InvoicePitches)
                .HasForeignKey(d => d.BookingTimeId)
                .HasConstraintName("FK__InvoicePi__booki__47DBAE45");

            entity.HasOne(d => d.Pitch).WithMany(p => p.InvoicePitches)
                .HasForeignKey(d => d.PitchId)
                .HasConstraintName("FK__InvoicePi__pitch__48CFD27E");

            entity.HasOne(d => d.PricePitch).WithMany(p => p.InvoicePitches)
                .HasForeignKey(d => d.PricePitchId)
                .HasConstraintName("FK__InvoicePi__price__49C3F6B7");

            entity.HasOne(d => d.TotalInvoice).WithMany(p => p.InvoicePitches)
                .HasForeignKey(d => d.TotalInvoiceId)
                .HasConstraintName("FK__InvoicePi__total__4AB81AF0");
        });

        modelBuilder.Entity<Pitch>(entity =>
        {
            entity.HasKey(e => e.PitchId).HasName("PK__Pitch__60EF0906132B07E6");

            entity.ToTable("Pitch");

            entity.Property(e => e.PitchId)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("pitchID");
            entity.Property(e => e.Image)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("image");
            entity.Property(e => e.PitchType).HasColumnName("pitchType");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<PricePitch>(entity =>
        {
            entity.HasKey(e => e.PricePitchId).HasName("PK__PricePit__B68AF7DDA6FFDCFC");

            entity.ToTable("PricePitch");

            entity.Property(e => e.PricePitchId).HasColumnName("pricePitchID");
            entity.Property(e => e.PitchId)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("pitchID");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.TimeEnd)
                .HasColumnType("datetime")
                .HasColumnName("timeEnd");
            entity.Property(e => e.TimeStart)
                .HasColumnType("datetime")
                .HasColumnName("timeStart");

            entity.HasOne(d => d.Pitch).WithMany(p => p.PricePitches)
                .HasForeignKey(d => d.PitchId)
                .HasConstraintName("FK__PricePitc__pitch__4BAC3F29");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Role__CD98460A6B60EEAA");

            entity.ToTable("Role");

            entity.Property(e => e.RoleId)
                .ValueGeneratedNever()
                .HasColumnName("roleID");
            entity.Property(e => e.RoleName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("roleName");
        });

        modelBuilder.Entity<TotalInvoicePitch>(entity =>
        {
            entity.HasKey(e => e.TotalInvoiceId).HasName("PK__TotalInv__8C8B68D51B6BE50E");

            entity.ToTable("TotalInvoicePitch");

            entity.Property(e => e.TotalInvoiceId).HasColumnName("totalInvoiceID");
            entity.Property(e => e.BookTime).HasColumnName("bookTime");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasOne(d => d.User).WithMany(p => p.TotalInvoicePitches)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__TotalInvo__userI__4CA06362");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
