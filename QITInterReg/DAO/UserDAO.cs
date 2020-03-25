using QITInterReg.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QITInterReg.DAO
{
    public interface UserDAO
    {

        void insertUser(User user);

        IEnumerable<User> getUsers();

        User getUserById(int id);

        void updateUser(User user);

        User getUserByEmail(String email);

        User getUserOnActivationSent(String id);
    }
}