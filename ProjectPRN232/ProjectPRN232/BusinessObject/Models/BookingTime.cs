using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class BookingTime
{
    public int BookingTimeId { get; set; }

    public string? Time { get; set; }

    public virtual ICollection<InvoicePitch> InvoicePitches { get; set; } = new List<InvoicePitch>();
}
