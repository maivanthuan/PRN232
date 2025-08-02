using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Pitch
{
    public string PitchId { get; set; } = null!;

    public int PitchType { get; set; }

    public string? Image { get; set; }

    public int Status { get; set; }

    public virtual ICollection<FeedbackPitch> FeedbackPitches { get; set; } = new List<FeedbackPitch>();

    public virtual ICollection<InvoicePitch> InvoicePitches { get; set; } = new List<InvoicePitch>();

    public virtual ICollection<PricePitch> PricePitches { get; set; } = new List<PricePitch>();
}
