using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class FeedbackPitch
{
    public int FeedbackId { get; set; }

    public int? UserId { get; set; }

    public string? PitchId { get; set; }

    public string? Content { get; set; }

    public double? Rating { get; set; }

    public DateOnly? TimeFeedback { get; set; }

    public virtual Pitch? Pitch { get; set; }

    public virtual Account? User { get; set; }
}
