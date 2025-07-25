using System;
using System.Collections.Generic;

namespace DAL.Entities;

public partial class ComponentRequest
{
    public int Id { get; set; }

    public string? ComponentType { get; set; }

    public double? Volume { get; set; }

    public DateOnly? ExpiredDate { get; set; }

    public virtual ICollection<BloodRequest> BloodRequests { get; set; } = new List<BloodRequest>();
}
