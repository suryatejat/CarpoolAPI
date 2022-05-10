using Carpool.API.Models;
using Carpool.API.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Carpool.API.Services
{
    public class CustomerServices : ICustomerServices
    {
        private CarpoolDbContext _DbContext;
        private IConfiguration configuration;
        public CustomerServices(CarpoolDbContext DbContext, IConfiguration configuration)
        {
            _DbContext = DbContext;
            this.configuration = configuration;
        }

        public Customer AddCustomer(Customer newCustomer)
        {
            try
            {
                var customer = _DbContext.Customers.SingleOrDefault(x => x.EmailId == newCustomer.EmailId);
                if(customer != null)
                {
                    throw new Exception("There is an account existing with this Email. Please try another Email address");
                }
                _DbContext.Customers.Add(newCustomer);
                _DbContext.SaveChanges();
                return newCustomer;
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public Customer AuthenticateCustomer(Customer customer)
        {
            try
            {
                var foundCustomer = _DbContext.Customers.SingleOrDefault(x => x.EmailId == customer.EmailId || x.CustomerId == customer.CustomerId);
                if (foundCustomer == null)
                    throw new Exception("Invalid Email Id");
                if (foundCustomer.Password != customer.Password)
                    throw new Exception("Invalid Password");
                return customer;
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public List<Customer> AllCustomers()
        {
            try
            {
               return _DbContext.Customers.ToList();
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }


        public Customer DeleteCustomer(string value)
        {
            try
            {
                var customer = _DbContext.Customers.SingleOrDefault(x => x.EmailId == value || x.CustomerId == value);
                if (customer == null)
                    throw new Exception("Invalid CustomerId / EmailId");
                _DbContext.Customers.Remove(customer);
                _DbContext.SaveChanges();
                return customer;
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}