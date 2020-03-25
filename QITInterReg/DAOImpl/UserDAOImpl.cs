using QITInterReg.DAO;
using QITInterReg.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace QITInterReg.DAOImpl
{
    public class UserDAOImpl : UserDAO
    {
        private QUIRegDBEntities dbContext;

        public UserDAOImpl(QUIRegDBEntities qUIRegDBEntities)
        {
            this.dbContext = qUIRegDBEntities;
        }

        public User getUserOnActivationSent(string id)
        {
            return dbContext.Users.Where(u => u.ActivationCode == new Guid(id)).FirstOrDefault();
        }

        public User getUserByEmail(string email)
        {
            User user = dbContext.Users.Where(u => u.EmailID == email).FirstOrDefault();
            return user;
        }

        public User getUserById(int id)
        {
            return dbContext.Users.Find(id);
        }

        public IEnumerable<User> getUsers()
        {
            throw new NotImplementedException();
        }

        public void insertUser(User user)
        {
            dbContext.Users.Add(user);
            dbContext.SaveChanges();
        }

        public void updateUser(User user)
        {
            dbContext.Configuration.ValidateOnSaveEnabled = false; // This helps to avoid confirm password doesnot match issue on save change
            dbContext.Entry(user).State = EntityState.Modified;
            dbContext.SaveChanges();
        }
    }
}