namespace LondonMovingSouth.CatalogService
{
    public class Details
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }
        public string Dimensions { get; set; }
        public string Weight { get; set; }
        public string Colour { get; set; }
        public int Quantity { get; set; }
        public Condition Condition { get; set; }
    }
}