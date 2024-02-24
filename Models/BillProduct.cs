using System;
using System.Collections.Generic;

namespace Models;

public partial class BillProduct
{
    public int BillProductId { get; set; }

    public int Quantity { get; set; }

    public int? BillId { get; set; }

    public int? ProductId { get; set; }

    public virtual Bill? Bill { get; set; }

    public virtual Product? Product { get; set; }
}
