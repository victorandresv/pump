using System;
namespace pump.Models
{
    public class ProductModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public double Price { get; set; }
        public string Img { get; set; }
        public string Company_Id { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
