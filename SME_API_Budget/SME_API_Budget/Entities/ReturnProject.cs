using System;
using System.Collections.Generic;

namespace SME_API_Budget.Entities;

public partial class ReturnProject
{
    public int Id { get; set; }

    public int? KeyId { get; set; }

    public string? DataP1 { get; set; }

    public string? DataP2 { get; set; }

    public string? DataP3 { get; set; }

    public DateTime? DataP4 { get; set; }

    public DateTime? DataP5 { get; set; }

    public string? DataP6 { get; set; }

    public string? DataP7 { get; set; }

    public string? DataP8 { get; set; }

    public string? DataP9 { get; set; }

    public string? DataP10 { get; set; }

    public string? DataP11 { get; set; }

    public decimal? DataP12 { get; set; }

    public decimal? DataP13 { get; set; }

    public string? YearBdg { get; set; }

    public string? ProjectCode { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }
}
