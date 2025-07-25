using System;
using System.Collections.Generic;

namespace DAL.Entities;

public partial class BloodRequest
{
    public long Id { get; set; }

    public string BloodType { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public string? Status { get; set; }

    public int? ComponentRequestId { get; set; }

    public virtual ICollection<BloodUnit> BloodUnits { get; set; } = new List<BloodUnit>();

    public virtual ComponentRequest? ComponentRequest { get; set; }
}
