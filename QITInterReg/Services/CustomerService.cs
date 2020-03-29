using QITInterReg.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QITInterReg.Services
{
    public interface CustomerService
    {
        IEnumerable<Customer> getAll();

        Customer getByCustomerId(int id);

        void createCustomer(Customer customer);

        void update(Customer customer);

        void delete(int id);
    }

}