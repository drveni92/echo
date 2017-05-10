using Billing.Database.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SqlClient;
using System.Linq;

namespace Billing.Database
{
    public class BillingContext : DbContext
    {
        public BillingContext() : base("name=Billing.Database") { }


        public DbSet<Agent> Agents { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Procurement> Procurements { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Shipper> Shippers { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Town> Towns { get; set; }
        public DbSet<ApiUser> ApiUsers { get; set; }
        public DbSet<AuthToken> AuthTokens { get; set; }
        public DbSet<Event> History { get; set; }
        public DbSet<AutomaticStates> AutomaticStates { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Ignore<Basic>();
           
            var conv = new AttributeToTableAnnotationConvention<SoftDeleteAttribute, string>(
               "SoftDeleteColumnName",
               (type, attributes) => attributes.Single().ColumnName);

            modelBuilder.Conventions.Add(conv);

        }
    }
}
