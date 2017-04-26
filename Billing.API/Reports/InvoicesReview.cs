using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Billing.Repository;
using Billing.API.Models.Reports;
using Billing.Database;

namespace Billing.API.Reports
{
    public class InvoicesReview : BaseReport
    {
        public InvoicesReview(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public InvoiceReviewCustomerModel Report(int id, DateTime StartDate, DateTime EndDate)
        {
            InvoiceReviewCustomerModel result = new InvoiceReviewCustomerModel();
            result.StartDate = StartDate;
            result.EndDate = EndDate;
            result.CustomerId = id;
            var Invoices = _unitOfWork.Invoices.Get().Where(x => (x.Date >= StartDate && x.Date <= EndDate)).ToList();
            var Items = Invoices.SelectMany(x => x.Items).ToList();

            var query2 = Invoices.Where(x => x.Customer.Id == id)
                .GroupBy(
                x => new {
                    Id = x.Id,
                    InvoiceNo = x.InvoiceNo,
                    Date = x.Date,
                    ShippedOn = x.ShippedOn,
                    Status =(int)x.Status,
                    Vat = x.Vat,
                    Name = x.Customer.Name
                })
                              .Select(x => new
                              {
                                  Id = x.Key.Id,
                                  InvoiceNo = x.Key.InvoiceNo,
                                  Date = x.Key.Date,
                                  ShippedOn = x.Key.ShippedOn,
                                  Status = x.Key.Status,
                                  Vat = x.Key.Vat,
                                  Total = x.Sum(y => y.Total)
                              }).ToList();
            Customer customer = _unitOfWork.Customers.Get().FirstOrDefault(x => x.Id == id);
            result.CustomerName = customer.Name;
            double total = 0;
            foreach (var item in query2)
            {
                total += Math.Round((item.Total) / (1 + item.Vat / 100), 2);
                result.Invoices.Add(new InvoiceReviewModel()
                {
                    InvoiceId = item.Id,
                    InvoiceNo = item.InvoiceNo,
                    InvoiceTotal = Math.Round((item.Total) / (1 + item.Vat / 100), 2),
                    InvoiceStatus = item.Status,
                    InvoiceDate = item.Date,
                    ShippedOn = item.ShippedOn
                });
            }
            result.GrandTotal = Math.Round(total, 2);

            return result;

        }

        public InvoiceReviewGetModel Report(int id)
        {
            InvoiceReviewGetModel result = new InvoiceReviewGetModel();

            var Invoices = _unitOfWork.Invoices.Get().Where(x => (x.Id == id)).ToList();
            var Items = Invoices.SelectMany(x => x.Items).ToList();
            foreach (var invoice in Invoices)
            {
                result.InvoiceNo = invoice.InvoiceNo;
                result.InvoiceDate = invoice.Date;
                result.InvoiceStatus = (int)invoice.Status;
                result.VatAmount = Math.Round(invoice.VatAmount, 2);
                result.Subtotal = invoice.SubTotal + result.VatAmount;
                result.Shipping = invoice.Shipping;
                result.Shipper = (invoice.Shipper == null) ? null : invoice.Shipper.Name;
                result.ShippedOn = invoice.ShippedOn;

            }
            var query2 = Items
            .GroupBy(
            x => new
            {
                ProductId = x.Product.Id,
                ProductName = x.Product.Name,
                Quantity = x.Quantity,
                Price = x.Price,
                Unit = x.Product.Unit,
                Subtotal = x.SubTotal
            })
                          .Select(x => new
                          {
                              ProductId = x.Key.ProductId,
                              ProductName = x.Key.ProductName,
                              Quantity = x.Key.Quantity,
                              Price = x.Key.Price,
                              Unit = x.Key.Unit,
                              Subtotal = x.Key.Subtotal,
                              Total = x.Sum(y => y.SubTotal)
                          }).ToList();
            var customer = Invoices.FirstOrDefault();
            result.CustomerName = customer.Customer.Name;

            foreach (var item in query2)
            {
                result.Items.Add(new InvoiceReviewItem()
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    Unit = item.Unit,
                    Subtotal = item.Subtotal
                });
            }

            return result;
        }
    }
}