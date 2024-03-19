using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Models;

public partial class Cat
{
    public int CatId { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [RegularExpression(@"^[a-zA-Z''-'\s]{1,50}$", ErrorMessage = "Name can only contain letters and spaces")]
    public string Name { get; set; } = null!;

    [Range(0, 1, ErrorMessage = "Invalid gender value")]
    public int? Gender { get; set; }

    [Required(ErrorMessage = "Breed is required")]
    public string Breed { get; set; } = null!;

    public DateTime Birthday { get; set; }

    [Required(ErrorMessage = "HealthStatus is required")]
    public int? HealthStatus { get; set; }

    public int? ShopId { get; set; }

    [Required(ErrorMessage = "Image is required")]
    public string ImageUrl { get; set; }

    public string Description { get; set; }

    public virtual ICollection<AreaCat> AreaCats { get; set; } = new List<AreaCat>();

    public virtual Shop? Shop { get; set; }
}
