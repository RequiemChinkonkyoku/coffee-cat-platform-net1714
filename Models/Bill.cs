using System;
using System.Collections.Generic;

namespace Models;

public partial class Bill
{
    public int BillId { get; set; }

    public decimal TotalPrice { get; set; }

    public int Status { get; set; }

    public DateTime PaymentTime { get; set; }

    public string? Note { get; set; }

    public int? ReservationId { get; set; }

    public int? StaffId { get; set; }

    public int? PromotionId { get; set; }

    public virtual ICollection<BillProduct> BillProducts { get; set; } = new List<BillProduct>();

    public virtual Promotion? Promotion { get; set; }

    public virtual Reservation? Reservation { get; set; }

    public virtual Staff? Staff { get; set; }
}
