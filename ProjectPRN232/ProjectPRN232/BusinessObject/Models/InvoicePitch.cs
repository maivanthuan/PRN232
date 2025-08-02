using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class InvoicePitch
{
    public int InvoicePitchId { get; set; }

    public string? PitchId { get; set; }

    public int? PricePitchId { get; set; }

    public int? TotalInvoiceId { get; set; }

    public int? BookingTimeId { get; set; }

    public virtual BookingTime? BookingTime { get; set; }

    public virtual Pitch? Pitch { get; set; }

    public virtual PricePitch? PricePitch { get; set; }

    public virtual TotalInvoicePitch? TotalInvoice { get; set; }
}
