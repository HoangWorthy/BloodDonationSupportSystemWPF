using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DAL.Entities;

public partial class BloodDonationSupportSystemContext : DbContext
{
    public BloodDonationSupportSystemContext()
    {
    }

    public BloodDonationSupportSystemContext(DbContextOptions<BloodDonationSupportSystemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Blog> Blogs { get; set; }

    public virtual DbSet<BloodRequest> BloodRequests { get; set; }

    public virtual DbSet<BloodStock> BloodStocks { get; set; }

    public virtual DbSet<BloodUnit> BloodUnits { get; set; }

    public virtual DbSet<ComponentRequest> ComponentRequests { get; set; }

    public virtual DbSet<DonationEvent> DonationEvents { get; set; }

    public virtual DbSet<DonationTimeSlot> DonationTimeSlots { get; set; }

    public virtual DbSet<EventRegistration> EventRegistrations { get; set; }

    public virtual DbSet<Profile> Profiles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer(GetConnectionString());

    private string GetConnectionString()
    {
        IConfiguration config = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", true, true)
                    .Build();
        var strConn = config["ConnectionStrings:DefaultConnection"];

        return strConn;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__accounts__3213E83FC29D94A9");

            entity.ToTable("accounts");

            entity.HasIndex(e => e.Email, "UQ__accounts__AB6E6164D9944F24").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Avatar)
                .HasColumnType("text")
                .HasColumnName("avatar");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.ProfileId).HasColumnName("profile_id");
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .HasColumnName("role");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("ENABLE")
                .HasColumnName("status");

