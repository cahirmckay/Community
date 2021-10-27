using System;
using System.Collections.Generic;

using Community.Core.Models;

namespace Community.Data.Services
{
    // This interface describes the operations that a UserService class should implement
    public interface IUserService
    {
        // Initialise the repository - only to be used during development 
        void Initialise();

        // ---------------- User Management --------------
        IList<User> GetUsers();
        IList<User> GetCommunityUsers(User u);
        User GetUser(int id);
        User GetUserByEmail(string email, int? id);
        User AddUser(string name, string email, int age, string gender, int communityId, string password, Role role);
        User UpdateUser(User user);
        bool DeleteUser(int id);
        User Authenticate(string email, string password);
        User AdminEditUser(User user);
       
    }
    
}
