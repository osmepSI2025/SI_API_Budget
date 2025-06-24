using System;
using System.Collections.Generic;

namespace SME_API_Budget.Entities;

public partial class ReturnPPlanBdg
{
    public int Id { get; set; }

    public int RefCode { get; set; }

    public string DataP1 { get; set; } = null!;

    public string YearBdg { get; set; } = null!;

    public string ProjectCode { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public DateTime UpdateDate { get; set; }

    public virtual ICollection<ReturnPPlanBdgSub> ReturnPPlanBdgSubs { get; set; } = new List<ReturnPPlanBdgSub>();
}
