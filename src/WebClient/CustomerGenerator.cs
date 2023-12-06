using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebClient
{
    class  CustomerGenerator
    {

        private static readonly string[] firstNames = { "Alexander", "Helen", "George", "Paul", "Julia" };
        private static readonly string[] lastNames = { "Adams", "Ford", "Green", "Black", "Scott" };

        public static Customer Generator()
        {

            Random random = new Random();
            string firstName = firstNames[random.Next(0, firstNames.Length)];
            string lastName = lastNames[random.Next(0, lastNames.Length)];
            int i = random.Next(0, 20);

            return new Customer { Id = i, Firstname = firstName, Lastname = lastName };
      
        }

    }
}
