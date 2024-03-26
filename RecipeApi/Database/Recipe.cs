using System.ComponentModel.DataAnnotations.Schema;

public class Recipe
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<Ingredient> Ingredients { get; set; }
    public string Instructions { get; set; }
    
}