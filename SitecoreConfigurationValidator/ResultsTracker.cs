using System;
using System.Collections.Generic;
using System.Linq;
using SitecoreConfigurationValidator.Enums;
using SitecoreConfigurationValidator.Models;

namespace SitecoreConfigurationValidator
{
    public class ResultsTracker
    {
        private readonly CommandLineOptions _commandLineOptions;
        private readonly List<ValidateFileResponse> _responses;

        public ResultsTracker(CommandLineOptions commandLineOptions)
        {
            _commandLineOptions = commandLineOptions;
            _responses = new List<ValidateFileResponse>();
        }

        public void Add(ValidateFileResponse result)
        {
            _responses.Add(result);

        }

        public void DisplayResults()
        {
            Console.WriteLine();
            Console.WriteLine();
            Logger.Info("*******RESULTS*******");
            Logger.Info("Validated against Role: " + _commandLineOptions.SitecoreRole);
            Logger.Info("Total spreadsheets rows validated: " + _responses.Count);

            Logger.Info("Not Processed:" + _responses.Count(x => x.Result == ValidateFileResult.NotProcessed));
            Logger.Info("Succeeded:" + _responses.Count(x => x.Result == ValidateFileResult.Success));
            Console.WriteLine();

            var notFounds = _responses.Where(x => x.Result == ValidateFileResult.FileNotFound);
            Logger.Warning("File Not Found:" + notFounds.Count());
            foreach (var notFound in notFounds)
            {
                Logger.Info(notFound.FileName + " - " + notFound.FileName);
            }

            Console.WriteLine();

            var faileds = _responses.Where(x => x.Result == ValidateFileResult.Failed);
            Logger.Info("Failed:" + faileds.Count());
            foreach (var fail in faileds)
            {
                Logger.Info(fail.FileName + " - " + fail.FileName);
            }

            
        }
    }
}