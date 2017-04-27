namespace Billing.API.Models
{
    public class AutomaticStatesModel
    {
        public int Id { get; set; }
        public InvoiceModel Invoice { get; set; }
    }
}