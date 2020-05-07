using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FTP.Utility
{
    public static class FtpUtility
    {
        ///<summary>Deletes the file on the FTP site</summary>
        ///<param name="filePathName">The file fullpath and name</param>
        ///<param name="ftpUserId">The FTP User ID</param>
        ///<param name="ftpPassword">The FTP Password</param>
        ///<param name="message">Processing Message</param>
        ///<returns>0-OK, -1=unknown error, 1=file not found and also also returns error message</returns>
        public static int DeleteFile(string filePathName, string ftpUserId, string ftpPassword, ref string message)
        {
            FtpWebRequest ftpWebRequest = (FtpWebRequest)WebRequest.Create(filePathName);
            FtpWebResponse response = null;
            var responseValue = -1;

            ftpWebRequest.Credentials = new NetworkCredential(ftpUserId, ftpPassword);
            ftpWebRequest.Method = WebRequestMethods.Ftp.DeleteFile;

            try
            {
                response = (FtpWebResponse)ftpWebRequest.GetResponse();
                responseValue = 0;
                message = String.Empty;
            }
            catch (WebException ex)
            {
                message = ex.Message;
                if (message.Contains("(550)")) //file not found
                {
                    responseValue = 1;
                }
            }
            finally
            {
                if (response != null)
                    response.Close();
            }
            return responseValue;
        }

        ///<summary>Renames the file on the FTP site</summary>
        ///<param name="filePathName">The file fullpath and name</param>
        ///<param name="newFileName">The new file name with extension</param>
        ///<param name="ftpUserId">The FTP User ID</param>
        ///<param name="ftpPassword">The FTP Password</param>
        ///<param name="message">Processing Message</param>
        ///<returns>0-OK, -1=unknown error, 1=file not found and also also returns error message</returns>
        public static int RenameFile(string filePathName, string newFileName, string ftpUserId, string ftpPassword, ref string message)
        {
            FtpWebRequest ftpWebRequest = (FtpWebRequest)WebRequest.Create(filePathName);
            FtpWebResponse response = null;
            var responseValue = -1;

            ftpWebRequest.Credentials = new System.Net.NetworkCredential(ftpUserId, ftpPassword);
            //ftpWebRequest.KeepAlive = True
            //ftpWebRequest.UsePassive = True
            ftpWebRequest.Method = WebRequestMethods.Ftp.Rename;
            ftpWebRequest.RenameTo = newFileName;
            try
            {
                response = (FtpWebResponse)ftpWebRequest.GetResponse();
                responseValue = 0;
            }
            catch (WebException ex)
            {
                message = ex.Message;
                if (message.Contains("(550)")) //file not found
                {
                    responseValue = 1;
                }
            }
            finally
            {
                if (response != null)
                    response.Close();
            }
            return responseValue;
        }

        ///<summary>Upload File</summary>
        ///<param name="localPath">The file fullpath and name</param>
        ///<param name="ftpPath">The file ftp fullpath and name</param>
        ///<param name="ftpUserId">The FTP User ID</param>
        ///<param name="ftpPassword">The FTP Password</param>
        ///<param name="message">Processing Message</param>
        ///<returns>0-OK, -1=unknown error, 1=file not found and also also returns error message</returns>
        public static int UploadFile(string localPath, string ftpPath, string ftpUserId, string ftpPassword, ref string message)
        {
            FtpWebRequest ftpWebRequest = (FtpWebRequest)WebRequest.Create(ftpPath);
            var responseValue = -1;

            ftpWebRequest.Credentials = new System.Net.NetworkCredential(ftpUserId, ftpPassword);
            //ftpWebRequest.KeepAlive = True
            //ftpWebRequest.UsePassive = True
            ftpWebRequest.Method = WebRequestMethods.Ftp.UploadFile;

            try
            {
                // read in file...
                byte[] bFile = System.IO.File.ReadAllBytes(localPath);

                // upload file...
                Stream ioStream = ftpWebRequest.GetRequestStream();
                ioStream.Write(bFile, 0, bFile.Length);
                ioStream.Close();
                ioStream.Dispose();

                responseValue = 0;
            }
            catch (WebException ex)
            {
                message = ex.Message;
                if (message.Contains("(x550)") | message.Contains("(550)")) //file not found
                {
                    responseValue = 1;
                }
            }
            finally
            {
            }
            return responseValue;
        }

        ///<summary>Check if the file exists at FTP location</summary>
        ///<param name="ftpFilePath">The file ftp fullpath and name</param>
        ///<param name="ftpUserId">The FTP User ID</param>
        ///<param name="ftpPassword">The FTP Password</param>
        ///<param name="message">Processing Message</param>
        ///<returns>true/false</returns>
        public static bool CheckFileExists(string ftpFilePath, string ftpUserId, string ftpPassword)
        {
            dynamic request = (FtpWebRequest)WebRequest.Create(ftpFilePath);
            request.Credentials = new NetworkCredential(ftpUserId, ftpPassword);
            request.Method = WebRequestMethods.Ftp.GetFileSize;
            try
            {
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                return true;
            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;
                if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    return false;
                }
            }
            return false;
        }
    }
}
