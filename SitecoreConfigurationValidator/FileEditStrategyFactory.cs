using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using SitecoreConfigurationValidator.Enums;
using SitecoreConfigurationValidator.Models;
using SitecoreConfigurationValidator.Strategies;
using SitecoreConfigurationValidator.Xml;

namespace SitecoreConfigurationValidator
{
    public class FileEditStrategyFactory
    {
        private IEnumerable<SpreadsheetExclusionElement> _exclusions;

        public IEnumerable<SpreadsheetExclusionElement> Exclusions
        {
            get
            {
                return _exclusions ?? (_exclusions = GetConfigExclusions());
            }
        }


        public IFileEditStrategy Create(ValidateRequest request, string sitecoreWebRoot)
        {
            if (FileIsExcluded(request))
            {
                return GetExcludedFileEditStrategy(request, sitecoreWebRoot);
            }

            if (!IsValidSearchOption(request.SearchOption, request.FilePath))
            {
                return new DisableFileEditStrategy(sitecoreWebRoot);
            }

            switch (request.EditOption)
            {
                case ConfigEditOptions.NA:
                    return new NullFileEditStrategy(sitecoreWebRoot);
                case ConfigEditOptions.Disable:
                    return new DisableFileEditStrategy(sitecoreWebRoot);
                case ConfigEditOptions.Enable:
                    return new EnableFileEditStrategy(sitecoreWebRoot);
                case ConfigEditOptions.TBC:
                    return new NullFileEditStrategy(sitecoreWebRoot);
                default:
                    throw new ArgumentOutOfRangeException(nameof(request.EditOption), request.EditOption, null);
            }
        }

        private IFileEditStrategy GetExcludedFileEditStrategy(ValidateRequest request, string sitecoreWebRoot)
        {
            var exclusion =
                Exclusions.First(x => x.SitecoreRole == request.SitecoreRole && request.FilePath.Contains(x.FileName));

            if (exclusion == null)
            {
                return new NullFileEditStrategy(sitecoreWebRoot);
            }

            Logger.Info("***FILE EXLUSION*** - " + request.FilePath);

            switch (exclusion.Action)
            {
                case ConfigEditOptions.NA:
                    return new NullFileEditStrategy(sitecoreWebRoot);
                case ConfigEditOptions.Disable:
                    return new DisableFileEditStrategy(sitecoreWebRoot);
                case ConfigEditOptions.Enable:
                    return new EnableFileEditStrategy(sitecoreWebRoot);
                case ConfigEditOptions.TBC:
                    return new NullFileEditStrategy(sitecoreWebRoot);
                default:
                    throw new ArgumentOutOfRangeException(nameof(request.EditOption), request.EditOption, null);
            }
        }

        private bool FileIsExcluded(ValidateRequest request)
        {
            var roleExclusions = Exclusions.Where(x => x.SitecoreRole == request.SitecoreRole);
            if (!roleExclusions.Any()) return false;

            return roleExclusions.Any(x => request.FilePath.Contains(x.FileName));
        }

        private bool IsValidSearchOption(SearchOption searchOption, string fullFilePath)
        {
            if (!FilePathContainsASearchOption(fullFilePath))
            {
                return true;
            }

            return fullFilePath.ToLower().Contains(searchOption.ToString().ToLower());
        }

        private bool FilePathContainsASearchOption(string fullFilePath)
        {
            var lowered = fullFilePath.ToLower();
            return Enum.GetNames(typeof (SearchOption)).Any(searchOption => lowered.Contains(searchOption.ToLower()));
        }

        private IEnumerable<SpreadsheetExclusionElement> GetConfigExclusions()
        {
            var configSection = ConfigurationManager.GetSection("spreadsheetExclusions") as SpreadsheetExclusionsSection;
            return configSection != null ? configSection.Exclusions.AsEnumerable() : new List<SpreadsheetExclusionElement>();
        }
    }
}
