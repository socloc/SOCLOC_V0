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
        int userId;
        IntervalGetter intervalGetter;
        public UserScreen()
        {
            //userName = usrNm;
            InitializeComponent();
            //IntervalGetter.typeOfGetter.getPosition
            int interval = 10;
            CreatePostionGetter(interval);
            
        }

        private void CreatePostionGetter(int interval)
        {
            intervalGetter = new IntervalGetter(10, IntervalGetter.typeOfGetter.getPosition);
            intervalGetter.getPositionAsync();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string par = "";
            string valueToParse = "";
            string[] parsedValues;

            if (NavigationContext.QueryString.TryGetValue("par", out par))
            {
                valueToParse = par;
                parsedValues = valueToParse.Split('_');
                userName = parsedValues[0];
                userId = Int32.Parse(parsedValues[1]);
                UserLogger userLogger = new UserLogger(userName, userId);
                DataContext = userLogger;
            }
        }

        private void Cancel_Interval_Click(object sender, RoutedEventArgs e)
        {
            intervalGetter.tokenSource.Cancel();
        }

        private void goToMap_button_Click(object sender, RoutedEventArgs e)
        {
            if(GeoLocation.Instance.geoposition != null)
            {
                Map.Instance.TurnOnMap(GeoLocation.Instance.geoposition.Coordinate.Longitude, GeoLocation.Instance.geoposition.Coordinate.Latitude);
            }
        }
    }
}