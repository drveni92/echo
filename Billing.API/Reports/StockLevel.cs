using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Billing.Repository;
using Billing.API.Models.Reports;

namespace Billing.API.Reports
{
    public class StockLevel : BaseReport
    {
        public StockLevel(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        // Products By Category (parameter id is category id)
        public StockLevelModel Report(int id)
        {
            var Category = _unitOfWork.Categories.Get(id);
            if (Category == null) throw new Exception("Category not found");

            StockLevelModel result = new StockLevelModel(id, Category.Name);

            result.Products = _unitOfWork.Products.Get().Where(x => x.Category.Id == id).ToList()
                                                        .Select(x => _factory.Create(x.Id, x.Name, x.Stock)).ToList();

            return result;
        }
    }
}