using System;
using System.Collections.Generic;

namespace DAL.Entities;

public partial class Profile
{
    public long Id { get; set; }

    public long? AccountId { get; set; }

    public string? Name { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public string? Ward { get; set; }

    public string? District { get; set; }

    public string? City { get; set; }

    public string? BloodType { get; set; }

    public string? Gender { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public DateOnly? LastDonationDate { get; set; }

    public DateOnly? NextEligibleDonationDate { get; set; }

    public string? Status { get; set; }

    public string? PersonalId { get; set; }

    public virtual Account? Account { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    public virtual ICollection<BloodUnit> BloodUnits { get; set; } = new List<BloodUnit>();

    public virtual ICollection<EventRegistration> EventRegistrations { get; set; } = new List<EventRegistration>();
}
