using Mango.Service.OrderApi;
using Mango.Service.OrderApi.Model.Dto;

namespace Mango.Service.OrderApi.Models
{
    public class StripeRequestDto
    {
        public string StripeSessionUrl { get; set; }
        public string ApprovedUrl { get; set; }
        public string CancelUrl { get; set; }
        public string StripeSessionId { get; set; }
        public OrderHeaderDto OrderHeaderDto { get; set; }
    }
}
