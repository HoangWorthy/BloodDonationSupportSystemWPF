using System;
using System.Collections.Generic;

namespace DAL.Entities;

public partial class Account
{
    public long Id { get; set; }

    public long ProfileId { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string? Avatar { get; set; }

    public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();

    public virtual ICollection<BloodUnit> BloodUnits { get; set; } = new List<BloodUnit>();

    public virtual ICollection<DonationEvent> DonationEvents { get; set; } = new List<DonationEvent>();

    public virtual ICollection<EventRegistration> EventRegistrations { get; set; } = new List<EventRegistration>();

    public virtual Profile Profile { get; set; } = null!;

    public virtual ICollection<Profile> Profiles { get; set; } = new List<Profile>();
}
