using Mango.web.Model.Dto;

namespace Mango.web.Models.Dto;

public class CartDto
{
    public CartHeaderDto  cartHeader { get; set; }
    public IEnumerable<CartDetailsDto>? cartDetails { get; set; }
}
