using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Web.Models;

namespace WebApplication.Web.DAL
{
    public interface IUserDAL
    {
        /// <summary>
        /// Retrieves a user from the system by username.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        User GetUser(string username);

        bool CheckIfUserNameExists(string username);

        string PullUserRole(string username);

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="user"></param>
        void CreateUser(User user,string password);

        /// <summary>
        /// Updates a user.
        /// </summary>
        /// <param name="user"></param>
        void UpdateUser(User user,string password);

        bool CheckLogin(string username, string password);

        /// <summary>
        /// Deletes a user from the system.
        /// </summary>
        /// <param name="user"></param>

        void ChangeRole(int userToDeleteId, int currentUserId, string userRole);

        List<User> GetAllUsers();
    }
}
