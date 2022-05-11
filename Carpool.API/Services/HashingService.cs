using System.Security.Cryptography;
using System.Text;

namespace Carpool.API.Services
{
    public class HashingService
    {
        public string GetHash(string value)
        {
            var data = Encoding.ASCII.GetBytes(value);
            var hashData = new SHA1Managed().ComputeHash(data);
            var hash = string.Empty;
            int counter = 0;
            foreach (var b in hashData)
            {
                if (counter == 4)
                {
                    hash += "-";
                    counter = 0;
                }
                hash += b.ToString("X2");
                counter += 1;
            }
            return hash;
        }
    }
}