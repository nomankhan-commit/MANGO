﻿using Mango.Service.OrderApi.Models.Dto;

namespace Mango.Service.OrderApi.Model.Dto
{
    public class OrderDetailsDto
    {
   
        public int OrderDetailsId { get; set; }
        public int OrderHeaderId { get; set; }
        public int ProductId { get; set; }
        public ProductDto? ProductDto { get; set; }
        public int Count { get; set; }
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
        public OrderHeaderDto? OrderHeader { get; set; }
    }
}
