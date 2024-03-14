using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models;

public partial class Customer
{
    public int CustomerId { get; set; }
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, ErrorMessage = "Name field must not exceed 100 characters.")]

    public string? Name { get; set; }
    [Required(ErrorMessage = "Phone number is required.")]

    public string? Phone { get; set; }
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    [StringLength(100, ErrorMessage = "Email field must not exceed 100 characters.")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, ErrorMessage = "Password field must not exceed 100 characters.")]
    public string? Password { get; set; } = null!;

    [Required(ErrorMessage = "Status is required")]
    public int Status { get; set; }

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
