using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace SocLoc_project_WP
{
    delegate void IndicatorToDownload();
    static class DatabaseHandler
    {
        public static Boolean authentification = false;
        public static int userId;
        public static Boolean isRegisterSuccess = false;
        public static event IndicatorToDownload WhenDownloaded_whenReg;
        public static event IndicatorToDownload WhenDownloaded_whenLog;

        public static void LogIn(string userName, string password)
        {
            //List<string> lines = new List<string>();
            //try
            //{
                HttpWebRequest httpRequest = HttpWebRequest.CreateHttp(new Uri("http://socloc.chrix.eu/servlets/user/login?email=" + userName + "&password=" + password, UriKind.Absolute));
                IAsyncResult result = (IAsyncResult)httpRequest.BeginGetResponse(new AsyncCallback(turnOnLoginEvent), httpRequest);

            //    HttpWebRequest httpRequest = HttpWebRequest.CreateHttp(new Uri("http://socloc.chrix.eu/servlets/user/login?email=" + userName + "&password=" + password, UriKind.Absolute));
            //    httpRequest.BeginGetResponse(asyncResult =>
            //    {
            //        WebResponse webResponse = httpRequest.EndGetResponse(asyncResult);
            //        Stream stream = webResponse.GetResponseStream();
            //        StreamReader streamReader = new StreamReader(stream);
            //        lines.Add(streamReader.ReadLine());
            //        if (lines[0] != "0")
            //        {
            //            userId = Int32.Parse(lines[0]);
            //            authentification = true;
            //        }
            //        else
            //        {
            //            authentification = false;
            //        }
            //        streamReader.Dispose();
            //    }, null);
            //}
            //catch(WebException ex)
            //{
            //    authentification = false;
            //}

            //if (WhenDownloaded_whenLog != null)
            //    WhenDownloaded_whenLog();
            #region alternative option for taking response from server --> WebRequest class is also possible 
            //webClient.DownloadStringCompleted += WebClient_DownloadStringCompleted;
            //webClient.DownloadStringAsync(new Uri("", UriKind.Absolute));
            //WebClient webClient = new WebClient();
            //webClient.OpenReadCompleted += new OpenReadCompletedEventHandler(authUser_OpenReadCompletedEventArgs);
            //webClient.OpenReadAsync(new Uri("http://socloc.chrix.eu/servlets/user/login?email=" + userName + "&password=" + password));
            #endregion

            #region deserializing for JSON
            //HttpWebRequest httpRequest = HttpWebRequest.CreateHttp(new Uri("", UriKind.Absolute));
            //httpRequest.BeginGetResponse(asyncResult =>
            //{
            //    /*
            //    json2csharp.com --> generate csharp class from json file
            //    */
            //    WebResponse webResponse = httpRequest.EndGetResponse(asyncResult);
            //    Stream stream = webResponse.GetResponseStream();
            //    DataContractJsonSerializer jsSerializer = new DataContractJsonSerializer(typeof(LogJS[]));
            //    var users = (LogJS[])jsSerializer.ReadObject(stream);
            //    //var user = users[0];
            //}, null);
            #endregion
        }

        private static void turnOnLoginEvent(IAsyncResult result)
        {
            List<string> lines = new List<string>();
            try
            {
                HttpWebRequest httpRequest = (HttpWebRequest)result.AsyncState;
                WebResponse webResponse = httpRequest.EndGetResponse(result);

                Stream stream = webResponse.GetResponseStream();
                StreamReader streamReader = new StreamReader(stream);
                lines.Add(streamReader.ReadLine());
                if (lines[0] != "0")
                    {
                        userId = Int32.Parse(lines[0]);
                        authentification = true;
                        WhenDownloaded_whenLog();
                }
                    else
                    {
                        authentification = false;
                        WhenDownloaded_whenLog();
                    }
                    streamReader.Dispose();
            }
            catch (WebException ex)
            {
                authentification = false;
                WhenDownloaded_whenLog();
            }
        }

      

        internal static void RegisterIn(string name, string surname, string city, string phone, string email, string password)
        {
            HttpWebRequest httpRequest = HttpWebRequest.CreateHttp(new Uri("http://socloc.chrix.eu/servlets/user/create?name=" + name + "&surname=" + surname + "&city=" + city + "&phone=" + phone + "&email=" + email + "&password=" + password, UriKind.Absolute));
            IAsyncResult result = (IAsyncResult)httpRequest.BeginGetResponse(new AsyncCallback(turnOnRegisterEvent), httpRequest);
            //AsyncCallback callBack = new AsyncCallback(turnOnRegEvent);
            //httpRequest.BeginGetResponse(asyncResult =>
            //{
            //    try
            //    {

            //        WebResponse webResponse = httpRequest.EndGetResponse(asyncResult);

            //        Stream stream = webResponse.GetResponseStream();
            //        StreamReader streamReader = new StreamReader(stream);
            //        lines.Add(streamReader.ReadLine());

            //        if (lines[0] != "")
            //        {
            //            isRegisterSuccess = true;
            //        }
            //        else
            //        {
            //            isRegisterSuccess = false;
            //        }
            //        streamReader.Dispose();
            //    }
            //    catch (WebException ex)
            //    {
            //        isRegisterSuccess = false;
            //         //WhenDownloaded_whenReg();
            //     }
            //}, null);
            //try
            //{
            //    WebResponse webResponse = await httpRequest.GetResponseAsync();
            //    Stream stream = webResponse.GetResponseStream();
            //    StreamReader streamReader = new StreamReader(stream);
            //    lines.Add(streamReader.ReadLine());

            //    if (lines[0] != "")
            //    {
            //        isRegisterSuccess = true;
            //    }
            //    else
            //    {
            //        isRegisterSuccess = false;
            //    }
            //    streamReader.Dispose();
            //}
            //catch (WebException ex)
            //{
            //    isRegisterSuccess = false;
            //    //WhenDownloaded_whenReg();
            //}


            //if (httpRequest.HaveResponse)
            //    WhenDownloaded_whenReg();
        }

        private static void turnOnRegisterEvent(IAsyncResult result)
        {
            List<string> lines = new List<string>();
            try
            {
                HttpWebRequest httpRequest = (HttpWebRequest)result.AsyncState;
                WebResponse webResponse = httpRequest.EndGetResponse(result);

                Stream stream = webResponse.GetResponseStream();
                StreamReader streamReader = new StreamReader(stream);
                lines.Add(streamReader.ReadLine());

                if (lines[0] != "")
                {
                    isRegisterSuccess = true;
                    WhenDownloaded_whenReg();
                }
                else
                {
                    isRegisterSuccess = false;
                    WhenDownloaded_whenReg();
                }
                streamReader.Dispose();
            }
            catch (WebException ex)
            {
                isRegisterSuccess = false;
                WhenDownloaded_whenReg();
            }
        }

        //static private void authUser_OpenReadCompletedEventArgs(object sender, OpenReadCompletedEventArgs e)
        //{
        //    List<string> lines = new List<string>();
        //    try
        //    {
        //        using (StreamReader streamReader = new StreamReader(e.Result))
        //        {
        //            while (!streamReader.EndOfStream)
        //            {
        //                lines.Add(streamReader.ReadLine());
        //            }
        //            //DataContractJsonSerializer jsSerializer = new DataContractJsonSerializer(typeof(LogJS[]));
        //            //var users = (LogJS[])jsSerializer.ReadObject(streamReader);
        //            //var user = users[0];
        //        }
        //        if(lines[0] == "1")
        //            authentification = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        authentification = false;
        //    }
        //}
    }
}
