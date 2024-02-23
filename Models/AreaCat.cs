using System;
using System.Collections.Generic;

namespace Models;

public partial class AreaCat
{
    public int AreaCatId { get; set; }

    public int? AreaId { get; set; }

    public int? CatId { get; set; }

    public DateTime Date { get; set; }

    public virtual Area? Area { get; set; }

    public virtual Cat? Cat { get; set; }
}
