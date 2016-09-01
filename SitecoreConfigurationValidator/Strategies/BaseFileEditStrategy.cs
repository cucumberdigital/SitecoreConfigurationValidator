using System;
using System.Linq;
using SitecoreConfigurationValidator.Enums;
using SitecoreConfigurationValidator.Models;

namespace SitecoreConfigurationValidator.Strategies
{
    public abstract class BaseFileEditStrategy
    {
        private readonly string _sitecoreWebRoot;
        protected BaseFileEditStrategy(string sitecoreWebRoot)
        {
            _sitecoreWebRoot = sitecoreWebRoot;
        }

        protected bool IsExcludedExtension(string currentExtension)
        {
            string[] excludes = { ".example" };
            return excludes.Contains(currentExtension);
        }

        protected ValidateFileResponse LogResult(ValidateFileResult result, string message, string relativeFilePath)
        {
            var response = new ValidateFileResponse();
            switch (result)
            {
                case ValidateFileResult.Failed:
                    Logger.Error(message);
                    break;
                case ValidateFileResult.FileNotFound:
                    Logger.Warning(message);
                    break;
                case ValidateFileResult.Success:
                    Logger.Success(message);
                    break;
                case ValidateFileResult.NotProcessed:
                    Logger.Info(message);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(result), result, null);
            }
            response.Result = result;
            response.Message = message;
            response.FileName = relativeFilePath;
            return response;
        }
    }
}
