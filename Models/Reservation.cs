using System;
using System.Collections.Generic;

namespace Models;

public partial class Reservation
{
    public int ReservationId { get; set; }

    public DateTime BookingDay { get; set; }

    public TimeSpan StartTime { get; set; }

    public TimeSpan EndTime { get; set; }

    public int Status { get; set; }

    public decimal? TotalPrice { get; set; }

    public int? CustomerId { get; set; }

    public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<ReservationTable> ReservationTables { get; set; } = new List<ReservationTable>();
}
