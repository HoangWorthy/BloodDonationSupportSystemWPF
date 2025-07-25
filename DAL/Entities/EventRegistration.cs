using System;
using System.Collections.Generic;

namespace DAL.Entities;

public partial class EventRegistration
{
    public long Id { get; set; }

    public long AccountId { get; set; }

    public long ProfileId { get; set; }

    public long EventId { get; set; }

    public string BloodType { get; set; } = null!;

    public DateOnly RegistrationDate { get; set; }

    public string? Status { get; set; }

    public long? TimeSlotId { get; set; }

    public string DonationType { get; set; } = null!;

    public virtual Account Account { get; set; } = null!;

    public virtual DonationEvent Event { get; set; } = null!;

    public virtual Profile Profile { get; set; } = null!;

    public virtual DonationTimeSlot? TimeSlot { get; set; }
}
