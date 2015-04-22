# V1AttachmentExport

A utility one can use to export attachments from VersionOne to their local hard disk.  This is not a supported utility by VersionOne nor Matt Badgley.  Use at your own peril.

## Usage
If you don't feel like download the source code and compiling yourself, you can grab the contents of the /deploy directory and place anywhere on your local hard disk.  From there, you'll need to edit the V1AttachmentExport.exe.config file.  This is a standard XML configuration file.  Only edit the V1AttachmentExport section.  The instructions/settings are detailed in the file.

If you plan on using OAuth, you will need to use the GrantTool.  See [https://github.com/versionone/OAuth2Client.Net](https://github.com/versionone/OAuth2Client.Net) to learn how to use.

## DISCLAIMER

The licenses for VersionOne is covered through your agreement with [VersionOne](http://www.versionone.com).  As for this work, it's not guaranteed and made as an example utility to allow you to extract attachments from VersionOne for future reference.
