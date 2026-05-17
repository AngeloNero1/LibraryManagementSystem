using System.Security.Cryptography;
using System.Text;

namespace LibraryManagementSystem.Services
{
    public class PasswordHelper
    {
        public string HashPassword(string password)
        {
            SHA256 sha256 = SHA256.Create();

            byte[] bytes = Encoding.UTF8.GetBytes(password);

            byte[] hash = sha256.ComputeHash(bytes);

            return Convert.ToHexString(hash);
        }
    }
}