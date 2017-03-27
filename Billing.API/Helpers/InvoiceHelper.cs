using Billing.Database;
using Billing.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.API.Helpers
{
    public class InvoiceHelper
    {
        Invoice Invoice = new Invoice();

        public Invoice NextStep(UnitOfWork unitOfWork, int id, bool Cancel)
        {
            Invoice = unitOfWork.Invoices.Get(id);
            if (Invoice != null)
            {
                switch (Invoice.Status)
                {
                    case (int)Status.OrderCreated: OrderCreated(Cancel); break;
                    case (int)Status.InvoiceCreated: InvoiceCreated(Cancel); break;
                    case (int)Status.InvoiceSent: InvoiceSent(Cancel); break;
                    case (int)Status.InvoicePaid: InvoicePaid(); break;
                    case (int)Status.InvoiceOnHold: InvoiceOnHold(); break;
                    case (int)Status.InvoiceReady: InvoiceReady(); break;
                        //default:
                }
                unitOfWork.Invoices.Update(Invoice, id);
                unitOfWork.Histories.Insert(new History() { Invoice = Invoice, Date = DateTime.Today, Status = Invoice.Status });
                unitOfWork.Commit();
            }
            return Invoice;
        }

        private void OrderCreated(bool Cancel)
        {
            if (Cancel)
            {
                Invoice.Status = (int)Status.Canceled;
            }
            else
            {
                Invoice.Status = (int)Status.InvoiceCreated;
            }
        }

        private void InvoiceCreated(bool Cancel)
        {
            if (Cancel)
            {
                Invoice.Status = (int)Status.Canceled;
            }
            else
            {
                Invoice.Status = (int)Status.InvoiceSent;
            }
        }

        private void InvoiceSent(bool Cancel)
        {
            if (Cancel)
            {
                Invoice.Status = (int)Status.Canceled;
            }
            else
            {
                Invoice.Status = (int)Status.InvoicePaid;
            }
        }

        private void InvoicePaid()
        {
            Invoice.Status = (int)Status.InvoiceReady;
            foreach (var Item in Invoice.Items)
            {
                if (Item.Product.Stock.Invertory < Item.Quantity)
                {
                    Invoice.Status = (int)Status.InvoiceOnHold;
                    break;
                }
            }
        }

        private void InvoiceOnHold()
        {
            Invoice.Status = (int)Status.InvoiceReady;
            foreach (var Item in Invoice.Items)
            {
                if (Item.Product.Stock.Invertory < Item.Quantity)
                {
                    Invoice.Status = (int)Status.InvoiceOnHold;
                    break;
                }
            }
        }

        private void InvoiceReady()
        {
            Invoice.Status = (int)Status.InvoiceShipped;
            Invoice.ShippedOn = DateTime.Today;
        }
    }
}