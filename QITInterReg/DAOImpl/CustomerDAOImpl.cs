using QITInterReg.DAO;
using QITInterReg.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace QITInterReg.DAOImpl
{
    public class CustomerDAOImpl : CustomerDAO
    {
        QUIRegDBEntities dbContext;

        public CustomerDAOImpl(QUIRegDBEntities dbContext)
        {
            this.dbContext = dbContext;

        }

        public void create(Customer customer)
        {
            dbContext.Customers.Add(customer);
            dbContext.SaveChanges();
        }

        public IEnumerable<Customer> getAllCustomer()
        {
            return dbContext.Customers.ToList();
        }

        public Customer getById(int id)
        {
            Debug.WriteLine(id);
            return dbContext.Customers.Find(id);
        }

        public void update(Customer customer)
        {
            dbContext.Entry(customer).State = EntityState.Modified;
            dbContext.SaveChanges();

        }

        public void delete(int id)
        {
            Customer customer = dbContext.Customers.Find(id);
            dbContext.Customers.Remove(customer);
            dbContext.SaveChanges();
        }
    }
}