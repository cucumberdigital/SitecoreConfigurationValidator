using System;
using System.IO;
using SitecoreConfigurationValidator.Extensions;
using SitecoreConfigurationValidator.Models;

namespace SitecoreConfigurationValidator
{
    public class SitecoreConfigManager
    {
        private readonly string _sitecoreWebRoot;
        private readonly FileEditStrategyFactory _factory;

        public SitecoreConfigManager(string sitecoreWebRoot)
        {
            _sitecoreWebRoot = sitecoreWebRoot;
            _factory = new FileEditStrategyFactory();

            
        }

        public ValidateFileResponse ValidateFile(ValidateRequest request)
        {
            var editStrategy = _factory.Create(request, _sitecoreWebRoot);

            return editStrategy.Process(request.FilePath, request.SearchOption);
          
        }
    }
}