            entity.HasOne(d => d.Profile).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.ProfileId)
                .HasConstraintName("FK__accounts__profil__5070F446");
        });

        modelBuilder.Entity<Blog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__blogs__3213E83F07F2CD8C");

            entity.ToTable("blogs");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AuthorId).HasColumnName("author_id");
            entity.Property(e => e.Content)
                .HasColumnType("text")
                .HasColumnName("content");
            entity.Property(e => e.CreationDate).HasColumnName("creation_date");
            entity.Property(e => e.LastModifiedDate).HasColumnName("last_modified_date");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasColumnName("status");
            entity.Property(e => e.Thumbnail)
                .HasMaxLength(500)
                .HasColumnName("thumbnail");
            entity.Property(e => e.Title)
                .HasMaxLength(500)
                .HasColumnName("title");

            entity.HasOne(d => d.Author).WithMany(p => p.Blogs)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__blogs__author_id__5165187F");
        });

        modelBuilder.Entity<BloodRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__blood_re__3213E83F1C8C092F");

            entity.ToTable("blood_requests");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BloodType)
                .HasMaxLength(20)
                .HasColumnName("blood_type");
            entity.Property(e => e.ComponentRequestId).HasColumnName("component_request_id");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_date");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasColumnName("status");

            entity.HasOne(d => d.ComponentRequest).WithMany(p => p.BloodRequests)
                .HasForeignKey(d => d.ComponentRequestId)
                .HasConstraintName("FK_blood_requests_component_request");
        });

        modelBuilder.Entity<BloodStock>(entity =>
        {
            entity.ToTable("blood_stock");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.BloodType)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("blood_type");
            entity.Property(e => e.ExpiredDate).HasColumnName("expired_date");
            entity.Property(e => e.Volume).HasColumnName("volume");
        });

        modelBuilder.Entity<BloodUnit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__blood_un__3213E83FBFF7DB79");

            entity.ToTable("blood_units");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BloodRequestId).HasColumnName("blood_request_id");
            entity.Property(e => e.BloodType)
                .HasMaxLength(20)
                .HasColumnName("blood_type");
            entity.Property(e => e.ComponentType)
                .HasMaxLength(20)
                .HasColumnName("component_type");
            entity.Property(e => e.DonorId).HasColumnName("donor_id");
            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.ProfileId).HasColumnName("profile_id");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasColumnName("status");
            entity.Property(e => e.Volume)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("volume");

            entity.HasOne(d => d.BloodRequest).WithMany(p => p.BloodUnits)
                .HasForeignKey(d => d.BloodRequestId)
                .HasConstraintName("FK__blood_uni__blood__52593CB8");

            entity.HasOne(d => d.Donor).WithMany(p => p.BloodUnits)
                .HasForeignKey(d => d.DonorId)
                .HasConstraintName("FK__blood_uni__donor__534D60F1");

            entity.HasOne(d => d.Event).WithMany(p => p.BloodUnits)
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("FK__blood_uni__event__5441852A");

            entity.HasOne(d => d.Profile).WithMany(p => p.BloodUnits)
                .HasForeignKey(d => d.ProfileId)
                .HasConstraintName("FK__blood_uni__profi__5535A963");
        });

        modelBuilder.Entity<ComponentRequest>(entity =>
        {
            entity.ToTable("component_request");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.ComponentType)
                .HasMaxLength(20)
                .IsFixedLength()
                .HasColumnName("component_type");
            entity.Property(e => e.ExpiredDate).HasColumnName("expired_Date");
            entity.Property(e => e.Volume).HasColumnName("volume");
        });

        modelBuilder.Entity<DonationEvent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__donation__3213E83F93185AC1");

            entity.ToTable("donation_events");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(500)
                .HasColumnName("address");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .HasColumnName("city");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(CONVERT([date],getdate()))")
                .HasColumnName("created_date");
            entity.Property(e => e.District)
                .HasMaxLength(100)
                .HasColumnName("district");
            entity.Property(e => e.DonationDate).HasColumnName("donation_date");
            entity.Property(e => e.DonationType)
                .HasMaxLength(20)
                .HasColumnName("donation_type");
            entity.Property(e => e.Hospital)
                .HasMaxLength(255)
                .HasColumnName("hospital");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.RegisteredMemberCount).HasColumnName("registered_member_count");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasColumnName("status");
            entity.Property(e => e.TotalMemberCount).HasColumnName("total_member_count");
            entity.Property(e => e.Ward)
                .HasMaxLength(100)
                .HasColumnName("ward");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.DonationEvents)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__donation___creat__5629CD9C");
        });

        modelBuilder.Entity<DonationTimeSlot>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__donation__3213E83F34434A95");

            entity.ToTable("donation_time_slots");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CurrentRegistrations).HasColumnName("current_registrations");
            entity.Property(e => e.EndTime).HasColumnName("end_time");
            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.MaxCapacity).HasColumnName("max_capacity");
            entity.Property(e => e.StartTime).HasColumnName("start_time");

            entity.HasOne(d => d.Event).WithMany(p => p.DonationTimeSlots)
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("FK__donation___event__571DF1D5");
        });

        modelBuilder.Entity<EventRegistration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__event_re__3213E83F8311C77B");

            entity.ToTable("event_registrations");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.BloodType)
                .HasMaxLength(20)
                .HasColumnName("blood_type");
            entity.Property(e => e.DonationType)
                .HasMaxLength(20)
                .HasColumnName("donation_type");
            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.ProfileId).HasColumnName("profile_id");
            entity.Property(e => e.RegistrationDate)
                .HasDefaultValueSql("(CONVERT([date],getdate()))")
                .HasColumnName("registration_date");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasColumnName("status");
            entity.Property(e => e.TimeSlotId).HasColumnName("time_slot_id");

            entity.HasOne(d => d.Account).WithMany(p => p.EventRegistrations)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__event_reg__accou__5812160E");

            entity.HasOne(d => d.Event).WithMany(p => p.EventRegistrations)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__event_reg__event__59063A47");

            entity.HasOne(d => d.Profile).WithMany(p => p.EventRegistrations)
                .HasForeignKey(d => d.ProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__event_reg__profi__59FA5E80");

            entity.HasOne(d => d.TimeSlot).WithMany(p => p.EventRegistrations)
                .HasForeignKey(d => d.TimeSlotId)
                .HasConstraintName("FK__event_reg__time___5AEE82B9");
        });

        modelBuilder.Entity<Profile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__profiles__3213E83FAA2C7ECF");

            entity.ToTable("profiles");

            entity.HasIndex(e => e.PersonalId, "UQ__profiles__C16BAC14061A8C72").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.Address)
                .HasMaxLength(500)
                .HasColumnName("address");
            entity.Property(e => e.BloodType)
                .HasMaxLength(20)
                .HasColumnName("blood_type");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .HasColumnName("city");
            entity.Property(e => e.DateOfBirth).HasColumnName("date_of_birth");
            entity.Property(e => e.District)
                .HasMaxLength(100)
                .HasColumnName("district");
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .HasColumnName("gender");
            entity.Property(e => e.LastDonationDate).HasColumnName("last_donation_date");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.NextEligibleDonationDate).HasColumnName("next_eligible_donation_date");
            entity.Property(e => e.PersonalId)
                .HasMaxLength(50)
                .HasColumnName("personal_id");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasColumnName("status");
            entity.Property(e => e.Ward)
                .HasMaxLength(100)
                .HasColumnName("ward");

            entity.HasOne(d => d.Account).WithMany(p => p.Profiles)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK__profiles__accoun__5BE2A6F2");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
