using System.Configuration;
using System.Xml;

namespace V1AttachmentExportCore
{
    public class ConfigurationHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            return new V1AttachmentExportConfiguration(section);
        }
    }
}