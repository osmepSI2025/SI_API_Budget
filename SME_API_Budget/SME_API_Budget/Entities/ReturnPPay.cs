using System;
using System.Collections.Generic;

namespace SME_API_Budget.Entities;

public partial class ReturnPPay
{
    public int Id { get; set; }

    public string? DataP1 { get; set; }

    public string? DataP2 { get; set; }

    public string? YearBdg { get; set; }

    public string? ProjectCode { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }
}
