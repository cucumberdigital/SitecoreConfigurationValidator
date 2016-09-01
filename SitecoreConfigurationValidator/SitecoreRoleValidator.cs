using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SitecoreConfigurationValidator.Enums;
using SitecoreConfigurationValidator.Extensions;
using SitecoreConfigurationValidator.Models;

namespace SitecoreConfigurationValidator
{
    public class SitecoreRoleValidator
    {
        private readonly ExcelFileProcessor _processor;
        private readonly SitecoreConfigManager _sitecoreConfigManager;
        private readonly CommandLineOptions _commandLineOptions;


        public SitecoreRoleValidator(CommandLineOptions commandLineOptions)
        {
            this._commandLineOptions = commandLineOptions;
            _processor = new ExcelFileProcessor(_commandLineOptions.InputFile);
            _sitecoreConfigManager = new SitecoreConfigManager(_commandLineOptions.WebsiteRootPath);
        }

        public void Validate()
        {
            if (!SelectedSitecoreRoleExistsInConfig())
            {
                Logger.Error("Unable to validate against " + _commandLineOptions.SitecoreRole + ", doesn't exist in App Setting Sitecore Roles");
                return;
            }

            Logger.Info("Validating config against " + _commandLineOptions.SitecoreRole);


            if (_processor.ConfigRows == null || !_processor.ConfigRows.Any())
            {
                Logger.Error("Unable to read Configuration for any rows for " + _commandLineOptions.SitecoreRole);
                return;
            }

            var selectedRoleColumnIndex = _processor.GetSelectedRoleColumnIndex(_commandLineOptions.SitecoreRole);
            //TODO Initialize and track statistics to report to output

            var tracker = new ResultsTracker(_commandLineOptions);
            foreach (var row in _processor.ConfigRows)
            {
                var file = string.Format("{0}{1}", row.FilePath.EnsureEndsWith('\\'), row.FileName);

                try
                {
                    var rowEditOption = GetEditOption(row, selectedRoleColumnIndex);
                    var request = CreateValidationRequest(file, rowEditOption);
                    
                    Logger.Info("Validating " + row.FileName);

                    var result = _sitecoreConfigManager.ValidateFile(request);
                    tracker.Add(result);
                }
                catch (Exception ex)
                {
                    Logger.Error("Error - " + ex.Message);
                    throw;
                }

            }

            tracker.DisplayResults();
        }

        private ValidateRequest CreateValidationRequest(string file, ConfigEditOptions editOption)
        {
            var request = new ValidateRequest()
            {
                FilePath = file.Trim(),
                EditOption = editOption,
                SearchOption = _commandLineOptions.SearchOption,
                SitecoreRole = _commandLineOptions.SitecoreRole
            };
            return request;
        }

        private ConfigEditOptions GetEditOption(ConfigRow row, int selectedRoleColumnIndex)
        {
            var cellValue = _processor.GetCellValue(row.RowIndex, selectedRoleColumnIndex);
            switch (cellValue)
            {
                case "Enable":
                    return ConfigEditOptions.Enable;
                case "Disable":
                    return ConfigEditOptions.Disable;
                case "n/a":
                    return ConfigEditOptions.NA;
                case "tbc":
                    return ConfigEditOptions.TBC;
            }
            return ConfigEditOptions.NA;
        }

        private bool SelectedSitecoreRoleExistsInConfig()
        {
            return _processor.SitecoreRoles
                .Select(x => x.Name)
                .Any(x => x == _commandLineOptions.SitecoreRole);
        }

    }
}
