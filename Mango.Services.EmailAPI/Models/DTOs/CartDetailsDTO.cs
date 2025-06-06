﻿namespace Mango.Services.EmailAPI.Models.DTOs
{
    public class CartDetailsDTO
    {
        public int CartDetailsID { get; set; }
        public int CartHeaderID { get; set; }
        public CartHeaderDTO? CartHeader { get; set; }
        public int ProductID { get; set; }
        public ProductDTO? Product { get; set; }
        public int Count { get; set; }
    }
}
