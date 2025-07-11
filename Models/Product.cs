using CourseWebAPI.Models;
using System.ComponentModel.DataAnnotations;

public class Product
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; }

    public decimal Price { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime LastUpdateDate { get; set; }

    public bool? DeleteFlag { get; set; } = false;

    // Foreign key
    public int? CategoryId { get; set; }

    // Navigation property
    public Category? Category { get; set; }
}
