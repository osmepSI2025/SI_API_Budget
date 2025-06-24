using System;
using System.Collections.Generic;

namespace SME_API_Budget.Entities;

public partial class ReturnPActivitySub
{
    public int Id { get; set; }

    public int? RefCode { get; set; }

    public string? SubCode { get; set; }

    public string? DataP6 { get; set; }

    public decimal? DataP7 { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public virtual ReturnPActivity? RefCodeNavigation { get; set; }
}
