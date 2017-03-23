using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Billing.Repository;
using Billing.API.Models.Reports;
using Billing.Database;

namespace Billing.API.Reports
{
    public class SalesByCategory : BaseReport
    {
        public SalesByCategory(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public SalesByCategoryModel Report(DateTime start, DateTime end)
        {
            SalesByCategoryModel result = new SalesByCategoryModel(start, end);

            var Invoices = _unitOfWork.Invoices.Get().Where(x => (x.Date >= start && x.Date <= end)).ToList();
            var Items = Invoices.SelectMany(x => x.Items).ToList();
            result.GrandTotal = Invoices.Sum(x => x.SubTotal);

            var query = Items.GroupBy(x => x.Product.Category)
                              .Select(x => new { CategoryId = x.Key, CategoryTotal = x.Sum(y => y.SubTotal) })
                              .ToList();

            foreach (var item in query)
            {
                CategorySalesModel category = new CategorySalesModel()
                {
                    Name = item.CategoryId.Name,
                    Total = item.CategoryTotal,
                    Percent = Math.Round(item.CategoryTotal / result.GrandTotal * 100, 2),
                };

                result.Sales.Add(category);
            }

            return result;

        }

        public SalesByProductModel Report(DateTime start, DateTime end, int CategoryId)
        {
            SalesByProductModel result = new SalesByProductModel();
            var Invoices = _unitOfWork.Invoices.Get().Where(x => (x.Date >= start && x.Date <= end)).ToList();
            var Items = Invoices.SelectMany(x => x.Items).ToList();
            Category a = _unitOfWork.Categories.Get(CategoryId);

            result.StartDate = start;
            result.EndDate = end;
            result.Sales = new List<CategorySalesByProductModel>();
            result.CategoryName = a.Name;
            double CategoryTotal = Items.Where(x => x.Product.Category.Id == CategoryId).Sum(x => x.SubTotal);
            double grandTotal = Invoices.Sum(x => x.Total);

            var query = Items.Where(x => x.Product.Category.Id == CategoryId).GroupBy(x => x.Product.Name)
                               .Select(x => new
                               {
                                   Name = x.Key,
                                   Total = x.Sum(y => y.SubTotal)
                               }).ToList();

            foreach (var item in query)
            {
                CategorySalesByProductModel product = new CategorySalesByProductModel()
                {
                    Name = item.Name,
                    Total = item.Total,
                    Percent = Math.Round(100 * item.Total / CategoryTotal, 2),
                    TotalPercent = Math.Round(100 * item.Total / grandTotal, 2)
                };
                result.Sales.Add(product);

            }
            return result;
        }
    }
}