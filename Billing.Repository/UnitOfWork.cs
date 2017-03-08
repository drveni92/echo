using Billing.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billing.Repository
{
    public class UnitOfWork : IDisposable
    {
        private readonly BillingContext _context = new BillingContext();

        private IBillingRepository<Agent> _agents;
        private IBillingRepository<Category> _categories;
        private IBillingRepository<Customer> _customers;
        private IBillingRepository<Invoice> _invoices;
        private IBillingRepository<Item> _items;
        private IBillingRepository<Procurement> _procurements;
        private IBillingRepository<Product> _products;
        private IBillingRepository<Shipper> _shippers;
        private IBillingRepository<Supplier> _suppliers;
        private IBillingRepository<Town> _towns;

        public BillingContext Context { get { return _context; } }

        public IBillingRepository<Agent> Agents { get { return _agents ?? (_agents = new BillingRepository<Agent>(_context)); } }
        public IBillingRepository<Category> Categories { get { return _categories ?? (_categories = new BillingRepository<Category>(_context)); } }
        public IBillingRepository<Customer> Customers { get { return _customers ?? (_customers = new BillingRepository<Customer>(_context)); } }
        public IBillingRepository<Invoice> Invoices { get { return _invoices ?? (_invoices = new BillingRepository<Invoice>(_context)); } }
        public IBillingRepository<Item> Items { get { return _items ?? (_items = new BillingRepository<Item>(_context)); } }
        public IBillingRepository<Procurement> Procurements { get { return _procurements ?? (_procurements = new BillingRepository<Procurement>(_context)); } }
        public IBillingRepository<Product> Procurement { get { return _products ?? (_products = new BillingRepository<Product>(_context)); } }
        public IBillingRepository<Shipper> Shippers { get { return _shippers ?? (_shippers = new BillingRepository<Shipper>(_context)); } }
        public IBillingRepository<Supplier> Suppliers { get { return _suppliers ?? (_suppliers = new BillingRepository<Supplier>(_context)); } }
        public IBillingRepository<Town> Towns { get { return _towns ?? (_towns = new BillingRepository<Town>(_context)); } }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
