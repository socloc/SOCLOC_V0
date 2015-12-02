using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SocLoc_project_WP
{
    static class DatabaseHandler
    {
        static Boolean authentification = false;

        static public Boolean LogIn(string userName, string password)
        {
            WebClient webClient = new WebClient();
            webClient.OpenReadCompleted += new OpenReadCompletedEventHandler(authUser_OpenReadCompletedEventArgs);
            webClient.OpenReadAsync(new Uri(""));
            return authentification;
        }

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
                }
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
