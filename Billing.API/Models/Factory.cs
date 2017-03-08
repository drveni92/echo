﻿using Billing.Database;
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
            AgentModel model = new AgentModel()
            {
                Id = agent.Id,
                Name = agent.Name
            };
            foreach (Town town in agent.Towns.Where(x => x.Customers.Count > 0).ToList())
            {
                model.Towns.Add(town.Name);
            }
            return model;
        }

        public CustomerModel Create(Customer customer)
        {
            CustomerModel model = new CustomerModel()
            {
                Id = customer.Id,
                Name = customer.Name,
                Town = customer.Town.Name
            };

            foreach (Invoice invoice in customer.Invoices.ToList())
            {
                model.InvoicesNo.Add(invoice.InvoiceNo);
            }
            return model;
        }


        public SupplierModel Create(Supplier supplier)
        {
            SupplierModel model = new SupplierModel()
            {
                Id = supplier.Id,
                Name = supplier.Name,
                Town = supplier.Town.Name,
                Address = supplier.Address
            };
            return model;
        }


        public ShipperModel Create(Shipper shipper)
        {
            ShipperModel model = new ShipperModel()
            {
                Id = shipper.Id,
                Name = shipper.Name,
                Address = shipper.Address,
                Town = shipper.Town.Name
            };
            foreach (Invoice invoice in shipper.Invoices)
            {
                model.InvoicesNo.Add(invoice.InvoiceNo);
            }
            return model;
        }

        public ProcurementModel Create(Procurement procurement)
        {
            ProcurementModel model = new ProcurementModel()
            {
                Id = procurement.Id,
                Quantity = procurement.Quantity,
                Price = procurement.Price,
                Date = procurement.Date,
                Document = procurement.Document,
                Product = procurement.Product.Name,
                Supplier = procurement.Supplier.Name
            };
            return model;
        }

        public ProductModel Create(Product product)
        {
            return new ProductModel()
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Category = product.Category.Name,
                Unit = product.Unit,
                Stock = (product.Stock == null) ? 0 : (int)(product.Stock.Input - product.Stock.Output)
            };
        }
    }
}