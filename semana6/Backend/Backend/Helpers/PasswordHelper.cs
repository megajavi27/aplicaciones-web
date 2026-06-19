using System.Security.Cryptography;
using System.Text;

namespace Backend.Helpers
{
    public static class PasswordHelper
    {
        public static string Hash(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return string.Concat(bytes.Select(b => b.ToString("x2")));
        }
    }
}
