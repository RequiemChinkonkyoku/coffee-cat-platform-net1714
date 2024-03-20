using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models;

public partial class Promotion
{
    public int PromotionId { get; set; }

    [Required(ErrorMessage ="Name cannot be null")]
    [MaxLength(100, ErrorMessage ="Name cannot exceed 100 characters")]
    public string Name { get; set; } = null!;
    [MaxLength(250, ErrorMessage = "Description cannot exceed 250 characters")]
    public string? Description { get; set; }
    [Range(0,1, ErrorMessage ="Invalid Promotion Type")]
    public int PromotionType { get; set; }

    [Range(0,double.MaxValue, ErrorMessage = "PromotionAmount cannot be negative value")]
    public int PromotionAmount { get; set; }

    public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();
}
