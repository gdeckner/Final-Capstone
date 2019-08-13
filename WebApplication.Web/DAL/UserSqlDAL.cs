using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Web.Models;
using WebApplication.Web.Security;

namespace WebApplication.Web.DAL
{
    public class UserSqlDAL : IUserDAL
    {
        private readonly string connectionString;
        private readonly IPasswordHasher passHasher;

        public UserSqlDAL(string connectionString, IPasswordHasher passwordHasher)
        {
            this.connectionString = connectionString;
            passHasher = passwordHasher;
        }

        public bool CheckIfUserNameExists(string username)
        {
            bool doesExist = false;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = @"select userName from userLogin
                where userName = @userName";
                cmd.Parameters.AddWithValue("@userName", username.ToLower());
                SqlDataReader reader = cmd.ExecuteReader();

                if(reader.Read())
                {
                    doesExist = true;
                }
            }
            return doesExist;
        }

        /// <summary>
        /// Saves the user to the database.
        /// </summary>
        /// <param name="user"></param>
        public void CreateUser(User user,string password)
        {
            byte[] salt = passHasher.GenerateRandomSalt();
            string hashedPassword = passHasher.ComputeHash(password, salt);

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO UserLogin (first_Last_Name, userName, password, salt, userRole) VALUES (@name, @username, @password, @salt, @role);", conn);
                    cmd.Parameters.AddWithValue("@name", user.Name);
                    cmd.Parameters.AddWithValue("@username", user.Username);
                    cmd.Parameters.AddWithValue("@password", hashedPassword);
                    cmd.Parameters.AddWithValue("@salt", Convert.ToBase64String(salt));
                    cmd.Parameters.AddWithValue("@role", user.Role);

                    cmd.ExecuteNonQuery();

                    return;
                }
            }
            catch(SqlException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Deletes the user from the database.
        /// </summary>
        /// <param name="user"></param>

        //public void DeleteUser(int id)

        public void DeleteUser(int userToDeleteId, int currentUserId)
        {

            if(userToDeleteId == currentUserId)
            {
              //do nothing   
            }
            else
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand(@"
                        Delete from Payroll WHERE userId = @id 
                        Delete from Hours WHERE userId = @id
                        Delete from UserJob WHERE userId = @id
                        DELETE FROM userLogin WHERE userId = @id;", conn);
                        cmd.Parameters.AddWithValue("@id", userToDeleteId);

                        cmd.ExecuteNonQuery();

                        return;
                    }
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
            }
           
        }

        /// <summary>
        /// Gets the user from the database.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public User GetUser(string username)
        {
            User user = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM UserLogin WHERE userName = @username;", conn);
                    cmd.Parameters.AddWithValue("@username", username);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        user = MapRowToUser(reader);
                    }
                }

                return user;
            }
            catch (SqlException ex)
            {
                throw ex;
            }            
        }

        public string PullUserRole(string username)
        {
            string roleResult;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = @"select userRole, username
                from userLogin
                where userName = @username";
                cmd.Parameters.AddWithValue("@username", username);
                roleResult = Convert.ToString(cmd.ExecuteScalar());
            }
            return roleResult;
        }

        /// <summary>
        /// Gets the user from the database.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM UserLogin;", conn);
                    
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        users.Add(MapRowToUser(reader));
                    }
                }

                return users;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public List<User> GetAllNonAdmin()
        {
            List<User> users = new List<User>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(@"SELECT * FROM UserLogin
                    where userRole = 'Users'", conn);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        users.Add(MapRowToUser(reader));
                    }
                }

                return users;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Updates the user in the database.
        /// </summary>
        /// <param name="user"></param>
        public void UpdateUser(User user,string password)
        {
            byte[] salt = passHasher.GenerateRandomSalt();
            string hashedPassword = passHasher.ComputeHash(password, salt);

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE UserLogin SET password = @password, salt = @salt, userRole = @role WHERE userID = @id;", conn);                    
                    cmd.Parameters.AddWithValue("@password", hashedPassword);
                    cmd.Parameters.AddWithValue("@salt", Convert.ToBase64String(salt));
                    cmd.Parameters.AddWithValue("@role", user.Role);
                    cmd.Parameters.AddWithValue("@id", user.UserId);

                    cmd.ExecuteNonQuery();

                    return;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        private User MapRowToUser(SqlDataReader reader)
        {
            return new User()
            {
                UserId = Convert.ToInt32(reader["userID"]),
                Name = Convert.ToString(reader["first_Last_Name"]),
                Username = Convert.ToString(reader["userName"]),
                Role = Convert.ToString(reader["userRole"])
            };
        }
        public bool CheckLogin(string userName, string password)
        {
            //Verifies that login info is correct
            bool loginPassed = false;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = @"select password, salt from UserLogin
                where userName = @userName";
                cmd.Parameters.AddWithValue("@userName", userName);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    string pulledPassword = (string)reader["password"];
                    string pulledSalt = (string)reader["salt"];
                    string hash = passHasher.ComputeHash(password, Convert.FromBase64String(pulledSalt));
                    loginPassed = hash.Equals(pulledPassword);
                }
            }
            return loginPassed;
        }
    }
}
