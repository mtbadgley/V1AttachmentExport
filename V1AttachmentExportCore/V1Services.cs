using System;
using System.Configuration;
using System.Net;
using NLog;
using OAuth2Client;

namespace V1AttachmentExportCore
{
    public class V1Services
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static readonly V1AttachmentExportConfiguration Config =
            (V1AttachmentExportConfiguration)ConfigurationManager.GetSection("V1AttachmentExport");

        #region HTTPCalls
        /// <summary>
        ///     Perform a REST API HTTP GET call.
        ///     Uses OAuth2 to authenticate.
        /// </summary>
        /// <param name="queryurl">Fully qualified URL to perform a GET</param>
        /// <returns>
        ///     Returns response from API Call, XML or JSON. Will return
        ///     empty string for exception.
        /// </returns>
        public static string MakeHttpRequest(string queryurl)
        {
            Logger.Debug("Processing GET :: {0}", queryurl);
            try
            {
                string result;
                NetworkCredential credentials = GetCredentials();

                using (var webclient = new WebClient())
                {
                    webclient.Credentials = credentials;
                    result = webclient.DownloadString(queryurl);
                }
                Logger.Debug("Query Results :: {0}", result);
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error("Error processing query. Exception: {0}", ex.Message);
                return String.Empty;
            }
        }

        /// <summary>
        ///     Perform a REST API HTTP POST call.
        ///     Uses OAuth2 for authentication.
        /// </summary>
        /// <param name="queryurl">A fully qualified URL</param>
        /// <param name="payload">Either XML or JSON payload based on the call</param>
        /// <returns>
        ///     Response from call, XML or JSON.  Will return an empty
        ///     string if call fails.
        /// </returns>
        public static string MakeHttpPost(string queryurl, string payload = "")
        {
            Logger.Debug("Processing POST :: {0}", queryurl);
            NetworkCredential credentials = GetCredentials();
            try
            {
                string result;
                using (var webclient = new WebClient())
                {
                    webclient.Credentials = credentials;
                    result = webclient.UploadString(queryurl, "POST", payload);
                }
                Logger.Debug("Query Results :: {0}", result);
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error("Error processing query. Exception: {0}", ex.Message);
                return String.Empty;
            }
        }

        /// <summary>
        /// Uses the .Net Webclient to download a file from any website
        /// via a Url.
        /// </summary>
        /// <param name="fileurl"></param>
        /// <param name="filename"></param>
        /// <returns>True if successful, else Fail for failure.</returns>
        public static bool DownloadFileFromUrl(string fileurl, string filename)
        {
            Logger.Debug("Processing File Url :: {0}", fileurl);
            NetworkCredential credentials = GetCredentials();
            try
            {
                using (var webclient = new WebClient())
                {
                    webclient.Credentials = credentials;
                    webclient.DownloadFile(fileurl, filename);
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error("Error processing query. Exception: {0}", ex.Message);
                return false;
            }
        }
        #endregion HTTPCalls

        #region NetworkCredentials
        /// <summary>
        /// Determines whether or not OAuth or Basic Auth is being used, and 
        /// then creates the appropriate network credentials for use by the 
        /// .Net web client.
        /// </summary>
        /// <returns>NetworkCredential object for the .Net WebClient.</returns>
        private static NetworkCredential GetCredentials()
        {
            NetworkCredential creds;
            if (Config.Settings.AuthType.ToUpper() == "OAUTH")
            {
                IStorage storage = Storage.JsonFileStorage.Default;
                creds = new OAuth2Credential(Config.Settings.OauthScope, storage, null);
            }
            else
            {
                creds = new NetworkCredential(Config.Settings.V1Username, Config.Settings.V1Password);
            }
            return creds;
        }
        #endregion NetworkCredentials
    }
}
