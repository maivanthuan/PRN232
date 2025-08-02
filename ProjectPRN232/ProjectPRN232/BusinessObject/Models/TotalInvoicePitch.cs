using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class TotalInvoicePitch
{
    public int TotalInvoiceId { get; set; }

    public int? UserId { get; set; }

    public DateOnly? BookTime { get; set; }

    public virtual ICollection<InvoicePitch> InvoicePitches { get; set; } = new List<InvoicePitch>();

    public virtual Account? User { get; set; }
}
