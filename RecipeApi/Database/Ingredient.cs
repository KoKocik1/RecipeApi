using System.ComponentModel.DataAnnotations;

public class Ingredient
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public double Quantity { get; set; }
    [Required]
    public string Unit { get; set; }
}
