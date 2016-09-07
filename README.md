# SitecoreConfigurationValidator
A tool designed to automate the updating of Sitecore server role configuration, based on the Config Enable Disable Spreadsheet, provided by Sitecore.

### Usage
For a CD server:
SitecoreConfigurationValidator.exe -i {path-to-spreadsheet} -w {path-to-folder-above-website-root} -r "Content Delivery (CD)" -s {lucene-or-solr} -v

For a CM server:
SitecoreConfigurationValidator.exe -i {path-to-spreadsheet} -w {path-to-folder-above-website-root} -r "Content Management (CM)" -s {lucene-or-solr} -v
