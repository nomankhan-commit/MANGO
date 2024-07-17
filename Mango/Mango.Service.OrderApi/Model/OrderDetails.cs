﻿using Mango.Service.OrderApi.Models.Dto;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mango.Service.OrderApi.Model
{
    public class OrderDetails
    {
        [Key]
        public int OrderDetailsId { get; set; }
        public int OrderHeaderId { get; set; }
        [ForeignKey("OrderHeaderId")]
        public OrderHeader? OrderHeader { get; set; }
        public int ProductId { get; set; }
        [NotMapped]
        public ProductDto? ProductDto { get; set; }
        public int Count { get; set; }
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
    }
}
