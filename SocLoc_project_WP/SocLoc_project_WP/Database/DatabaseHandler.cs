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
    delegate void IndicatorToErrorHandling(string ex);
    delegate void IndicatorForFriendsLoc(List<Friend> friendsList);
    delegate void IndicatorForUnknownsLoc(List<UnknownUser> unknownsList);
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
        public static event IndicatorToErrorHandling WhenErrorOccurs;
        public static event IndicatorForFriendsLoc WhenFriendsLoc;
        public static event IndicatorForUnknownsLoc WhenAllLoc;
        public static event IndicatorGetName WhenGetName;

        public static void LogIn(string userName, string password)
        {
                HttpWebRequest httpRequest = HttpWebRequest.CreateHttp(new Uri("http://socloc.chrix.eu/servlets/user/login?email=" + userName + "&password=" + password, UriKind.Absolute));
                IAsyncResult result = (IAsyncResult)httpRequest.BeginGetResponse(new AsyncCallback(turnOnLoginEvent), httpRequest);
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
                }
                streamReader.Dispose();
            }
            catch (WebException ex)
            {
                WhenErrorOccurs(ex.ToString());
            }
        }

        internal static void GetName(int userID)
        {
            HttpWebRequest httpRequest = HttpWebRequest.CreateHttp(new Uri("http://socloc.chrix.eu/servlets/user/getData/" + userID.ToString(), UriKind.Absolute));
            IAsyncResult result = (IAsyncResult)httpRequest.BeginGetResponse(new AsyncCallback(getNameEvent), httpRequest);

        }

        private static void getNameEvent(IAsyncResult result)
        {
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
                WhenErrorOccurs(ex.ToString());
            }
        }

        internal static void GetAllLastLocation()
        {
            HttpWebRequest httpRequest = HttpWebRequest.CreateHttp(new Uri("http://socloc.chrix.eu/servlets/location/getAll", UriKind.Absolute));
            IAsyncResult result = (IAsyncResult)httpRequest.BeginGetResponse(new AsyncCallback(getAllEvent), httpRequest);
        }

        private static void getAllEvent(IAsyncResult result)
        {
            try
            {
                HttpWebRequest httpRequest = (HttpWebRequest)result.AsyncState;
                WebResponse webResponse = httpRequest.EndGetResponse(result);

                Stream stream = webResponse.GetResponseStream();
                StreamReader streamReader = new StreamReader(stream);
                string text = streamReader.ReadToEnd();

                if (text == "Brak ostatnich lokalizacji znajomych.")
                {
                    isThereFriendsLoc = false;
                    WhenNoFriends();
                }
                else
                {
                    List<UnknownUser> unknownList = new List<UnknownUser>();
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
                        UnknownUser unkUs = new UnknownUser(usrID, usrLong, usrLat);
                        unknownList.Add(unkUs);
                    }
                    WhenAllLoc(unknownList);
                }
                streamReader.Dispose();
            }
            catch (WebException ex)
            {
                WhenErrorOccurs(ex.ToString());
            }
        }
    }
}
