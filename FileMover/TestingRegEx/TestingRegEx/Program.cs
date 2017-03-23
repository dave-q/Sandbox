using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TestingRegEx
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {

                Console.WriteLine("Enter string or EXIT to exit");
                var stringToCheck = Console.ReadLine();
                if (stringToCheck.ToUpper() == "EXIT")
                {
                    break;
                }

                var regString = @"(?:(^([a-zA-Z0-9](?:(?:[a-zA-Z0-9-]*|(?<!-)\.(?![-.]))*[a-zA-Z0-9]+)?))([\\])([[:alnum:]]\w+$))";

                var isMatch = Regex.IsMatch(stringToCheck, regString);

                Console.WriteLine("Is MATCH: {0}", isMatch);
            }


        }
    }
}
