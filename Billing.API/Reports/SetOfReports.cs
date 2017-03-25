using Billing.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.API.Reports
{
    public class SetOfReports
    {
        private UnitOfWork _unitOfWork;

        public SetOfReports(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private DashboardReport _dashboard;
        private SalesByRegion _salesByRegion;
        private SalesByCategory _salesByCategory;
        private SalesByCustomer _salesByCustomer;
        private SalesByAgentsRegions _salesByAgentsRegions;
        private InvoicesReview _invoicesReview;
        private InvoiceReport _invoicesReport;
        private StockLevel _stockLevel;

        public DashboardReport Dashboard { get { return _dashboard ?? (_dashboard = new DashboardReport(_unitOfWork)); } }
        public SalesByRegion SalesByRegion { get { return _salesByRegion ?? (_salesByRegion = new SalesByRegion(_unitOfWork)); } }
        public SalesByCategory SalesByCategory { get { return _salesByCategory ?? (_salesByCategory = new SalesByCategory(_unitOfWork)); } }
        public SalesByCustomer SalesByCustomer { get { return _salesByCustomer ?? (_salesByCustomer = new SalesByCustomer(_unitOfWork)); } }
        public SalesByAgentsRegions SalesByAgentsRegions { get { return _salesByAgentsRegions ?? (_salesByAgentsRegions = new SalesByAgentsRegions(_unitOfWork)); } }
        public InvoicesReview InvoicesReview { get { return _invoicesReview ?? (_invoicesReview = new InvoicesReview(_unitOfWork)); } }
        public InvoiceReport InvoiceReport { get { return _invoicesReport ?? (_invoicesReport = new InvoiceReport(_unitOfWork)); } }
        public StockLevel StockLevel { get { return _stockLevel ?? (_stockLevel = new StockLevel(_unitOfWork)); } }
    }
}