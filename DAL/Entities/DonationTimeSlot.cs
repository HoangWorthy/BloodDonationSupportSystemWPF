using System;
using System.Collections.Generic;

namespace DAL.Entities;

public partial class DonationTimeSlot
{
    public long Id { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public int MaxCapacity { get; set; }

    public int CurrentRegistrations { get; set; }

    public long EventId { get; set; }

    public virtual DonationEvent Event { get; set; } = null!;

    public virtual ICollection<EventRegistration> EventRegistrations { get; set; } = new List<EventRegistration>();
}
