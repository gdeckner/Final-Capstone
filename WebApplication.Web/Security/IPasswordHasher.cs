using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication.Web.Security
{
    public interface IPasswordHasher
    {
        //Takes the password and generated salt then converts it to a hash for the SQL password
        string ComputeHash(string password, byte[] salt);

        //Generates a random number then converts it to an array of bytes to be returned for the hasher
        byte[] GenerateRandomSalt();
    }
}
