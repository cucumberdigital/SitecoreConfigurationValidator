using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel;
using SitecoreConfigurationValidator.Models;
using SitecoreConfigurationValidator.Xml;

namespace SitecoreConfigurationValidator
{
    public class ExcelFileProcessor
    {
        private IEnumerable<worksheet> _worksheets;
       
        public IEnumerable<SitecoreRoleElement> SitecoreRoles { get; private set; }

        private IEnumerable<ConfigRow> _configRows;
        public IEnumerable<ConfigRow> ConfigRows
        {
            get { return _configRows ?? (_configRows = PopulateConfigRows()); }
        }

        public ExcelFileProcessor(string filePath)
        {
            _worksheets = GetWorksheets(filePath);

            SitecoreRoles = GetConfigRoles();
            
            if (!ValidateHeaderRowContainsRoles())
            {
                throw new Exception("Invalid Header Role cofiguration");
            }
            
        }

        private int GetHeaderRowIndex()
        {
             return int.Parse(ConfigurationManager.AppSettings["Excel.HeaderRowIndex"]);
        }

        private IEnumerable<SitecoreRoleElement> GetConfigRoles()
        {
            var configSection = ConfigurationManager.GetSection("sitecoreRoles") as SitecoreRoleSection;
            return configSection != null ? configSection.Roles.AsEnumerable() : new List<SitecoreRoleElement>();
        }

        private IEnumerable<worksheet> GetWorksheets(string inputFilePath)
        {
            try
            {
                _worksheets = Workbook.Worksheets(inputFilePath);
                if (_worksheets == null || !_worksheets.Any())
                {
                    throw new Exception("No Worksheets could be found");
                }
                return _worksheets;
            }
            catch (System.IO.FileNotFoundException ex)
            {
                Logger.Error("Massive error with your file bro");
                Console.Read();
            }
            return null;
        }

        private bool ValidateHeaderRowContainsRoles()
        {
            var sheet = _worksheets.First();
            var headerRowIndex = GetHeaderRowIndex();
            var headerRowTestText = ConfigurationManager.AppSettings["Excel.HeaderRowTestText"];

            Logger.Info("Config Header Row index: " + headerRowIndex);
            var headerRow = sheet.Rows[headerRowIndex];

            var testRowIndex = 0;
            var productNameTextFound = false;
            
            Logger.Info("Testing to find the text \"" + headerRowTestText + "\" in the Excel file starting at Row index: " + testRowIndex);
            while (!productNameTextFound)
            {
                var testHeaderRow = sheet.Rows[testRowIndex];
                if (testHeaderRow.Cells == null)
                {
                    testRowIndex++;
                }
                else
                {
                    foreach (var testHeaderRowCell in testHeaderRow.Cells)
                    {
                        if (testHeaderRowCell == null) continue;
                        Logger.Info("Testing Header Row #" + testRowIndex + " Cell# " + testHeaderRowCell.ColumnIndex);

                        if (testHeaderRowCell.Text == headerRowTestText)
                        {
                            productNameTextFound = true;
                            Logger.Info("Text \"" + headerRowTestText + "\" found in  Row #" + testRowIndex + " Cell# " + testHeaderRowCell.ColumnIndex);
                            Logger.Warning("Please update App.config Excel.HeaderRowIndex setting to " + testRowIndex + " if an error is thrown");
                            break;
                        }
                    }

                    if (!productNameTextFound)
                    {
                        testRowIndex++;
                    }
                }
                
            }

            if (productNameTextFound)
            {
                headerRow = sheet.Rows[testRowIndex];
            }

            
            foreach (var role in SitecoreRoles)
            {
                var roleHeaderRowCell = headerRow.Cells[role.ColumnIndex];
                if (roleHeaderRowCell == null || roleHeaderRowCell.Text != role.Name)
                {

                    Logger.Error("Invalid Role configuration - " + role.Name + " at cell #" + role.ColumnIndex);
                    return false;
                }
                else
                {
                    Logger.Info("Role " + role.Name + " is valid");
                }
            }
            return true;
        }

        private IEnumerable<ConfigRow> PopulateConfigRows()
        {
            var headerRowIndex = GetHeaderRowIndex();
            var sheet = _worksheets.First();

            var rowIndex = headerRowIndex;
            var currentRow = sheet.Rows[rowIndex];
            var configRows = new List<ConfigRow>();

            while (rowIndex < (sheet.Rows.Count() - 1))
            {
                rowIndex++;
                currentRow = sheet.Rows[rowIndex];

                Logger.Info("Adding " + currentRow.Cells[3].Text + " from Row " + rowIndex);
                configRows.Add(new ConfigRow()
                {
                    FilePath = currentRow.Cells[2].Text,
                    FileName = currentRow.Cells[3].Text,
                    RowIndex = rowIndex
                });
                
            }

            return configRows;
        }

        public int GetSelectedRoleColumnIndex(string selectedRole)
        {
            var headerRowIndex = GetHeaderRowIndex();
            var sheet = _worksheets.First();

            var headerRow = sheet.Rows[headerRowIndex];
            var selectedRoleCell = headerRow.Cells.First(x => x != null && string.Equals(x.Text, selectedRole));

            return selectedRoleCell.ColumnIndex;
        }
        
        public string GetCellValue(int rowIndex, int columnIndex)
        {
            var sheet = _worksheets.First();
            return sheet.Rows[rowIndex].Cells[columnIndex].Text;
        }
    }
}
