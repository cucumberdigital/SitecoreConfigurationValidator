using SitecoreConfigurationValidator.Enums;

namespace SitecoreConfigurationValidator.Extensions
{
    public static class ConfigEditOptionExtensions
    {
        public static string ToFileExtension(this ConfigEditOptions option)
        {
            if (option == ConfigEditOptions.NA || option == ConfigEditOptions.TBC) return string.Empty;

            if (option == ConfigEditOptions.Enable) return "config";

            return "disabled";
        }
    }
}