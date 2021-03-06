﻿namespace LondonMovingSouth.CatalogService
{

    public class Product
    {
        public int Id { get; set; }
        public int DetailId { get; set; }
        public Category Category { get; set; }
        public string Summary { get; set; }
        public decimal Price { get; set; }
        public string CreatedDate { get; set; }
        public string ModifiedDate { get; set; }
    }
}
