namespace Core.Dtos.Orders;

public class OrderItemDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public string PictureUrl { get; set; }
    public decimal Price { get; private set; }
    public int Quantity { get; private set; }
}