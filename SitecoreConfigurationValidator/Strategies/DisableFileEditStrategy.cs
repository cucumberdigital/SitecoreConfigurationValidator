using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using SitecoreConfigurationValidator.Enums;
using SitecoreConfigurationValidator.Extensions;
using SitecoreConfigurationValidator.Models;
using SearchOption = SitecoreConfigurationValidator.Enums.SearchOption;

namespace SitecoreConfigurationValidator.Strategies
{
    public class DisableFileEditStrategy : BaseFileEditStrategy, IFileEditStrategy
    {
        private readonly string _sitecoreWebRoot;

        public DisableFileEditStrategy(string sitecoreWebRoot) : base(sitecoreWebRoot)
        {
            _sitecoreWebRoot = sitecoreWebRoot;
        }

        public ValidateFileResponse Process(string relativeFilePath, SearchOption searchOption)
        {
            var response = new ValidateFileResponse();
            var message = string.Empty;

            var fullFilePath = string.Format(@"{0}{1}", _sitecoreWebRoot, relativeFilePath);
            var extensionToBe = string.Format(".{0}", ConfigEditOptions.Disable.ToFileExtension());

            if (!FindFileOnDisk(ref fullFilePath, extensionToBe))
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
                var indexStr = ConfigEditOptions.Enable.ToFileExtension();
                var strIndexEnd = fullFilePath.IndexOf(indexStr) + indexStr.Length;
                var configBasedFilePath = fullFilePath.Substring(0, strIndexEnd);

                file.CopyTo(string.Concat(configBasedFilePath, extensionToBe));
                file.Delete();

                return LogResult(ValidateFileResult.Success, string.Format("File {0} renamed from to add .disabled extension", file.FullName), relativeFilePath);
            }
            catch (Exception ex)
            {
                return LogResult(ValidateFileResult.Failed, string.Format("An error occurred editing the file {0} to the extension {1} - {2}",
                    relativeFilePath, extensionToBe, ex.Message), relativeFilePath);
            }

            return LogResult(ValidateFileResult.Failed, "Fallen trough", relativeFilePath);
        }
        
        private bool FindFileOnDisk(ref string fullFilePath, string extensionToBe)
        {
            if (!File.Exists(fullFilePath))
            {
                var fileFound = CheckDisabledFileIsNowEnabled(ref fullFilePath);
                if (!fileFound)
                {
                    fileFound = CheckEnabledFileIsNowDisabled(ref fullFilePath, extensionToBe);
                    if (!fileFound)
                    {
                        return false;
                    }
                }
            }
            return true;
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

        private bool CheckEnabledFileIsNowDisabled(ref string fullFilePath, string extensionToBe)
        {
            var tempPath = fullFilePath + extensionToBe;

            if (File.Exists(tempPath))
            {
                fullFilePath = tempPath;
                return true;
            }
            return false;

        }
    }
}