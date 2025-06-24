using System;
using System.Collections.Generic;

namespace SME_API_Budget.Entities;

public partial class ReturnPExpectedSub
{
    public int Id { get; set; }

    public int KeyId { get; set; }

    public string SubCode { get; set; } = null!;

    public decimal DataPS1 { get; set; }

    public virtual ReturnPExpected Key { get; set; } = null!;
}
