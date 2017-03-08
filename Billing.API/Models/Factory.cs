using Billing.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.API.Models
{
    public class Factory
    {
        public AgentModel Create(Agent agent)
        {
            return new AgentModel()
            {
                Id = agent.Id,
                Name = agent.Name,
                Towns = agent.Towns.Where(x => x.Customers.Count != 0).Select(x => x.Name).ToList()
            };
        }

        public CustomerModel Create(Customer customer)
        {
            return new CustomerModel()
            {
                Id = customer.Id,
                Name = customer.Name,
                Town = customer.Town.Name,
                InvoicesNo = customer.Invoices.Select(x => x.InvoiceNo).ToList()
            };
        }


        public SupplierModel Create(Supplier supplier)
        {
            return new SupplierModel()
            {
                Id = supplier.Id,
                Name = supplier.Name,
                Town = supplier.Town.Name,
                Address = supplier.Address
            };
        }


        public ShipperModel Create(Shipper shipper)
        {
            return new ShipperModel()
            {
                Id = shipper.Id,
                Name = shipper.Name,
                Address = shipper.Address,
                Town = shipper.Town.Name,
                InvoicesNo = shipper.Invoices.Select(x => x.InvoiceNo).ToList()
            };
        }

        public TownModel Create(Town town)
        {
            return new TownModel()
            {
                Id = town.Id,
                Name = town.Name,
                Region = town.Region.ToString(),
                Zip = town.Zip,
                Customers = town.Customers.Select(x => x.Name).ToList(),
                Suppliers = town.Suppliers.Select(x => x.Name).ToList(),
                Shippers = town.Shippers.Select(x => x.Name).ToList(),
                Agents = town.Agents.Select(x => x.Name).ToList()

            };
        }

        public CategoryModel Create(Category category)
        {
            return new CategoryModel()
            {
                Id = category.Id,
                Name = category.Name,
                Products = category.Products.Select(x => x.Name).ToList(),
            };
        }
    }
}