using QITInterReg.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QITInterReg.DAO
{
    public interface CustomerDAO
    {

        IEnumerable<Customer> getAllCustomer();

        Customer getById(int id);

        void create(Customer customer);

        void update(Customer customer);

        void delete(int id);
    }
}