using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace SitecoreConfigurationValidator.Xml
{
    [ConfigurationCollection(typeof(SpreadsheetExclusionElement), AddItemName = "exclude")]
    public class SpreadsheetExclusionsCollection : ConfigurationElementCollection, IEnumerable<SpreadsheetExclusionElement>
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new SpreadsheetExclusionElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            var configElement = element as SpreadsheetExclusionElement;
            return configElement != null ? configElement.SitecoreRole + configElement.FileName : null;
        }

        public SpreadsheetExclusionElement this[int index]
        {
            get
            {
                return BaseGet(index) as SpreadsheetExclusionElement;
            }
        }

        #region IEnumerable<FeatureElement> Members

        IEnumerator<SpreadsheetExclusionElement> IEnumerable<SpreadsheetExclusionElement>.GetEnumerator()
        {
            return (from i in Enumerable.Range(0, this.Count)
                select this[i])
                .GetEnumerator();
        }

        #endregion
    }
}