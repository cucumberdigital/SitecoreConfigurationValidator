using System;
using SitecoreConfigurationValidator.Enums;
using SitecoreConfigurationValidator.Models;

namespace SitecoreConfigurationValidator.Strategies
{
    public class NullFileEditStrategy : BaseFileEditStrategy, IFileEditStrategy
    {
        private readonly string _sitecoreWebRoot;
        public NullFileEditStrategy(string sitecoreWebRoot) : base(sitecoreWebRoot)
        {
            _sitecoreWebRoot = sitecoreWebRoot;
        }

        public ValidateFileResponse Process(string relativeFilePath, SearchOption searchOption)
        {
            return LogResult(ValidateFileResult.NotProcessed,
                string.Format("The file {0} did not need to be modified", relativeFilePath),
                relativeFilePath);
            
        }
    }
}