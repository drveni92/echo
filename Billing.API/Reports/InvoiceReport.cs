using Billing.API.Models.Reports;
using Billing.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.API.Reports
{
    public class InvoiceReport : BaseReport
    {
        public InvoiceReport(UnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public InvoiceReportModel Report(int InvoiceId)
        {
            InvoiceReportModel result = new InvoiceReportModel();
            var Invoices = _unitOfWork.Invoices.Get().Where(x => x.Id == InvoiceId).ToList();
            foreach (var item in Invoices)
            {
                result.InvoiceNo = item.InvoiceNo;
                result.InvoiceDate = item.Date;
                result.CustomerId = item.Customer.Id;
                result.CustomerAddress = item.Customer.Address;
                //result.ZipCode = item.Customer
                result.Town = item.Customer.Town.Name;
                result.Salesperson = item.Agent.Name;
                //result.OrderDate = item.
                result.ShippedDate = item.ShippedOn;
                result.ShippedVia = item.Shipper.Name;
                result.InvoiceSubtotal = item.SubTotal;
                result.VatAmount = item.VatAmount;
                result.Shipping = item.Shipping;
                result.InvoiceTotal = item.VatAmount + item.SubTotal + item.Shipping;
            }
            result.Items = _unitOfWork.Items.Get().Where(x => x.Invoice.Id == InvoiceId)
                .GroupBy(x => new InvoiceItems
                {
                    ProductId = x.Id,
                    ProductName = x.Product.Name,
                    ProductUnit = x.Product.Unit,
                    Quantity = x.Quantity,
                    Price = x.Price,
                    Subtotal = x.SubTotal
                })
                .Select(x => new InvoiceItems
                {
                ProductId = x.Key.ProductId,
                ProductName = x.Key.ProductName,
                ProductUnit = x.Key.ProductUnit,
                Quantity = x.Key.Quantity,
                Price = x.Key.Price,
                Subtotal = x.Key.Subtotal
                })
                .ToList();


            return result;

        }
    }
}