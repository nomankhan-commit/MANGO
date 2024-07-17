using Mango.Service.OrderApi.Model.Dto;

namespace Mango.Service.OrderApi.Models.Dto;

public class CartDto
{
    public CartHeaderDto  cartHeader { get; set; }
    public IEnumerable<CartDetailsDto>? cartDetails { get; set; }
}
