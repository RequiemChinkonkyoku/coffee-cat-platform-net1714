using System;
using System.Collections.Generic;

namespace Models;

public partial class Staff
{
    public int StaffId { get; set; }

    public string Name { get; set; } = null!;

    public int Gender { get; set; }

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int Status { get; set; }

    public int? RoleId { get; set; }

    public int? ShopId { get; set; }

    public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();

    public virtual Role? Role { get; set; }

    public virtual Shop? Shop { get; set; }
}
