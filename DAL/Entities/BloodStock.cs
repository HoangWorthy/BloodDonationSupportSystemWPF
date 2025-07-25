using System;
using System.Collections.Generic;

namespace DAL.Entities;

public partial class BloodStock
{
    public int Id { get; set; }

    public double? Volume { get; set; }

    public string? BloodType { get; set; }

    public DateOnly? ExpiredDate { get; set; }
}
