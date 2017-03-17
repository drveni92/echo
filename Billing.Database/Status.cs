namespace Billing.Database
{
    public enum Status
    {
        Canceled = -1,
        Ordered,
        Confirmed,
        InvoiceCreated,
        InvoiceSent,
        InvoicePayed,
        OnHold,
        Ready,
        Delivered
    }
}