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
    static class DatabaseHandler
    {
        static Boolean authentification = false;

        static public Boolean LogIn(string userName, string password)
        {
            //alternative option for taking response from server --> WebRequest class is also possible 
            //WebClient webClient = new WebClient();
            //webClient.DownloadStringCompleted += WebClient_DownloadStringCompleted;
            //webClient.DownloadStringAsync(new Uri("", UriKind.Absolute));

            //webClient.OpenReadCompleted += new OpenReadCompletedEventHandler(authUser_OpenReadCompletedEventArgs);
            //webClient.OpenReadAsync(new Uri(""));
            //WebBrowser webBrowser = new WebBrowser();

            HttpWebRequest httpRequest = HttpWebRequest.CreateHttp(new Uri("", UriKind.Absolute));
            httpRequest.BeginGetResponse(asyncResult =>
            {
                /*
                json2csharp.com --> generate csharp class from json file
                */
                WebResponse webResponse = httpRequest.EndGetResponse(asyncResult);
                Stream stream = webResponse.GetResponseStream();
                DataContractJsonSerializer jsSerializer = new DataContractJsonSerializer(typeof(LogJS[]));
                var users = (LogJS[])jsSerializer.ReadObject(stream);
                var user = users[0];
            }, null);
            return authentification;
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
        //            DataContractJsonSerializer jsSerializer = new DataContractJsonSerializer(typeof(LogJS[]));
        //            var users = (LogJS[])jsSerializer.ReadObject(streamReader);
        //            var user = users[0];
        //        }
        //        authentification = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        authentification = false;
        //    }
        //}

        internal static bool RegisterIn(string userName, string password)
        {
            return true;
        }
    }
}
