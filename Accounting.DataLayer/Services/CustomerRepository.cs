﻿using Accounting.DataLayer.Repositories;
using Accounting.ViewModels.Customers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.DataLayer.Services
{

    public class CustomerRepository : ICustomerRepository 
    {
        private Accounting_DBEntities db;
        public CustomerRepository(Accounting_DBEntities context)
        {
            db = context;
        }
        public bool DeleteCustomer(Customers customer)
        {
            try
            {
                db.Entry(customer).State = EntityState.Deleted;
                db.SaveChanges();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public bool DeleteCustomer(int customerId)
        {
            var customer = GetCustomerById(customerId);
            bool isSuccess = DeleteCustomer(customer);
            if (isSuccess == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Customers> GetAllCustomers()
        {
            return db.Customers.ToList();

        }

        public Customers GetCustomerById(int customerId)
        {
            return db.Customers.Find(customerId);
        }

        public int GetCustomerIdByName(string name)
        {
            return db.Customers.First(c => c.FullName == name).CustomerID;
        }

        public string GetCustomerNameById(int customerId)
        {
            return db.Customers.Find(customerId).FullName;
        }

        public IEnumerable<Customers> GetCustomersByFilter(string parameter)
        {
            return db.Customers.Where(c => c.FullName.Contains(parameter) || c.Email.Contains(parameter) || c.Mobile.Contains(parameter)).ToList();
        }
        
        public List<ListCustomerViewModel> GetNameCustomers(string filter = "")
        {
            Accounting_DBEntities repository = new Accounting_DBEntities();
            if (filter == "")
            {
                return repository.Customers.Select(c => new ListCustomerViewModel
                {
                    CustomerID = c.CustomerID,
                    FullName = c.FullName
                }).ToList();
            }
            return repository.Customers.Where(c => c.FullName.Contains(filter)).Select(c => new ListCustomerViewModel
            {
                CustomerID = c.CustomerID,
                FullName = c.FullName
            }).ToList();
        }

        public bool InsertCustomer(Customers customer)
        {
            try
            {
                db.Customers.Add(customer);
                return true;
            }
            catch
            {

                return false;
            }
        }

        public bool UpdateCustomer(Customers customer)
        {
            try
            {
                var local = db.Set<Customers>()
                         .Local
                         .FirstOrDefault(f => f.CustomerID == customer.CustomerID);
                if (local != null)
                {
                    db.Entry(local).State = EntityState.Detached;
                }
                db.Entry(customer).State = EntityState.Modified;
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
