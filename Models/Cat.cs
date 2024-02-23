using System;
using System.Collections.Generic;

namespace Models;

public partial class Cat
{
    public int CatId { get; set; }

    public string Name { get; set; } = null!;

    public int? Gender { get; set; }

    public string Breed { get; set; } = null!;

    public DateTime Birthday { get; set; }

    public string HealthStatus { get; set; } = null!;

    public int? ShopId { get; set; }

    public virtual ICollection<AreaCat> AreaCats { get; set; } = new List<AreaCat>();

    public virtual Shop? Shop { get; set; }
}
