using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace SitecoreConfigurationValidator.Xml
{
    [ConfigurationCollection(typeof(SitecoreRoleElement), AddItemName = "role")]
    public class SitecoreRolesCollection : ConfigurationElementCollection, IEnumerable<SitecoreRoleElement>
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new SitecoreRoleElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            var configElement = element as SitecoreRoleElement;
            return configElement != null ? configElement.Name : null;
        }

        public SitecoreRoleElement this[int index]
        {
            get
            {
                return BaseGet(index) as SitecoreRoleElement;
            }
        }

        #region IEnumerable<FeatureElement> Members

        IEnumerator<SitecoreRoleElement> IEnumerable<SitecoreRoleElement>.GetEnumerator()
        {
            return (from i in Enumerable.Range(0, this.Count)
                    select this[i])
                    .GetEnumerator();
        }

        #endregion
    }
}