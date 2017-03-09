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
                Category = product.Category.Name,
                Unit = product.Unit,
                Stock = (product.Stock == null) ? 0 : (int)(product.Stock.Input - product.Stock.Output)
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
            }
            return new Agent()
            {
                Id = model.Id,
                Name = model.Name,
                Towns = towns

            };
        }


        public Category Create(CategoryModel model)
        {
            List<Product> products = new List<Product>();
            foreach (var product in model.Product)
            {
                Product tmp = _unitOfWork.Products.Get().FirstOrDefault(x => x.Id == product.Id);
                if (tmp != null)
                {
                    products.Add(tmp);
                }
            }
            return new Category()
            {
                Id = model.Id,
                Name = model.Name,
                Products = products    
            };
        }
    }
}