using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Mango.Service.OrderApi.Models.Dto;

namespace Mango.Service.OrderApi.Model.Dto
{
    public class CartDetailsDto
    {
       
        public int CartDetailsId { get; set; }
        public int CartHeaderId { get; set; }
        public CartHeaderDto? CartHeader { get; set; }
        public int ProductId { get; set; }
        public ProductDto? ProductDto { get; set; }
        public int Count { get; set; }
    }
}
