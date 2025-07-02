using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class SystemAccount
{
    public int AccountId { get; set; }

    public string AccountPassword { get; set; } = null!;

    public string EmailAddress { get; set; } = null!;

    public string? AccountNote { get; set; }

    public string Role { get; set; } = null!;
}
