using SitecoreConfigurationValidator.Enums;
using SitecoreConfigurationValidator.Models;

namespace SitecoreConfigurationValidator.Strategies
{
    public interface IFileEditStrategy
    {
        ValidateFileResponse Process(string relativeFilePath, SearchOption searchOption);
    }
}