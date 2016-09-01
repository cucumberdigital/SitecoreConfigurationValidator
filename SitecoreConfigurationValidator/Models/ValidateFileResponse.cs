using SitecoreConfigurationValidator.Enums;

namespace SitecoreConfigurationValidator.Models
{
    public class ValidateFileResponse
    {
        public string Message { get; set; }
        public string FileName { get; set; }
        public ValidateFileResult Result { get; set; }
    }
}