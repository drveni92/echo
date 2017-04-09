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
        public class ItemInvoice
        {
            public int Id { get; set; }
            public string InvoiceNo { get; set; }
        }
        public ItemModel()
        {
            Invoice = new ItemInvoice();
        }

        public int Id { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double SubTotal { get; set; }
        public ItemProduct Product { get; set; }
        public ItemInvoice Invoice { get; set; }

    }
}