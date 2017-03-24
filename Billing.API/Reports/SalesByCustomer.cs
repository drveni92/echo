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
            var Invoices = _unitOfWork.Invoices.Get().Where(x => (x.Date >= start && x.Date <= end)).ToList();

            SalesByCustomerModel result = new SalesByCustomerModel(start, end)
            {
                GrandTotal = Invoices.Sum(x => x.Total)
            };

            result.Customers = Invoices.GroupBy(x => new { Id = x.Customer.Id, Name = x.Customer.Name })
                                       .Select(x => _factory.Create(x.Key.Id, x.Key.Name, x.Sum(y => y.Items.Sum(z => z.Price * z.Quantity)), result.GrandTotal))
                                       .OrderByDescending(x => x.Turnover)
                                       .ToList();
            return result;
        }
    }
}