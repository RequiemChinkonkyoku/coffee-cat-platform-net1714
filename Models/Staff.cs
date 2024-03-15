using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models;

public partial class Staff
{
    public int StaffId { get; set; }

//    [Required(ErrorMessage = "Staff Name is required")]
    public string Name { get; set; } = null!;

    public int Gender { get; set; }

//   [Required(ErrorMessage = "Staff Phone is required")]
//    [RegularExpression(@"^\d{10}$", ErrorMessage = "Please enter a valid 10-digit phone number.")]
    public string Phone { get; set; } = null!;

//    [Required(ErrorMessage = "Staff Email is required")]
//    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]

    public string Email { get; set; } = null!;
    
//    [Required(ErrorMessage = "Staff Password is required")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "Staff Status is required")]
    public int Status { get; set; }

    [Required(ErrorMessage = "Staff Role is required")]
    public int? RoleId { get; set; }

    public int? ShopId { get; set; }

    public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();

    public virtual Role? Role { get; set; }

    public virtual Shop? Shop { get; set; }
}
