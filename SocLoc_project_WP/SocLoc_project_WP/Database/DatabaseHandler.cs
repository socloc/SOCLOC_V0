using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SocLoc_project_WP
{
    delegate void IndicatorToDownload();
    delegate void IndicatorForFriendsLoc(List<Friend> friendsList);
    delegate void IndicatorGetName(int userID, string name);
    static class DatabaseHandler
    {
        public static Boolean authentification = false;
        public static int userId;
        public static Boolean isRegisterSuccess = false;
        public static Boolean isThereFriendsLoc = false;
        public static event IndicatorToDownload WhenDownloaded_whenReg;
        public static event IndicatorToDownload WhenDownloaded_whenLog;
        public static event IndicatorToDownload WhenNoFriends;
        public static event IndicatorForFriendsLoc WhenFriendsLoc;
        public static event IndicatorGetName WhenGetName;

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

        internal static void GiveLocation(int userID, double latitude, double longitude)
        {
            HttpWebRequest httpRequest = HttpWebRequest.CreateHttp(new Uri("http://socloc.chrix.eu/servlets/location/add?user_id=" + userID.ToString() + "&latitude=" + latitude.ToString() + "&longitude=" + longitude.ToString(), UriKind.Absolute));
            IAsyncResult result = (IAsyncResult)httpRequest.BeginGetResponse(new AsyncCallback(runGiveLocationEvent), httpRequest);
        }

        private static void runGiveLocationEvent(IAsyncResult result)
        {
            try
            {
                HttpWebRequest httpRequest = (HttpWebRequest)result.AsyncState;
                WebResponse webResponse = httpRequest.EndGetResponse(result);
                //Stream stream = webResponse.GetResponseStream();
                //StreamReader streamReader = new StreamReader(stream);
                //streamReader.Dispose();
                //stream.Dispose();
                webResponse.Dispose();
            }
            catch (WebException ex)
            {

            }
        }

        internal static void GetYourFriendsLocation(int userID)
        {
            HttpWebRequest httpRequest = HttpWebRequest.CreateHttp(new Uri("http://socloc.chrix.eu/servlets/location/getFriends/" + userID.ToString(), UriKind.Absolute));
            IAsyncResult result = (IAsyncResult)httpRequest.BeginGetResponse(new AsyncCallback(getFriendsLocEvent), httpRequest);

        }

        private static void getFriendsLocEvent(IAsyncResult result)
        {
            List<string> lines = new List<string>();
            try
            {
                HttpWebRequest httpRequest = (HttpWebRequest)result.AsyncState;
                WebResponse webResponse = httpRequest.EndGetResponse(result);

                Stream stream = webResponse.GetResponseStream();
                StreamReader streamReader = new StreamReader(stream);
                string text = streamReader.ReadToEnd();
                //lines.Add(streamReader.ReadLine());
                //lines.Add(streamReader.ReadLine());
                //lines.Add(streamReader.ReadLine());

                if (text == "Brak ostatnich lokalizacji znajomych.")
                {
                    isThereFriendsLoc = false;
                    WhenNoFriends();
                }
                else
                {
                    List<Friend> friendsList = new List<Friend>();
                    string[] words = text.Split('{');
                    const string patternForClassify = @".*user_id"":(.*),""latitude"":""(.*)"",""longitude"":""(.*)"",""created_at.*";
                    Regex rExtractToClassify = new Regex(patternForClassify, RegexOptions.IgnoreCase);
                    Match mExtract;
                    Group g0;
                    Group g1;
                    Group g2;
                    Group g3;
                    for (int i = 1; i < words.Length; i++)
                    {
                        mExtract = rExtractToClassify.Match(words[i]);
                        g1 = mExtract.Groups[1];
                        g2 = mExtract.Groups[2];
                        g3 = mExtract.Groups[3];
                        int usrID = Int16.Parse(g1.ToString());
                        Double usrLat = Double.Parse(g2.ToString());
                        Double usrLong = Double.Parse(g3.ToString());
                        Friend friend = new Friend(usrID, usrLong, usrLat);
                        friendsList.Add(friend);
                    }
                    WhenFriendsLoc(friendsList);
                    //isThereFriendsLoc = false;

                    //foreach(string line in lines)
                    //{


                    //}
                    //WhenFriendsLoc();
                }
                streamReader.Dispose();
            }
            catch (WebException ex)
            {
                isRegisterSuccess = false;
                WhenDownloaded_whenReg();
            }
        }

        internal static void GetFriendName(int userID)
        {
            HttpWebRequest httpRequest = HttpWebRequest.CreateHttp(new Uri("http://socloc.chrix.eu/servlets/user/getData/" + userID.ToString(), UriKind.Absolute));
            IAsyncResult result = (IAsyncResult)httpRequest.BeginGetResponse(new AsyncCallback(getFriendsNameEvent), httpRequest);

        }

        private static void getFriendsNameEvent(IAsyncResult result)
        {
            List<string> lines = new List<string>();
            try
            {
                HttpWebRequest httpRequest = (HttpWebRequest)result.AsyncState;
                WebResponse webResponse = httpRequest.EndGetResponse(result);

                Stream stream = webResponse.GetResponseStream();
                StreamReader streamReader = new StreamReader(stream);
                string text = streamReader.ReadToEnd();

                if (text == "Nie znaleziono takiego użytkownika.")
                {
                }
                else
                {
                    const string patternForClassify = @".*id"":(.*),""name"":""(.*)"",""surname"":""(.*)"",""date.*";
                    Regex rExtractToClassify = new Regex(patternForClassify, RegexOptions.IgnoreCase);
                    Match mExtract;
                    Group g0;
                    Group g1;
                    Group g2;
                    Group g3;
                    mExtract = rExtractToClassify.Match(text);
                    g1 = mExtract.Groups[1];
                    g2 = mExtract.Groups[2];
                    g3 = mExtract.Groups[3];
                    int userId = Int16.Parse(g1.ToString());
                    string returnedName = g2.ToString() + ' ' + g3.ToString();
                    WhenGetName(userId, returnedName);
                }
                streamReader.Dispose();
            }
            catch (WebException ex)
            {
                isRegisterSuccess = false;
                WhenDownloaded_whenReg();
            }
        }

        
    }
    }
