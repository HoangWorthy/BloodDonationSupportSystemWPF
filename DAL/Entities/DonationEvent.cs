using System;
using System.Collections.Generic;

namespace DAL.Entities;

public partial class DonationEvent
{
    public long Id { get; set; }

    public string Location => $"{Address}, {Ward}, {District}, {City}";

    public string Name { get; set; } = null!;

    public string? Hospital { get; set; }

    public string Address { get; set; } = null!;

    public string? Ward { get; set; }

    public string? District { get; set; }

    public string? City { get; set; }

    public DateOnly DonationDate { get; set; }

    public int RegisteredMemberCount { get; set; }

    public int TotalMemberCount { get; set; }   

    public string Status { get; set; } = null!;

    public string DonationType { get; set; } = null!;

    public long CreatedBy { get; set; }

    public DateOnly CreatedDate { get; set; }

    public virtual ICollection<BloodUnit> BloodUnits { get; set; } = new List<BloodUnit>();

    public virtual Account CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<DonationTimeSlot> DonationTimeSlots { get; set; } = new List<DonationTimeSlot>();

    public virtual ICollection<EventRegistration> EventRegistrations { get; set; } = new List<EventRegistration>();
}
