using System;

namespace SitecoreConfigurationValidator
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new CommandLineOptions();
            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                Logger.Initialize(options.Verbose);
                
                Logger.Info(options.InputFile);
                Logger.Info(options.WebsiteRootPath);
                Logger.Info("Search option: " + options.SearchOption);
                

                var validator = new SitecoreRoleValidator(options);
                validator.Validate();
            }

            Console.ReadLine();
        }
    }
}
