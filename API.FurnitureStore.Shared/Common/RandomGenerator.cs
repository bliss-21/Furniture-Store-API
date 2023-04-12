using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace API.FurnitureStore.Shared.Common
{
    public static class RandomGenerator
    {
        public static string GenerateRandomString(int size) 
        {
            var random = new Random();
            var chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ$#-_.";

            return new string(Enumerable.Repeat(chars, size).Select(x => x[random.Next(x.Length)]).ToArray());
        }
    }
}
