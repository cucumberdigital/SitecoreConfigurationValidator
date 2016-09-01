using System;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SitecoreConfigurationValidator.Xml
{
    public class SitecoreRoleSection : ConfigurationSection
    {
        [ConfigurationProperty("roles", IsRequired = true)]
        public SitecoreRolesCollection Roles
        {
            get
            {
                return base["roles"] as SitecoreRolesCollection;
            }
        }
    }
}

