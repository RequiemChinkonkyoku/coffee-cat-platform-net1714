using System;
using System.Collections.Generic;

namespace Models;

public partial class Role
{
    public int RoleId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();
}
