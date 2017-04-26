using Billing.API.Models.Reports;
using Billing.Database;
using Billing.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.API.Reports
{
    public class SalesByCustomersCategories : BaseReport
    {
        public SalesByCustomersCategories(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public SalesCustomersCategoriesModel Report(DateTime start, DateTime end, int page = 0)
        {
            SalesCustomersCategoriesModel result = new SalesCustomersCategoriesModel(start, end);

            List<Customer> Customers = _unitOfWork.Customers.Get().ToList();

            List<InputCross> CustomersByCategories = _unitOfWork.Items.Get()
                                               .Where(x => (x.Invoice.Date >= start && x.Invoice.Date <= end)).ToList()
                                               .GroupBy(x => new
                                               {
                                                   CustomerName = x.Invoice.Customer.Name,
                                                   CategoryName = x.Product.Category.Name
                                               })
                                               .Select(x => new InputCross { Row = x.Key.CustomerName, Column = x.Key.CategoryName, Value = x.Sum(y => y.SubTotal) })
                                               .OrderByDescending(x => x.Value)
                                               .ToList();

            result.Customers = _factory.CreateCat(CustomersByCategories, Customers, _unitOfWork.Categories.Get().ToList());
            result.Categories = _factory.CreateReverseCat(CustomersByCategories, _unitOfWork.Categories.Get().ToList());
            result.GrandTotal = result.Customers.Sum(x => x.Turnover);

            return result;
        }
    }
}