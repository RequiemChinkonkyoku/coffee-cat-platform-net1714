using System;
using System.Collections.Generic;

namespace Models;

public partial class Promotion
{
    public int PromotionId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int PromotionType { get; set; }

    public int PromotionAmount { get; set; }

    public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();
}
