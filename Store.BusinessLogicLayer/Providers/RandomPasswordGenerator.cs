using Microsoft.AspNetCore.Identity;
using Store.BusinessLogicLayer.Providers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.BusinessLogicLayer.Providers
{
    public class RandomPasswordGenerator : IRandomPasswordGenerator
    {
        PasswordOptions opts;
        public RandomPasswordGenerator()
        {
            
        }
        public string GenerateRandomPassword()
        {
            string[] randomChars = new[] {
            "ABCDEFGHJKLMNOPQRSTUVWXYZ",
            "abcdefghijkmnopqrstuvwxyz",
            "0123456789",
            "!@$?_-"};

            Random rand = new Random();
            List<char> chars = new List<char>();


            chars.Insert(rand.Next(0, chars.Count),
            randomChars[0][rand.Next(0, randomChars[0].Length)]);

            chars.Insert(rand.Next(0, chars.Count),
            randomChars[1][rand.Next(0, randomChars[1].Length)]);

            chars.Insert(rand.Next(0, chars.Count),
            randomChars[2][rand.Next(0, randomChars[2].Length)]);

            chars.Insert(rand.Next(0, chars.Count),
            randomChars[3][rand.Next(0, randomChars[3].Length)]);

            int passwordLenghts = rand.Next(9, 20);

            for (int i = chars.Count; i <= passwordLenghts; i++)
            {
                int position = rand.Next(0, 4);
                chars.Insert(rand.Next(0, chars.Count),
                randomChars[position][rand.Next(0, randomChars[position].Length)]);
            }

            int to = rand.Next(30, 100);

            for (int i = 0; i <= to; i++)
            {
                int first = rand.Next(0, 7);
                int second = rand.Next(0, 7);

                char a = chars[first];
                char b = chars[second];
                char tmp = b;

                chars[second] = a;
                chars[first] = tmp;
            }

            string password = string.Join("", chars);

            Console.WriteLine(password);

            return password;
        }
    }
}
