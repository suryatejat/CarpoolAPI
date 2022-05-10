using Carpool.API.Models;
using System.Collections.Generic;

namespace Carpool.API.Services.Interfaces
{
    public interface ICustomerServices
    {
        public List<Customer> AllCustomers();
        public Customer AddCustomer(Customer customer);
        public Customer AuthenticateCustomer(Customer customer);
        public Customer DeleteCustomer(string value);
    }
}