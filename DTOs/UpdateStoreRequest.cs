namespace Shop_Backend.DTOs;

public class UpdateStoreRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}