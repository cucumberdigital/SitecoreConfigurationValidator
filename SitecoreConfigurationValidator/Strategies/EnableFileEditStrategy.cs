using System;
using System.IO;
using System.Linq;
using SitecoreConfigurationValidator.Enums;
using SitecoreConfigurationValidator.Extensions;
using SitecoreConfigurationValidator.Models;
using SearchOption = SitecoreConfigurationValidator.Enums.SearchOption;

namespace SitecoreConfigurationValidator.Strategies
{
    public class EnableFileEditStrategy : BaseFileEditStrategy, IFileEditStrategy
    {
        private readonly string _sitecoreWebRoot;

        public EnableFileEditStrategy(string sitecoreWebRoot) : base(sitecoreWebRoot)
        {
            _sitecoreWebRoot = sitecoreWebRoot;
        }

        public ValidateFileResponse Process(string relativeFilePath, SearchOption searchOption)
        {
            var fullFilePath = string.Concat(_sitecoreWebRoot, relativeFilePath);
            var extensionToBe = "." + ConfigEditOptions.Enable.ToFileExtension();

            if (!FindFileOnDisk(ref fullFilePath))
            {
                return LogResult(ValidateFileResult.FileNotFound, string.Format("Path to file " + fullFilePath + " does not exist"), relativeFilePath);
            }

            var file = new FileInfo(fullFilePath);
            var currentExtension = file.Extension;

            if (IsExcludedExtension(currentExtension) || string.Equals(currentExtension, extensionToBe))
            {
                return LogResult(ValidateFileResult.Success, string.Format("The file {0} has a valid extension of {1}", relativeFilePath, extensionToBe), relativeFilePath);
            }

            try
            {
                file.CopyTo(fullFilePath.Replace(file.Extension, string.Empty));
                file.Delete();
                
                return LogResult(ValidateFileResult.Success, string.Format("File {0} renamed from to remove .disabled extension", fullFilePath), relativeFilePath);
            }
            catch (Exception ex)
            {
                return LogResult(ValidateFileResult.Failed, string.Format("An error occurred editing the file {0} to the extension {1} - {2}",
                    relativeFilePath, extensionToBe, ex.Message), relativeFilePath);
            }

            return LogResult(ValidateFileResult.Failed, "Fallen trough", relativeFilePath);
        }
        
        private bool FindFileOnDisk(ref string fullFilePath)
        {
            if (!File.Exists(fullFilePath))
            {
                var fileFound = CheckDisabledFileIsNowEnabled(ref fullFilePath);
                if (!fileFound)
                {
                    fileFound = CheckEnabledFileIsNowDisabled(ref fullFilePath);
                    if (!fileFound)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private bool CheckEnabledFileIsNowDisabled(ref string fullFilePath)
        {
            if (!fullFilePath.Contains(ConfigEditOptions.Disable.ToFileExtension()))
            {
                fullFilePath = string.Concat(fullFilePath, ".", ConfigEditOptions.Disable.ToFileExtension());
            }

            return File.Exists(fullFilePath);
        }

        private bool CheckDisabledFileIsNowEnabled(ref string fullFilePath)
        {
            //By default file is disabled, but is now enabled
            if (fullFilePath.Contains(ConfigEditOptions.Disable.ToFileExtension()))
            {
                fullFilePath = fullFilePath.Replace("." + ConfigEditOptions.Disable.ToFileExtension(), "");
            }

            return File.Exists(fullFilePath);
        }
    }
}