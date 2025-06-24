using System;
using System.Collections.Generic;

namespace SME_API_Budget.Entities;

public partial class ReturnPPlanBdgSub
{
    public int Id { get; set; }

    public string SubCode { get; set; } = null!;

    public int RefCode { get; set; }

    public string DataPS1 { get; set; } = null!;

    public decimal DataPS2 { get; set; }

    public string RefCode2 { get; set; } = null!;

    public string BdgType { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public DateTime UpdateDate { get; set; }

    public virtual ReturnPPlanBdg RefCodeNavigation { get; set; } = null!;
}
