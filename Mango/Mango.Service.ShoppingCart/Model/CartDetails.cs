using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Mango.Service.ShoppingCart.Model.Dto;
using Mango.Service.ShoppingCart.Model.Dto;

namespace Mango.Service.ShoppingCart.Model
{
    public class CartDetails
    {
        [Key]
        public int CartDetailsId { get; set; }
        public int CartHeaderId { get; set; }
        [ForeignKey("CartHeaderId")]
        public CartHeader CartHeader { get; set; }
        public int ProductId { get; set; }

        [NotMapped]
        public ProductDto ProductDto { get; set; }
        public int Count { get; set; }
    }
}
