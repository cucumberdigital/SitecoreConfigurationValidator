# SitecoreConfigurationValidator
A tool designed to automate the updating of Sitecore server role configuration, based on the Config Enable Disable Spreadsheet, provided by Sitecore. This tool will modify the files in a base Sitecore install to match the Enabled or Disabled extensions defined in the Sitecore spreadsheet, based on the input variables defined.

###Options

* -i
: Description: the physical path to the spreadsheet, provided by Sitecore
Required
* -w
Description: the physical path to the Website Root folder on the Sitecore base files
Required
* -r
Description: the Sitecore server role you would to validate the Sitecore base files against
Required
* -s
Description: the Search Provider that you are using
Options: 'lucene' or 'solr'
Default: 'lucene'
* -v
Description: Verbose logging
Note: Soon to be obsolete

### Usage
For a CD server:

SitecoreConfigurationValidator.exe -i {path-to-spreadsheet} -w {path-to-folder-above-website-root} -r "Content Delivery (CD)" -s {lucene-or-solr} -v

For a CM server:

SitecoreConfigurationValidator.exe -i {path-to-spreadsheet} -w {path-to-folder-above-website-root} -r "Content Management (CM)" -s {lucene-or-solr} -v

###App Settings

sitecoreRoles
Used to determine the Sitecore server roles, defined in the columns of the spreadsheet, that the application can use to validate. Created as an extension point if the spreadsheet adds more roles over time
exclusions
Sometimes there are instances where the spreadsheet may be incorrect, or where you may like to make changes to the way a role is configure in the spreadsheet. Exclusions enable you to define, per role, individual files that youd like to be validated differently to that defined in the spreadsheet
Excel.HeaderRowIndex
Used to determine the header row that contains the column heading for the relative Sitecore Roles (going to be refactored shortly to be determined in a programmatic manner)
