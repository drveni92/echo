﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Billing.Repository;
using Billing.API.Models;
using Billing.API.Models.Reports;
using Billing.Database;

namespace Billing.API.Reports
{
    public class SalesByRegion : BaseReport
    {
        public SalesByRegion(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public SalesByRegionModel Report(DateTime start, DateTime end)
        {
            List<Invoice> Invoices = _unitOfWork.Invoices.Get().Where(x => x.Date >= start && x.Date <= end).ToList();
            SalesByRegionModel result = new SalesByRegionModel(start, end)
            {
                GrandTotal = Invoices.Sum(x => x.SubTotal)
            };

            result.Sales = Invoices.OrderBy(x => x.Customer.Id).ToList()
                                   .GroupBy(x => x.Customer.Town.Region.ToString())
                                   .Select(x => _factory.Create(Invoices, x.Key, x.Sum(y => y.SubTotal)))
                                   .ToList();
            return result;
        }

        public SalesByAgentModel Report(DateTime start, DateTime end, int agentId)
        {
            SalesByAgentModel result = new SalesByAgentModel();
            var Invoices = _unitOfWork.Invoices.Get().Where(x => (x.Date >= start && x.Date <= end)).ToList();
            Agent a = _unitOfWork.Agents.Get(agentId);

            result.StartDate = start;
            result.EndDate = end;
            result.Sales = new List<RegionSalesByAgentModel>();
            result.AgentName = a.Name;
            double total = 0;
            double grandTotal = Invoices.Sum(x => x.Total);

            var query = Invoices.Where(x => x.Agent.Id == agentId).GroupBy(x => x.Customer.Town.Region.ToString())
                               .Select(x => new
                               {
                                   Name = x.Key,
                                   Total = x.Sum(y => y.Total)
                               }).ToList();

            foreach (var item in query)
            {
                total += item.Total;
            }

            result.AgentTotal = Math.Round(total, 2);
            result.PercentTotal = Math.Round(100 * total / grandTotal, 2);

            foreach (var item in query)
            {
                RegionSalesByAgentModel region = new RegionSalesByAgentModel()
                {
                    RegionName = item.Name,
                    RegionTotal = Math.Round(item.Total, 2),
                    RegionPercent = Math.Round(100 * item.Total / total, 2),
                    TotalPercent = Math.Round(100 * item.Total / grandTotal, 2)
                };
                result.Sales.Add(region);

            }

            return result;
        }

        public SalesByTowns Report(string name)
        {
            SalesByTowns result = new SalesByTowns();
            Town town = _unitOfWork.Towns.Get().Where(x => x.Name.Equals(name)).FirstOrDefault();
            if (town == null) town = _unitOfWork.Towns.Get().Where(x => x.Name.Contains(name)).FirstOrDefault();
            if (town == null) town = _unitOfWork.Towns.Get().Where(x => name.Contains(x.Name)).FirstOrDefault();
            if (town == null) throw new Exception("Town not found.");
            
            var invoices = _unitOfWork.Invoices.Get().ToList().Where(x => x.Customer.Town.Name == town.Name).ToList();

            result.Region = town.Region.ToString();
            result.Town = town.Name;

            result.Agents = invoices.GroupBy(x => x.Agent).Select(x => new AgentSalesModel { Id = x.Key.Id, Name = x.Key.Name, Total = x.Sum(y => y.Total) }).ToList();

            return result;
        }
    }
}