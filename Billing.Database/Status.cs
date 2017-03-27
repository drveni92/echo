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
        OrderCreated,
        InvoiceCreated,
        InvoiceSent,
        InvoicePaid,
        InvoiceOnHold,
        InvoiceReady,
        InvoiceShipped
    }
}
