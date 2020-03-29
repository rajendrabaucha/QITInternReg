using QITInterReg.DAO;
using QITInterReg.DAOImpl;
using QITInterReg.Models;
using QITInterReg.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QITInterReg.ServicesImpl
{
    public class CustomerServiceImpl : CustomerService
    {
        CustomerDAO customerDAO;

        public CustomerServiceImpl()
        {
            customerDAO = new CustomerDAOImpl(new QUIRegDBEntities());
        }

        public void createCustomer(Customer customer)
        {
            customerDAO.create(customer);
        }

        public IEnumerable<Customer> getAll()
        {
            return customerDAO.getAllCustomer();
        }

        public Customer getByCustomerId(int id)
        {
            
            var customerObj = customerDAO.getById(id);

            var customer = new Customer();

            customer.CustomerID = id;

            customer.FName = customerObj.FName;

            customer.LName = customerObj.LName;

            return customer;
        }

        public void update(Customer customer)
        {
            customerDAO.update(customer);
        }

        public void delete(int id)
        {
            customerDAO.delete(id);
        }
    }
}