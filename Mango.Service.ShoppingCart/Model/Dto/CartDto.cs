namespace Mango.Service.ShoppingCart.Model.Dto
{
    public class CartDto
    {
        public CartHeaderDto  cartHeader { get; set; }
        public IEnumerable<CartDetailsDto>? cartDetails { get; set; }
    }
}
