using System;
using System.Xml;
using NUnit.Framework;
using V1AttachmentExportCore;

namespace V1AttachmentExportTest
{
    [TestFixture]
    public class V1AttachmentExportTest
    {
        [Test]
        public void DownloadAttachmentsByAssetTypeTest()
        {
            int numdownloaded = ProcessV1Attachments.DownloadAttachmentsByAssetType("Story");
            Assert.AreEqual(2,numdownloaded);
        }
        
        [Test]
        public void DownloadAttachmentTest()
        {
            bool results = ProcessV1Attachments.DownloadAttachment("1975", "B-01040", "sample_rmaform.pdf","Call Center");
            Assert.IsTrue(results);
        }

        [Test]
        public void GetFullPathTest()
        {
            var fullpath = ProcessV1Attachments.GetFullPath(@"A Happyday%of fruit with+of$funNow\");
            Assert.AreEqual(@"C:\\v1attachments\\AHappydayoffruitwithoffunNow",fullpath);
        }

        [Test]
        public void DownloadFileFromUrlTest()
        {
            bool results = V1Services.DownloadFileFromUrl("http://localhost/VersionOne/attachment.img/1975", "C:\\v1attachments\\B-01040_sample_rmaform.pdf");
            Assert.IsTrue(results);
        }
        
        [Test]
        public void MakeHttpRequestTest()
        {
            string results = V1Services.MakeHttpRequest("http://localhost/VersionOne/rest-1.v1/Data/Member/20");
            Assert.IsTrue(results.Contains("Member:20"));
        }
        
        [Test]
        public void GetAttachmentsTest()
        {
            string getattachments = ProcessV1Attachments.GetAttachments("Story");
            int result = 0;
            if (!String.IsNullOrEmpty(getattachments))
            {
                var getattachmentsxml = new XmlDocument();
                getattachmentsxml.LoadXml(getattachments);
                var xmlNodeList = getattachmentsxml.SelectNodes("Assets/Asset");
                if (xmlNodeList != null)
                    result = xmlNodeList.Count;
            }
            Assert.AreEqual(2,result);
        }
    }
}
