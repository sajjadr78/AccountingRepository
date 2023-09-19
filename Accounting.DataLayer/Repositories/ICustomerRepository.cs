using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accounting.DataLayer.Services;
using Accounting.ViewModels.Customers;

namespace Accounting.DataLayer.Repositories
{
    public interface ICustomerRepository 
    {
        List<Customers> GetAllCustomers();
        IEnumerable<Customers> GetCustomersByFilter(string parameter);
        List<ListCustomerViewModel> GetNameCustomers(string filter="");
        string GetCustomerNameById(int customerId);
        int GetCustomerIdByName(string name);
        Customers GetCustomerById(int customerId);
        bool InsertCustomer(Customers customer);
        bool UpdateCustomer(Customers customer);
        bool DeleteCustomer(Customers customer);
        bool DeleteCustomer(int customerId);
    }
}
