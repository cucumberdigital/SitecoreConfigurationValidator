using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SitecoreConfigurationValidator
{
    public static class Logger
    {
        public static void Success(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void Warning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void Info(string message)
        {
            if (_verbose)
            {
                Console.WriteLine(message);
            }
        }

        public static void Initialize(bool verbose)
        {
            _verbose = verbose;
        }

        private static bool _verbose = false;

       
    }
}
