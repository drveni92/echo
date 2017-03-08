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
                Zip = town.Zip
            };
        }

        public CategoryModel Create(Category category)
        {
            return new CategoryModel()
            {
                Id = category.Id,
                Name = category.Name,
                Products = category.Products.Select(x => x.Name).ToList()
            };
        }
      
        public ItemModel Create(Item item)
        {
            return new ItemModel()
            {
                Id = item.Id,
                Price = item.Price,
                Quantity = item.Quantity,
                SubTotal = item.SubTotal
            };
        }

        public InvoiceModel Create(Invoice invoice)
        {
            return new InvoiceModel()
            {
                Id = invoice.Id,
                InvoiceNo = invoice.InvoiceNo,
                Date = invoice.Date,
                ShippedOn = invoice.Date,
                SubTotal = invoice.SubTotal,
                Vat = invoice.Vat,
                VatAmount = invoice.VatAmount,
                Shipping = invoice.Shipping,
                Status = invoice.Status,
                Total = invoice.Total,
                Agent = new InvoiceModel.InvoiceAgent()
                {
                    Id = invoice.Agent.Id,
                    Name = invoice.Agent.Name
                },
                Shipper = new InvoiceModel.InvoiceShipper()
                {
                    Id = invoice.Shipper.Id,
                    Name = invoice.Shipper.Name
                },
                Customer = new InvoiceModel.InvoiceCustomer()
                {
                    Id = invoice.Customer.Id,
                    Name = invoice.Customer.Name
                },
                Items = invoice.Items.Select(x => Create(x)).ToList()
            };
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