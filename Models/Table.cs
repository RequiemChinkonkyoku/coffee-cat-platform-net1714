using System;
using System.Collections.Generic;

namespace Models;

public partial class Table
{
    public int TableId { get; set; }

    public int Status { get; set; }

    public int? AreaId { get; set; }

    public virtual Area? Area { get; set; }

    public virtual ICollection<ReservationTable> ReservationTables { get; set; } = new List<ReservationTable>();
}
