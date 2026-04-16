using Shop_Backend.Models;

public class Store
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int OwnerId { get; set; }
    public User? Owner { get; set; }
    public int? ProvinceId { get; set; }
    public Province? Province { get; set; }
    public ICollection<Product> Products { get; set; } = new List<Product>();

}