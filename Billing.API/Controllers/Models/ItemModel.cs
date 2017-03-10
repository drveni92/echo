namespace Billing.API.Models
{
    public class ItemModel
    {
        public struct ItemProduct
        {
            public int Id;
            public string Name;
            public string Unit;
        }
        public int Id { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double SubTotal { get; set; }
        public ItemProduct Product { get; set; }

    }
}