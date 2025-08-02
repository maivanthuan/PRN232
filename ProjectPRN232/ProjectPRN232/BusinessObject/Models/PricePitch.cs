using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class PricePitch
{
    public int PricePitchId { get; set; }

    public string? PitchId { get; set; }

    public int? Price { get; set; }

    public DateTime? TimeStart { get; set; }

    public DateTime? TimeEnd { get; set; }

    public virtual ICollection<InvoicePitch> InvoicePitches { get; set; } = new List<InvoicePitch>();

    public virtual Pitch? Pitch { get; set; }
}
