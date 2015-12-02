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
        public static event IndicatorToDownload WhenDownloaded;

        public static void LogIn(string userName, string password)
        {
            //List<string> lines = new List<string>();

            //WebResponse webResponse = await httpRequest.
            //Stream stream = webResponse.GetResponseStream();
            //using (StreamReader streamReader = new StreamReader(stream))
            //{
            //    lines.Add(streamReader.ReadLine());
            //}

            //if (lines[0] == "1")
            //{
            //    authentification = true;
            //}
            //else
            //{
            //    authentification = false;
            //}
            //Task task = Task.Run(() =>
            //{
            List<string> lines = new List<string>();
            HttpWebRequest httpRequest = HttpWebRequest.CreateHttp(new Uri("http://socloc.chrix.eu/servlets/user/login?email=" + userName + "&password=" + password, UriKind.Absolute));
            httpRequest.BeginGetResponse(asyncResult =>
        {
            WebResponse webResponse = httpRequest.EndGetResponse(asyncResult);
            Stream stream = webResponse.GetResponseStream();
            StreamReader streamReader = new StreamReader(stream);
            lines.Add(streamReader.ReadLine());
            if (lines[0] == "1")
            {
                authentification = true;
            }
            else
            {
                authentification = false;
            }

         

            //streamReader.Dispose();
        }, null);

            if (WhenDownloaded != null)
                WhenDownloaded();
            //});
            //return task;
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

        //alternative option for taking response from server
        //private static void WebClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        //{
        //    var jsonResult = e.Result;
        //    DataContractJsonSerializer jsSerializer = new DataContractJsonSerializer(typeof(LogJS[]));
        //    var users = (LogJS[])jsSerializer.ReadObject(jsonResult);
        //    var user = users[0]; 
        //}

        //it's also possible to get data from server with OpenReadCompletedEventArgs
        static private void authUser_OpenReadCompletedEventArgs(object sender, OpenReadCompletedEventArgs e)
        {
            List<string> lines = new List<string>();
            try
            {
                using (StreamReader streamReader = new StreamReader(e.Result))
                {
                    while (!streamReader.EndOfStream)
                    {
                        lines.Add(streamReader.ReadLine());
                    }
                    //DataContractJsonSerializer jsSerializer = new DataContractJsonSerializer(typeof(LogJS[]));
                    //var users = (LogJS[])jsSerializer.ReadObject(streamReader);
                    //var user = users[0];
                }
                if(lines[0] == "1")
                    authentification = true;
            }
            catch (Exception ex)
            {
                authentification = false;
            }
        }

        internal static bool RegisterIn(string userName, string password)
        {
            return true;
        }
    }
}
