﻿using Billing.Api.Models;
using Billing.API.Helpers;
using Billing.API.Helpers.Identity;
using Billing.Database;
using Billing.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Security;

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
            List<AgentModel.AgentTown> towns = new List<AgentModel.AgentTown>();
            foreach (var town in agent.Towns)
            {
                Town tmp = _unitOfWork.Towns.Get().FirstOrDefault(x => x.Id == town.Id);
                if (tmp != null)
                {
                    AgentModel.AgentTown a = new AgentModel.AgentTown();
                    a.Id = tmp.Id;
                    a.Name = tmp.Name;
                    towns.Add(a);
                }
                else throw new Exception("Town not found");
            }
            return new AgentModel()
            {
                Id = agent.Id,
                Name = agent.Name,
                Username = agent.Username,
                Towns = towns
            };
        }

        public CustomerModel Create(Customer customer)
        {
            return new CustomerModel()
            {
                Id = customer.Id,
                Name = customer.Name,
                Address = customer.Address,
                Town = (customer.Town != null) ? new CustomerModel.CustomerTown()
                {
                    Id = customer.Town.Id,
                    Name = customer.Town.Name
                } : null,
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
            List<TownModel.TownCustomer> customers = new List<TownModel.TownCustomer>();
            foreach (var customer in town.Customers)
            {
                Customer tmp = _unitOfWork.Customers.Get().FirstOrDefault(x => x.Id == customer.Id);
                if (tmp != null)
                {
                    TownModel.TownCustomer a = new TownModel.TownCustomer();
                    a.Id = tmp.Id;
                    a.Name = tmp.Name;
                    a.Adress = tmp.Address;
                    customers.Add(a);
                }
                else throw new Exception("Customer not found");
            };

            return new TownModel()
            {
                Id = town.Id,
                Name = town.Name,
                Region = string.Concat(town.Region.ToString().Select(x => Char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' '),
                Zip = town.Zip,
                Customers = customers
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
                ShippedOn = invoice.ShippedOn,
                SubTotal = invoice.SubTotal,
                Vat = invoice.Vat,
                VatAmount = invoice.VatAmount,
                Shipping = invoice.Shipping,
                Status = (int)invoice.Status,
                Total = invoice.Total,
                Agent = new InvoiceModel.InvoiceAgent()
                {
                    Id = invoice.Agent.Id,
                    Name = invoice.Agent.Name
                },
                Shipper = (invoice.Shipper != null) ? new InvoiceModel.InvoiceShipper()
                {
                    Id = invoice.Shipper.Id,
                    Name = invoice.Shipper.Name
                } : new InvoiceModel.InvoiceShipper(),
                Customer = new InvoiceModel.InvoiceCustomer()
                {
                    Id = invoice.Customer.Id,
                    Name = invoice.Customer.Name,
                    Address = invoice.Customer.Address,
                    Town = new InvoiceModel.InvoiceCustomerTown()
                    {
                        Id = invoice.Customer.Town.Id,
                        Name = invoice.Customer.Town.Name
                    }
                },
                Items = invoice.Items.Select(x => Create(x)).ToList(),
                Histories = invoice.History.Select(x => Create(x)).ToList()


            };
        }

        public ProcurementModel Create(Procurement procurement)
        {
            ProcurementModel model = new ProcurementModel()
            {
                Id = procurement.Id,
                Quantity = procurement.Quantity,
                Price = Math.Round(procurement.Price, 2),
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
                    Inventory = product.Stock.Inventory
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
                Product = new StockModel.StockProduct() { Id = stock.Product.Id, Name = stock.Product.Name, Unit = stock.Product.Unit }
            };
        }

        public TokenModel Create(AuthToken authToken)
        {
            return new TokenModel()
            {
                Token = authToken.Token,
                Expiration = authToken.Expiration,
                Remember = authToken.Remember,
                CurrentUser = new CurrentUserModel()
                {
                    Id = authToken.Agent.Id,
                    Name = authToken.Agent.Name,
                    Username = authToken.Agent.Username,
                    Roles = Roles.GetRolesForUser(authToken.Agent.Username).ToList()
                }
            };
        }

        public HistoryModel Create(Event history)
        {
            return new HistoryModel()
            {
                Id = history.Id,
                Date = history.Date,
                Status = (int)history.Status,
                Invoice = new HistoryModel.HistoryInvoice()
                {
                    Id = history.Invoice.Id,
                    InvoiceNo = history.Invoice.InvoiceNo
                }
            };

        }

        public AutomaticStatesModel Create(AutomaticStates states)
        {
            return new AutomaticStatesModel()
            {
                Id = states.Id,
                Invoice = Create(states.Invoice),
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
                Town tmp = _unitOfWork.Towns.Get(town.Id);
                if (tmp != null)
                {
                    towns.Add(tmp);
                }
                else throw new Exception("Town not found");
            }
            string name = model.Name.Split(' ')[0].ToLower();

            return new Agent()
            {
                Id = model.Id,
                Name = model.Name,
                Username = name,
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
            List<Event> histories = new List<Event>();
            Shipper shipper = _unitOfWork.Shippers.Get(model.Shipper.Id);          
            foreach (ItemModel item in model.Items)
            {
                Item tmp = _unitOfWork.Items.Get(item.Id);
                if (tmp != null) items.Add(tmp);
                else
                {
                    item.Invoice.Id = model.Id;
                    items.Add(Create(item)); //if item null create new
                }
            }

            if(model.Id > 0)
            {
                var temps = _unitOfWork.Items.Get().ToList().Where(x => x.Invoice.Id == model.Id);
                foreach(var item in temps)
                {
                    if(!items.Contains(item))
                    {
                        _unitOfWork.Items.Delete(item.Id);
                    }
                }

            }

            foreach (HistoryModel item in model.Histories)
            {
                Event tmp = _unitOfWork.Histories.Get(item.Id);
                if (tmp != null) histories.Add(tmp);
            }
            return new Invoice()
            {
                Id = model.Id,
                Date = model.Date,
                InvoiceNo = model.InvoiceNo,
                ShippedOn = (model.ShippedOn == null) ? null : model.ShippedOn,
                Shipping = model.Shipping,
                Vat = model.Vat,
                Status = (Status)model.Status,
                Agent = _unitOfWork.Agents.Get(model.Agent.Id),
                Customer = _unitOfWork.Customers.Get(model.Customer.Id),
                Shipper = shipper,
                Items = items,
                History = histories
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
            string region =  model.Region.ToString().Replace(" ", string.Empty); ;
            return new Town()
            {
                Id = model.Id,
                Name = model.Name,
                Region = (Region)Enum.Parse(typeof(Region),region),
                Zip = model.Zip,
                Customers = customers
            };
        }

        public Event Create(HistoryModel model)
        {
            return new Event()
            {
                Id = model.Id,
                Date = model.Date,
                Status = (Status)model.Status,
                Invoice = _unitOfWork.Invoices.Get(model.Invoice.Id)
            };
        }

        public AutomaticStates Create(int InvoiceId)
        {
            return new AutomaticStates(_unitOfWork.Agents.Get().ToList().First(x => x.Username == Thread.CurrentPrincipal.Identity.Name).Id)
            {
                Invoice = _unitOfWork.Invoices.Get(InvoiceId)
            };
        }

        // Remember token
        public string Create()  
        {
            string CharBase = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            string Remember = "";
            Random rnd = new Random();
            for (int i = 0; i < 24; i++) Remember += CharBase.Substring(rnd.Next(61), 1);
            return Remember;
        }


        // General object creator
        public ReturnObject<T> Create<T>(int page, int total, List<T> query)
        {
            return new ReturnObject<T>()
            {
                CurrentPage = page,
                TotalItems = total,
                List = query
            };
        }

        public ReturnObject<T> Create<T>(int page, int total, T data)
        {
            return new ReturnObject<T>()
            {
                CurrentPage = page,
                TotalItems = total,
                Data = data
            };
        }
    }
}