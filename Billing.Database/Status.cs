using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
