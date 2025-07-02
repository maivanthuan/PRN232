using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject;

public partial class CosmeticInformation
{
    [Key]
    public int CosmeticId { get; set; }

    public string CosmeticName { get; set; } = null!;

    public string? SkinType { get; set; }

    public DateOnly? ExpirationDate { get; set; }

    public string? CosmeticSize { get; set; }

    public decimal? DollarPrice { get; set; }

    public int? CategoryId { get; set; }

    public virtual CosmeticCategory? Category { get; set; }
}
