using System.Configuration;

namespace SitecoreConfigurationValidator.Xml
{
    public class SpreadsheetExclusionsSection : ConfigurationSection
    {
        [ConfigurationProperty("exclusions", IsRequired = true)]
        public SpreadsheetExclusionsCollection Exclusions
        {
            get
            {
                return base["exclusions"] as SpreadsheetExclusionsCollection;
            }
        }
    }
}