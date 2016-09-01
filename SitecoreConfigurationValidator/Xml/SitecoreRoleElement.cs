using System.Configuration;

namespace SitecoreConfigurationValidator.Xml
{
    public class SitecoreRoleElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }

        [ConfigurationProperty("columnIndex", IsRequired = true)]
        [IntegerValidator(MaxValue = 20)]
        public int ColumnIndex
        {
            get
            {
                return (int)this["columnIndex"];
            }
            set
            {
                this["columnIndex"] = (int)value;
            }
        }

        
    }

}