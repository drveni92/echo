﻿using Billing.Api.Models;
using Billing.Database;
using Billing.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.API.Models
{
    public class Factory
    {
        private UnitOfWork _unitOfWork;

        public Factory(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //Entity To Model

        public AgentModel Create(Agent agent)
        {
            return new AgentModel()
            {
                Id = agent.Id,
                Name = agent.Name,
                Towns = new List<AgentModel.AgentTown>(agent.Towns.Where(x => x.Customers.Count != 0).Select(x => new AgentModel.AgentTown() { Id = x.Id, Name = x.Name}).ToList())
            };
        }

        public CustomerModel Create(Customer customer)
        {
            return new CustomerModel()
            {
                Id = customer.Id,
                Name = customer.Name,
                Address = customer.Address,
                Town = new CustomerModel.CustomerTown()
                {
                    Id = customer.Town.Id,
                    Name = customer.Town.Name
                },
                InvoicesNo = customer.Invoices.Select(x => x.InvoiceNo).ToList()
            };
        }

        internal object Create(AuthToken authToken)
        {
            throw new NotImplementedException();
        }

        public SupplierModel Create(Supplier supplier)
        {
            return new SupplierModel()
            {
                Id = supplier.Id,
                Name = supplier.Name,
                Address = supplier.Address,
                Town = new SupplierModel.SupplierTown()
                {
                    Id = supplier.Town.Id,
                    Name = supplier.Town.Name
                }
            };
        }

        public ShipperModel Create(Shipper shipper)
        {
            return new ShipperModel()
            {
                Id = shipper.Id,
                Name = shipper.Name,
                Address = shipper.Address,
                Town = (shipper.Town != null) ? new ShipperModel.ShipperTown()
                {
                    Id = shipper.Town.Id,
                    Name = shipper.Town.Name
                } : new ShipperModel.ShipperTown(),
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
                Name = category.Name
            };
        }
      
        public ItemModel Create(Item item)
        {
            return new ItemModel()
            {
                Id = item.Id,
                Price = item.Price,
                Quantity = item.Quantity,
                SubTotal = item.SubTotal,
                Product = new ItemModel.ItemProduct()
                {
                    Id = item.Product.Id,
                    Name = item.Product.Name,
                    Unit = item.Product.Unit
                },
                Invoice = new ItemModel.ItemInvoice()
                {
                    Id = item.Invoice.Id,
                    InvoiceNo = item.Invoice.InvoiceNo
                }
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
                Product = new ProcurementModel.ProcurementProduct()
                {
                    Id = procurement.Product.Id,
                    Name = procurement.Product.Name
                },
                Supplier = new ProcurementModel.ProcurementSupplier()
                {
                    Id = procurement.Supplier.Id,
                    Name = procurement.Supplier.Name
                }
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
                Category = new ProductModel.ProductCategory()
                {
                    Id = product.Category.Id,
                    Name = product.Category.Name
                },
                Unit = product.Unit,
                Stock = (product.Stock != null) ? new ProductModel.ProductStock()
                {
                    Input = product.Stock.Input,
                    Output = product.Stock.Output,
                    Inventory = product.Stock.Invertory
                } : new ProductModel.ProductStock()
            };
        }

        public StockModel Create(Stock stock)
        {
            return new StockModel()
            {
                Id = stock.Id,
                Input = stock.Input,
                Output = stock.Output,
                Product = new StockModel.StockProduct() { Id = stock.Product.Id, Name = stock.Product.Name }
            };
        }
        
        //Model To Entity

        public Customer Create(CustomerModel model)
        {
            return new Customer()
            {
                Id = model.Id,
                Name = model.Name,
                Address = model.Address,
                Town = _unitOfWork.Towns.Get(model.Town.Id)
            };

        }

        public Supplier Create(SupplierModel model)
        {
            return new Supplier()
            {
                Id = model.Id,
                Name = model.Name,
                Address = model.Address,
                Town = _unitOfWork.Towns.Get(model.Town.Id)
            };
        }

        public Procurement Create(ProcurementModel model)
        {
            return new Procurement()
            {
                Id = model.Id,
                Quantity = model.Quantity,
                Price = model.Price,
                Date = model.Date,
                Document = model.Document,
                Product = _unitOfWork.Products.Get(model.Product.Id),
                Supplier = _unitOfWork.Suppliers.Get(model.Supplier.Id)
            };
        }

        public Shipper Create(ShipperModel model)
        {
            return new Shipper()
            {
                Id = model.Id,
                Name = model.Name,
                Address = model.Address,
                Town = _unitOfWork.Towns.Get(model.Town.Id)
            };
        }

        public Agent Create(AgentModel model)
        {
            List<Town> towns = new List<Town>();
            foreach (var town in model.Towns)
            {
                Town tmp = _unitOfWork.Towns.Get().FirstOrDefault(x => x.Id == town.Id);
                if (tmp != null)
                {
                    towns.Add(tmp);
                }
                else throw new Exception("Town not found");
            }
            return new Agent()
            {
                Id = model.Id,
                Name = model.Name,
                Towns = towns

            };
        }

        public Item Create(ItemModel model)
        {
            return new Item()
            {
                Id = model.Id,
                Price = model.Price,
                Quantity = model.Quantity,
                Product = _unitOfWork.Products.Get(model.Product.Id),
                Invoice = _unitOfWork.Invoices.Get(model.Invoice.Id)
            };
        }

        public Category Create(CategoryModel model)
        {
            return new Category()
            {
                Id = model.Id,
                Name = model.Name   
            };
        }

        public Product Create(ProductModel model)
        {
            Stock stock = _unitOfWork.Stocks.Get(model.Id);
            return new Product()
            {
                Id = model.Id,
                Name = model.Name,
                Price = model.Price,
                Unit = model.Unit,
                Category = _unitOfWork.Categories.Get(model.Category.Id),
                Stock = (stock != null) ? stock : new Stock() { Input = model.Stock.Input, Output = model.Stock.Output }
            };
        }

        public Invoice Create(InvoiceModel model)
        {
            List<Item> items = new List<Item>();
            foreach (ItemModel item in model.Items)
            {
                Item tmp = _unitOfWork.Items.Get(item.Id);
                if (tmp != null) items.Add(tmp);
            }
            return new Invoice()
            {
                Id = model.Id,
                Date = model.Date,
                InvoiceNo = model.InvoiceNo,
                ShippedOn = model.ShippedOn,
                Shipping = model.Shipping,
                Vat = model.Vat,
                Status = model.Status,
                Agent = _unitOfWork.Agents.Get(model.Agent.Id),
                Customer = _unitOfWork.Customers.Get(model.Customer.Id),
                Shipper = _unitOfWork.Shippers.Get(model.Shipper.Id),
                Items = items
            };
        }

        public Town Create(TownModel model)
        {
            List<Customer> customers = new List<Customer>();
            foreach (var customer in model.Customers)
            {
                Customer tmp = _unitOfWork.Customers.Get().FirstOrDefault(x => x.Id == customer.Id);
                if (tmp != null)
                {
                    customers.Add(tmp);
                }
            }
            return new Town()
            {
                Id = model.Id,
                Name = model.Name,
                Region =(Region)Enum.Parse(typeof(Region), model.Region),
                Zip = model.Zip,
                Customers = customers
            };
        }
    }
}