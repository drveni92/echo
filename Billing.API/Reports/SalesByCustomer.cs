using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Billing.Repository;
using Billing.API.Models.Reports;

namespace Billing.API.Reports
{
    public class SalesByCustomer : BaseReport
    {
        public SalesByCustomer(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public SalesByCustomerModel Report(DateTime start, DateTime end)
        {
            SalesByCustomerModel result = new SalesByCustomerModel(start, end);

            var Invoices = _unitOfWork.Invoices.Get().Where(x => (x.Date >= start && x.Date <= end)).ToList();

            result.GrandTotal = Invoices.Sum(x => x.Total);

            var listOfCustomers = Invoices.GroupBy(x => new { Id = x.Customer.Id, Name = x.Customer.Name })
                                          .Select(x => new
                                          {
                                              Id = x.Key.Id,
                                              Name = x.Key.Name,
                                              Turnover = x.Sum(y => y.Items.Sum(z => z.Price * z.Quantity))
                                          }).ToList();

            foreach (var customer in listOfCustomers)
            {
                result.Customers.Add(new CustomerSalesModel()
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    Turnover = customer.Turnover,
                    Percent = Math.Round(customer.Turnover / result.GrandTotal * 100, 2)
                });
            }


            return result;
        }
    }
}