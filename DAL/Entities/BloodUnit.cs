using System;
using System.Collections.Generic;

namespace DAL.Entities;

public partial class BloodUnit
{
    public long Id { get; set; }

    public long? EventId { get; set; }

    public long? DonorId { get; set; }

    public long? ProfileId { get; set; }

    public decimal Volume { get; set; }

    public string BloodType { get; set; } = null!;

    public string ComponentType { get; set; } = null!;

    public string Status { get; set; } = null!;

    public long? BloodRequestId { get; set; }

    public virtual BloodRequest? BloodRequest { get; set; }

    public virtual Account? Donor { get; set; }

    public virtual DonationEvent? Event { get; set; }

    public virtual Profile? Profile { get; set; }
}
