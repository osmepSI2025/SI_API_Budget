using System;
using System.Collections.Generic;

namespace SME_API_Budget.Entities;

public partial class RecPR
{
    public int Id { get; set; }

    public string? YearBdg { get; set; }

    public string? ProjectCode { get; set; }

    public string? ActivityName { get; set; }

    public int? RefCode { get; set; }

    public string? DataPS1 { get; set; }

    public string? DataPS2 { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public string? Response { get; set; }
}
