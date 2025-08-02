using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Account
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Name { get; set; }

    public string? Gender { get; set; }

    public string? PhoneNumber { get; set; }

    public string Password { get; set; } = null!;

    public int? RoleId { get; set; }

    public string? Avata { get; set; }

    public string? Otp { get; set; }

    public DateTime? TimeEffective { get; set; }

    public byte? StatusOtp { get; set; }

    public string? DateOfBirth { get; set; }

    public virtual ICollection<FeedbackPitch> FeedbackPitches { get; set; } = new List<FeedbackPitch>();

    public virtual Role? Role { get; set; }

    public virtual ICollection<TotalInvoicePitch> TotalInvoicePitches { get; set; } = new List<TotalInvoicePitch>();
}
