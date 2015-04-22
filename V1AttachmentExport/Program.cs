using System;
using System.Configuration;
using NLog;
using V1AttachmentExportCore;

namespace V1AttachmentExport
{
    public class Program
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static readonly V1AttachmentExportConfiguration Config =
            (V1AttachmentExportConfiguration) ConfigurationManager.GetSection("V1AttachmentExport");

        private static void Main()
        {
            string[] assettypes = Config.Settings.AssetTypes.Split(Convert.ToChar(","));
            foreach (string assettype in assettypes)
            {
                Logger.Info("Processing Asset Type :: {0}", assettype);
                int numdownloads = ProcessV1Attachments.DownloadAttachmentsByAssetType(assettype);
                Logger.Info("Processing Asset Type Complete :: {0} :: Number of Downloads = {1}", assettype, numdownloads);
            }
        }
    }
}