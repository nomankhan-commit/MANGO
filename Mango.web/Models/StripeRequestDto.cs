﻿using Mango.web.Model.Dto;

namespace Mango.web.Models
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
