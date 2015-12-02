using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace SocLoc_project_WP
{
    public partial class UserScreen : PhoneApplicationPage
    {
        string userName;

        public UserScreen()
        {
            //userName = usrNm;
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string par = "";

            if (NavigationContext.QueryString.TryGetValue("par", out par))
            {
                userName = par;
                UserLogger userLogger = new UserLogger(userName);
                DataContext = userLogger;
            }
        }
    }
}