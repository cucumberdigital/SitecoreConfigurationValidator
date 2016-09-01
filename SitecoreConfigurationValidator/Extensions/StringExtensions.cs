using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SitecoreConfigurationValidator.Extensions
{
    public static class StringExtensions
    {
        public static string EnsureEndsWith(this string str, char character)
        {
            if(string.IsNullOrWhiteSpace(str))return new string(new[] { character});

            if (str.EndsWith(new string(new[] {character}))) return str;

            return string.Concat(str, new string(new[] {character}));

        }
    }
}
