﻿using Billing.Database;
using Billing.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.API.Helpers
{
    public class InvoiceHelper
    {
        private UnitOfWork _unitOfWork;
        Invoice Invoice = new Invoice();

        public Invoice NextStep(UnitOfWork unitOfWork, int id, bool Cancel)
        {
            _unitOfWork = unitOfWork;
            Invoice = unitOfWork.Invoices.Get(id);
            if (Invoice != null)
            {
                switch (Invoice.Status)
                {
                    case Status.OrderCreated: OrderCreated(Cancel); break;
                    case Status.OrderConfirmed: OrderConfirmed(Cancel); break;
                    case Status.InvoiceCreated: InvoiceCreated(Cancel); break;
                    case Status.InvoiceSent: InvoiceSent(Cancel); break;
                    case Status.InvoicePaid: InvoicePaid(); break;
                    case Status.InvoiceOnHold: InvoiceOnHold(); break;
                    case Status.InvoiceReady: InvoiceReady(); break;
                        //default:
                }
                unitOfWork.Invoices.Update(Invoice, id);
                unitOfWork.Histories.Insert(new Event() { Invoice = Invoice, Date = DateTime.Today, Status = Invoice.Status });
                unitOfWork.Commit();
            }
            return Invoice;
        }

        private void OrderCreated(bool Cancel)
        {
            if (Cancel)
            {
                Invoice.Status = Status.Canceled;
            }
            else
            {
                Invoice.Status = Status.OrderConfirmed;
            }
        }

        private void OrderConfirmed(bool Cancel)
        {
            if (Cancel)
            {
                Invoice.Status = Status.Canceled;
            }
            else
            {
                Invoice.Status = Status.InvoiceCreated;
            }
        }

        private void InvoiceCreated(bool Cancel)
        {
            if (Cancel)
            {
                Invoice.Status = Status.Canceled;
            }
            else
            {
                Invoice.Status = Status.InvoiceSent;
            }
        }

        private void InvoiceSent(bool Cancel)
        {
            if (Cancel)
            {
                Invoice.Status = Status.Canceled;
            }
            else
            {
                Invoice.Status = Status.InvoicePaid;
            }
        }

        private void InvoicePaid()
        {
            Invoice.Status = Status.InvoiceReady;
            foreach (var Item in Invoice.Items)
            {
                if (Item.Product.Stock.Inventory < Item.Quantity)
                {
                    Invoice.Status = Status.InvoiceOnHold;
                    //break;
                    throw new Exception("Not enough items in stock");
                }
            }
        }

        private void InvoiceOnHold()
        {
            Invoice.Status = Status.InvoiceReady;
            foreach (var Item in Invoice.Items)
            {
                if (Item.Product.Stock.Inventory < Item.Quantity)
                {
                    Invoice.Status = Status.InvoiceOnHold;
                    //break;
                    throw new Exception("Not enough items in stock");
                }
            }
        }

        private void InvoiceReady()
        {
            Invoice.Status = Status.InvoiceShipped;
            Invoice.ShippedOn = DateTime.Today;
            var states = _unitOfWork.AutomaticStates.Get().Where(x => x.Invoice.Id == Invoice.Id).ToList();
            if (states.Count != 0)
            {
                var state = states.First();
                state.Completed = true;
                _unitOfWork.AutomaticStates.Update(state, state.Id);
                _unitOfWork.Commit();
            }
        }
    }
}