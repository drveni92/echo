﻿using Billing.Database;
using Repository;
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

        private IBillingRepository<ApiUser> _apiUsers;
        private IBillingRepository<AuthToken> _tokens;
        private IBillingRepository<Agent> _agents;
        private IBillingRepository<Category> _categories;
        private IBillingRepository<Customer> _customers;
        private IBillingRepository<Invoice> _invoices;
        private IBillingRepository<Item> _items;
        private IBillingRepository<Procurement> _procurements;
        private IBillingRepository<Product> _products;
        private IBillingRepository<Shipper> _shippers;
        private IBillingRepository<Stock> _stocks;
        private IBillingRepository<Supplier> _suppliers;
        private IBillingRepository<Town> _towns;
        private IBillingRepository<Event> _histories;
        private IBillingRepository<AutomaticStates> _automaticStates;

        public BillingContext Context { get { return _context; } }

        public IBillingRepository<ApiUser> ApiUsers { get { return _apiUsers ?? (_apiUsers = new BillingRepository<ApiUser>(_context)); } }
        public IBillingRepository<AuthToken> Tokens { get { return _tokens ?? (_tokens = new AuthTokenRepository(_context)); } }
        public IBillingRepository<Agent> Agents { get { return _agents ?? (_agents = new AgentsRepository(_context)); } }
        public IBillingRepository<Category> Categories { get { return _categories ?? (_categories = new BillingRepository<Category>(_context)); } }
        public IBillingRepository<Customer> Customers { get { return _customers ?? (_customers = new CustomersRepository(_context)); } }
        public IBillingRepository<Invoice> Invoices { get { return _invoices ?? (_invoices = new InvoicesRepository(_context)); } }
        public IBillingRepository<Item> Items { get { return _items ?? (_items = new ItemsRepository(_context)); } }
        public IBillingRepository<Procurement> Procurements { get { return _procurements ?? (_procurements = new ProcurementsRepository(_context)); } }
        public IBillingRepository<Product> Products { get { return _products ?? (_products = new ProductsRepository(_context)); } }
        public IBillingRepository<Shipper> Shippers { get { return _shippers ?? (_shippers = new ShippersRepository(_context)); } }
        public IBillingRepository<Stock> Stocks { get { return _stocks ?? (_stocks = new BillingRepository<Stock>(_context)); } }
        public IBillingRepository<Supplier> Suppliers { get { return _suppliers ?? (_suppliers = new SuppliersRepository(_context)); } }
        public IBillingRepository<Town> Towns { get { return _towns ?? (_towns = new BillingRepository<Town>(_context)); } }
        public IBillingRepository<Event> Histories { get { return _histories ?? (_histories = new BillingRepository<Event>(_context)); } }
        public IBillingRepository<AutomaticStates> AutomaticStates { get { return _automaticStates ?? (_automaticStates = new AutomaticStatesRepository(_context)); } }

        public void Dispose()
        {
            _context.Dispose();
        }

        public bool Commit()
        {
            return (_context.SaveChanges() > 0);
        }
    }
}
