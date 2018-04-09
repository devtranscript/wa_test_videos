using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace wa_transcript
{
    public class mdl_ftp
    {
        public class FTPManagerClass
        {
            private static string password = "";
            private static string username = "";
            private static string host = "";

            private FtpWebRequest ftpRequest = null;
            private FtpWebResponse ftpResponse = null;
            private Stream ftpStream = null;

            public FTPManagerClass(string user, string pass, string hostname)
            {
                username = user;
                password = pass;
                host = hostname;
            }

            public void DownloadFile(string remoteFile, string localFile)
            {
                try
                {
                    ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + remoteFile);
                    ftpRequest.Credentials = new NetworkCredential(username, password);
                    ftpRequest.UseBinary = true;
                    ftpRequest.UsePassive = true;
                    ftpRequest.KeepAlive = true;
                    ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                    ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                    ftpStream = ftpResponse.GetResponseStream();
                    FileStream fs = new FileStream(localFile, FileMode.OpenOrCreate);
                    byte[] byteBuffer = new byte[Convert.ToInt32(getFileSize(remoteFile))];
                    int bytesRead = ftpStream.Read(byteBuffer, 0, Convert.ToInt32(getFileSize(remoteFile)));
                    try
                    {
                        while (bytesRead > 0)
                        {
                            fs.Write(byteBuffer, 0, bytesRead);
                            bytesRead = ftpStream.Read(byteBuffer, 0, Convert.ToInt32(getFileSize(remoteFile)));
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                    ftpResponse.Close();
                    ftpStream.Close();
                    fs.Close();
                    ftpRequest = null;
                }
                catch (Exception ex)
                {

                }
            }

            public void UploadFile(string localFile, string remoteFile)
            {
                try
                {
                    ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + remoteFile);
                    ftpRequest.Credentials = new NetworkCredential(username, password);
                    ftpRequest.UseBinary = true;
                    ftpRequest.UsePassive = true;
                    ftpRequest.KeepAlive = true;
                    ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
                    ftpStream = ftpRequest.GetRequestStream();
                    FileStream lfs = new FileStream(localFile, FileMode.Open);
                    byte[] bytebuffer = new byte[lfs.Length];
                    int bytesSend = lfs.Read(bytebuffer, 0, (int)lfs.Length);
                    try
                    {
                        while (bytesSend != -1)
                        {
                            ftpStream.Write(bytebuffer, 0, bytesSend);
                            bytesSend = lfs.Read(bytebuffer, 0, (int)lfs.Length);

                        }
                    }


                    catch (Exception ex)
                    {

                    }
                    lfs.Close();
                    ftpStream.Close();
                    ftpRequest = null;
                }
                catch (Exception ex)
                {

                }
            }

            public void Rename(string oldname, string newname)
            {
                try
                {
                    ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + oldname);
                    ftpRequest.Credentials = new NetworkCredential(username, password);
                    ftpRequest.UseBinary = true;
                    ftpRequest.UsePassive = true;
                    ftpRequest.KeepAlive = true;
                    ftpRequest.Method = WebRequestMethods.Ftp.Rename;
                    ftpRequest.RenameTo = newname;
                    ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                    ftpResponse.Close();
                    ftpRequest = null;
                }
                catch (Exception ex)
                {


                }
            }

            public string[] getFilesOnServer(string dir)
            {
                string[] filesInDir = new string[10];
                try
                {
                    ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + dir);
                    ftpRequest.Credentials = new NetworkCredential(username, password);
                    ftpRequest.UsePassive = true;
                    ftpRequest.UseBinary = true;
                    ftpRequest.KeepAlive = true;
                    ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                    ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                    ftpStream = ftpResponse.GetResponseStream();
                    StreamReader sr = new StreamReader(ftpStream);
                    string dirRaw = null;
                    try
                    {
                        while (sr.Peek() != -1)
                        {
                            dirRaw += sr.ReadLine() + "|";
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    ftpResponse.Close();
                    sr.Close();
                    ftpStream.Close();
                    ftpRequest = null;
                    try
                    {
                        filesInDir = dirRaw.Split("|".ToCharArray());
                        return filesInDir;
                    }
                    catch (Exception ex)
                    {

                    }
                }
                catch (Exception ex)
                {

                }
                return filesInDir;
            }

            public void Delete(string filename)
            {
                try
                {
                    ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + filename);
                    ftpRequest.Credentials = new NetworkCredential(username, password);
                    ftpRequest.UseBinary = true;
                    ftpRequest.UsePassive = true;
                    ftpRequest.KeepAlive = true;
                    ftpRequest.Method = WebRequestMethods.Ftp.DeleteFile;
                    ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                    ftpResponse.Close();
                    ftpRequest = null;
                }
                catch (Exception ex)
                {

                }
            }

            public void CreateDir(string name)
            {
                try
                {
                    ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + name);
                    ftpRequest.Credentials = new NetworkCredential(username, password);
                    ftpRequest.UseBinary = true;
                    ftpRequest.UsePassive = true;
                    ftpRequest.KeepAlive = true;
                    ftpRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
                    ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                    ftpResponse.Close();
                    ftpRequest = null;
                }
                catch (Exception ex)
                {

                }
            }

            public long getFileSize(string filename)
            {
                long size;

                FtpWebRequest sizeRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + filename);
                sizeRequest.Credentials = new NetworkCredential(username, password);
                sizeRequest.Method = WebRequestMethods.Ftp.GetFileSize;
                sizeRequest.UseBinary = true;

                FtpWebResponse serverResponse = (FtpWebResponse)sizeRequest.GetResponse();
                FtpWebResponse respSize = (FtpWebResponse)sizeRequest.GetResponse();
                size = respSize.ContentLength;

                return size;
            }


        }
    }
}