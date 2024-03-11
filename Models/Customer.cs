using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    [Required(ErrorMessage = "Customer Name is required")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Customer Phone is required")]
    public string? Phone { get; set; }

    [Required(ErrorMessage = "Customer Email is required")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Customer Password is required")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "Customer Status is required")]
    public int Status { get; set; }

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
