using System;
using System.Collections.Generic;

namespace Models;

public partial class Area
{
    public int AreaId { get; set; }

    public string Location { get; set; } = null!;

    public int? ShopId { get; set; }

    public virtual ICollection<AreaCat> AreaCats { get; set; } = new List<AreaCat>();

    public virtual Shop? Shop { get; set; }

    public virtual ICollection<Table> Tables { get; set; } = new List<Table>();
}
