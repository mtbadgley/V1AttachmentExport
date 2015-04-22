using System;
using System.Configuration;
using System.Xml;
using NLog;

namespace V1AttachmentExportCore
{
    public class ProcessV1Attachments 
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static readonly V1AttachmentExportConfiguration Config =
            (V1AttachmentExportConfiguration)ConfigurationManager.GetSection("V1AttachmentExport");

        public static int DownloadAttachmentsByAssetType(string assettype)
        {
            int numdownloaded = 0;
            string getattachmentsstr = GetAttachments(assettype);
            if (getattachmentsstr.Contains("Attachment:"))
            {
                var getattachmentxml = new XmlDocument();
                getattachmentxml.LoadXml(getattachmentsstr);
                XmlNodeList attachments = getattachmentxml.SelectNodes("Assets/Asset");
                if (attachments != null)
                {
                    foreach (XmlNode attachment in attachments)
                    {
                        string oid = GetAttributeValue(attachment, "@id");
                        string filename = GetNodeText(attachment, @"Attribute[@name=""Filename""]");
                        string assetnumber = GetNodeText(attachment,
                            @"Attribute[@name=""Asset:" + assettype + @".Number""]");
                        string scopename = GetNodeText(attachment,
                            @"Attribute[@name=""Asset:" + assettype + @".Scope.Name""]");

                        DownloadAttachment(RemoveAssetType(oid), assetnumber, filename, scopename);
                        numdownloaded++;
                    }
                }
            }
            return numdownloaded;
        }
        
        public static string GetAttachments(string assettype)
        {
            string url = Config.Settings.BaseUrl +
                         "/rest-1.v1/Data/Attachment?sel=Filename,Asset:" + assettype + ".Number," +
                         "Asset:" + assettype + ".Scope.Name&where=AssetState='64';Asset.AssetType='" + assettype + "'";
            string result = V1Services.MakeHttpRequest(url);
            if (!result.Contains("Attachment:"))
                result = String.Empty;
            return result;
        }

        public static bool DownloadAttachment(string attachmentid, string assetnumber, string filename, string scopename)
        {
            string fullpath = GetFullPath(scopename) + @"\\" + assetnumber + "_" + filename;
            string fileurl = Config.Settings.BaseUrl + @"/attachment.img/" + attachmentid;
            Logger.Info("Downloading :: {0}", fullpath);
            bool result = V1Services.DownloadFileFromUrl(fileurl, fullpath);
            return result;
        }



        #region XMLHelpers
        private static string GetAttributeValue(XmlNode requestnode, string xpathstring)
        {
            string result = String.Empty;
            XmlNode selectSingleNode = requestnode.SelectSingleNode(xpathstring);
            if (selectSingleNode != null)
                result = selectSingleNode.Value;
            return result;
        }
        private static string GetNodeText(XmlNode requestnode, string xpathstring)
        {
            string result = String.Empty;
            XmlNode selectSingleNode = requestnode.SelectSingleNode(xpathstring);
            if (selectSingleNode != null)
                result = selectSingleNode.InnerText;
            return result;
        }
        #endregion XMLHelpers

        #region HelperFunctions
        public static string RemoveAssetType(string fulloid)
        {
            string[] oidparts = fulloid.Split(Convert.ToChar(":"));
            string oid = oidparts[1];
            return oid;
        }

        public static string GetFullPath(string scopename)
        {
            string result;
            if (Config.Settings.UseProject.ToUpper() == "TRUE")
            {
                result = Config.Settings.FilePath + @"\\" + RemoveSpecialCharacters(scopename);
            }
            else
            {
                result = Config.Settings.FilePath;
            }
            if (!System.IO.Directory.Exists(result))
            {
                System.IO.Directory.CreateDirectory(result);
            }
            return result;
        }

        public static string RemoveSpecialCharacters(string str)
        {
            var buffer = new char[str.Length];
            int idx = 0;
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z')
                    || (c >= 'a' && c <= 'z') || (c == '.') || (c == '_'))
                {
                    buffer[idx] = c;
                    idx++;
                }
            }
            return new string(buffer, 0, idx);
        }
        #endregion HelperFunctions
    }
}
