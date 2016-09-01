using System;
using System.Configuration;
using SitecoreConfigurationValidator.Enums;

namespace SitecoreConfigurationValidator.Xml
{
    public class SpreadsheetExclusionElement : ConfigurationElement
    {
        [ConfigurationProperty("sitecoreRole", IsRequired = true)]
        public string SitecoreRole
        {
            get
            {
                return (string)this["sitecoreRole"];
            }
            set
            {
                this["sitecoreRole"] = value;
            }
        }

        [ConfigurationProperty("action", IsRequired = true)]
        public ConfigEditOptions Action
        {
            get
            {
                return (ConfigEditOptions)this["action"];
            }
            set
            {
                this["action"] = value;
            }
        }

        [ConfigurationProperty("fileName", IsRequired = true)]
        public string FileName
        {
            get
            {
                return (string)this["fileName"];
            }
            set
            {
                this["fileName"] = value;
            }
        }
    }
}