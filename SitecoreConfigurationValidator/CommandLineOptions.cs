using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;
using SitecoreConfigurationValidator.Enums;

namespace SitecoreConfigurationValidator
{
    public class CommandLineOptions
    {
        [Option('i', "input", Required = true, HelpText = "Input file to read.")]
        public string InputFile { get; set; }

        [Option('w', "websiteroot", Required = true, HelpText = "Sitecore Website Root")]
        public string WebsiteRootPath { get; set; }

        //TODO Convert to Enum
        [Option('r', "role", HelpText = "Targetted Sitecore Environment Role being validated against")]
        public string SitecoreRole { get; set; }

        [Option('s', "search option", HelpText = "The Search option to use, values are Lucene or Solr", DefaultValue = SearchOption.Lucene)]
        public SearchOption SearchOption { get; set; }

        [Option('v', null, HelpText = "Print details during execution.")]
        public bool Verbose { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            // this without using CommandLine.Text
            //  or using HelpText.AutoBuild
            var usage = new StringBuilder();
            var help = HelpText.AutoBuild(this);
            usage.AppendLine(help.ToString());
            return usage.ToString();
        }
    }
}
