using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace V1AttachmentExportCore
{
    public class V1AttachmentExportConfiguration
    {
        public SettingsInfo Settings = new SettingsInfo();

        public V1AttachmentExportConfiguration(XmlNode section)
        {
            XDocument xmlDoc = XDocument.Parse(section.OuterXml);

            IEnumerable<SettingsInfo> settings = xmlDoc.Descendants("Settings").Select(item => new SettingsInfo
            {
                OauthScope =
                    string.IsNullOrEmpty(item.Element("OauthScope").Value)
                        ? string.Empty
                        : item.Element("OauthScope").Value,
                BaseUrl = 
                    string.IsNullOrEmpty(item.Element("BaseUrl").Value)
                        ? string.Empty
                        : item.Element("BaseUrl").Value,
                FilePath =
                    string.IsNullOrEmpty(item.Element("FilePath").Value)
                        ? string.Empty
                        : item.Element("FilePath").Value,
                UseProject = 
                    string.IsNullOrEmpty(item.Element("UseProject").Value)
                        ? string.Empty
                        : item.Element("UseProject").Value,
                AssetTypes =
                    string.IsNullOrEmpty(item.Element("AssetTypes").Value)
                        ? string.Empty
                        : item.Element("AssetTypes").Value,
                V1Username = 
                    string.IsNullOrEmpty(item.Element("V1Username").Value)
                        ? string.Empty
                        : item.Element("V1Username").Value,
                V1Password = 
                    string.IsNullOrEmpty(item.Element("V1Password").Value)
                        ? string.Empty
                        : item.Element("V1Password").Value,
                AuthType =
                    string.IsNullOrEmpty(item.Element("AuthType").Value)
                        ? string.Empty
                        : item.Element("AuthType").Value
            });
            if (!settings.Any())
            {
                throw new ConfigurationErrorsException("Settings information is incorrect in the configuration file.");
            }
            Settings = settings.First();
        }

        public struct SettingsInfo
        {
            public string OauthScope { get; set; }
            public string BaseUrl { get; set; }
            public string FilePath { get; set; }
            public string UseProject { get; set; }
            public string AssetTypes { get; set; }
            public string V1Username { get; set; }
            public string V1Password { get; set; }
            public string AuthType { get; set; }
        }
    }
}
