using SitecoreConfigurationValidator.Enums;

namespace SitecoreConfigurationValidator.Models
{
    public class ValidateRequest
    {
        public string FilePath { get; set; }
        public ConfigEditOptions EditOption { get; set; }
        public SearchOption SearchOption { get; set; }
        public string SitecoreRole { get; set; }
    }
}