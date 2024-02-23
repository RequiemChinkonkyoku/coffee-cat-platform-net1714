using System;
using System.Collections.Generic;

namespace Models;

public partial class Shop
{
    public int ShopId { get; set; }

    public string? Name { get; set; }

    public string? Location { get; set; }

    public string? Description { get; set; }

    public string? ContactNumber { get; set; }

    public virtual ICollection<Area> Areas { get; set; } = new List<Area>();

    public virtual ICollection<Cat> Cats { get; set; } = new List<Cat>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();
}
