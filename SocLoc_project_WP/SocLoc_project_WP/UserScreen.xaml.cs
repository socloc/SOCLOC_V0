using System;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using System.Device.Location;
using Windows.Devices.Geolocation;
using Microsoft.Phone.Maps.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.Generic;

namespace SocLoc_project_WP
{
    public partial class UserScreen : PhoneApplicationPage
    {
        string userName;
        int userId;
        IntervalGetter intervalGetter;
        public Geoposition lastGeoposition = null;
        private List<Friend> listWithFriendsLoc = new List<Friend>();
        private List<UnknownUser> listWithUnknown = new List<UnknownUser>();

        public UserScreen()
        {
            InitializeComponent();
            int interval = 12;
            CreatePostionGetter(interval);
            GeoLocation.AfterGetLocationEvent += GeoLocation_AfterGetLocationEvent;
            DatabaseHandler.WhenFriendsLoc += DatabaseHandler_WhenFriendsLoc;
            DatabaseHandler.WhenNoFriends += DatabaseHandler_WhenNoFriends;
            DatabaseHandler.WhenGetName += DatabaseHandler_WhenGetName;
            DatabaseHandler.WhenAllLoc += DatabaseHandler_WhenAllLoc;
            DatabaseHandler.WhenErrorOccurs += DatabaseHandler_WhenErrorOccurs;
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

        private void DatabaseHandler_WhenErrorOccurs(string ex)
        {
            Deployment.Current.Dispatcher.BeginInvoke(new Action(() => MessageBox.Show("There were problems with your actions:\n" + ex)));
        }

        private void DatabaseHandler_WhenAllLoc(List<UnknownUser> unknownsList)
        {
            listWithUnknown = unknownsList;
            Deployment.Current.Dispatcher.BeginInvoke(new Action(() => seeAll_button.Foreground = new SolidColorBrush(Colors.Red)));
            Deployment.Current.Dispatcher.BeginInvoke(new Action(() => seeAll_button.Content = "Show All"));
            foreach (UnknownUser unknUs in listWithUnknown)
            {
                DatabaseHandler.GetName(unknUs.os_userID);
            }
        }

        private void DatabaseHandler_WhenGetName(int userId, string name)
        {
            foreach(Friend friend in listWithFriendsLoc)
            {
                if (friend.os_userID == userId)
                    friend.name = name;
            }
            foreach (UnknownUser unknUs in listWithUnknown)
            {
                if (unknUs.os_userID == userId)
                    unknUs.name = name;
            }
        }

        private void DatabaseHandler_WhenNoFriends()
        {
            Deployment.Current.Dispatcher.BeginInvoke(new Action(() => MessageBox.Show("Your friends have no current location !")));
        }

        private void DatabaseHandler_WhenFriendsLoc(List<Friend> friendsList)
        {

            listWithFriendsLoc = friendsList;
            Deployment.Current.Dispatcher.BeginInvoke(new Action(() => seeFriendsLoc_button.Foreground = new SolidColorBrush(Colors.Red)));
            Deployment.Current.Dispatcher.BeginInvoke(new Action(() => seeFriendsLoc_button.Content = "Show Friends"));
            foreach (Friend friend in listWithFriendsLoc)
            {
                DatabaseHandler.GetName(friend.os_userID);
            }
        }

        private void GeoLocation_AfterGetLocationEvent()
        {
            lastGeoposition = GeoLocation.Instance.geoposition;
            DatabaseHandler.GiveLocation(userId, lastGeoposition.Coordinate.Latitude, lastGeoposition.Coordinate.Longitude);
            if (cancel_Interval_button.IsEnabled == false && goToMap_button.IsEnabled == false)
            {
                Deployment.Current.Dispatcher.BeginInvoke(new Action(() => MessageBox.Show("Your Location:\nLat: " + lastGeoposition.Coordinate.Latitude.ToString() + "\nLon: " + lastGeoposition.Coordinate.Longitude.ToString())));
                Deployment.Current.Dispatcher.BeginInvoke(new Action(() => cancel_Interval_button.IsEnabled = true));
                Deployment.Current.Dispatcher.BeginInvoke(new Action(() => goToMap_button.IsEnabled = true));
                Deployment.Current.Dispatcher.BeginInvoke(new Action(() => seeAll_button.IsEnabled = true));
            }
        }

        private void CreatePostionGetter(int interval)
        {
            intervalGetter = new IntervalGetter(interval, IntervalGetter.typeOfGetter.getPosition);
            intervalGetter.getPositionAsync();
        }

        private void Cancel_Interval_Click(object sender, RoutedEventArgs e)
        {
            intervalGetter.tokenSource.Cancel();
        }

        private void goToMap_button_Click(object sender, RoutedEventArgs e)
        {
            if(GeoLocation.Instance.geoposition != null)
            {
                BingMap.Instance.TurnOnMap(GeoLocation.Instance.geoposition.Coordinate.Longitude, GeoLocation.Instance.geoposition.Coordinate.Latitude);
            }
        }

        private void seeFriendsLoc_button_Click(object sender, RoutedEventArgs e)
        {
            if (seeFriendsLoc_button.Content.ToString() != "Show Friends")
            {
                DatabaseHandler.GetYourFriendsLocation(userId);
            }
            else
            {
                ShowFriendsLoc();
            }
        }

        private void ShowFriendsLoc()
        {
            myMap.Visibility = Visibility.Visible;
            GeoCoordinate geoCoordinate;
            if (lastGeoposition != null)
            {
                geoCoordinate = new GeoCoordinate();
                geoCoordinate.Latitude = GeoLocation.Instance.geoposition.Coordinate.Latitude;
                geoCoordinate.Longitude = GeoLocation.Instance.geoposition.Coordinate.Longitude;
            }
            else
                geoCoordinate = GetAverageFromLoc(listWithFriendsLoc);

            myMap.Center = geoCoordinate;
            myMap.ZoomLevel = 14;

            foreach (Friend friend in listWithFriendsLoc)
            {
                Ellipse myCircle = new Ellipse();
                myCircle.Fill = new SolidColorBrush(Colors.Blue);
                myCircle.Height = 20;
                myCircle.Width = 20;
                myCircle.Opacity = 50;
                MapOverlay mapOverlay = new MapOverlay();
                mapOverlay.Content = myCircle;
                mapOverlay.GeoCoordinate = new GeoCoordinate(friend.os_lastLat, friend.os_lastLong);
                mapOverlay.PositionOrigin = new Point(0, 0.5);
                MapLayer myLocationLayer = new MapLayer();
                friend.pointMapLayer = myLocationLayer;
                myLocationLayer.Add(mapOverlay);
                myCircle.MouseEnter += new MouseEventHandler((sender, e) => MyCircle_MouseEnter(sender, e, friend, true));
                myCircle.MouseLeave += new MouseEventHandler((sender, e) => MyCircle_MouseLeave(sender, e, friend, true));
                myMap.Layers.Add(myLocationLayer);
            }
        }

        private void seeAll_button_Click(object sender, RoutedEventArgs e)
        {
            if (seeAll_button.Content.ToString() != "Show All")
            {
                DatabaseHandler.GetAllLastLocation();
            }
            else
            {
                ShowAllLoc();
            }
        }

        private void ShowAllLoc()
        {
            myMap.Visibility = Visibility.Visible;
            GeoCoordinate geoCoordinate = new GeoCoordinate();
            geoCoordinate.Latitude = GeoLocation.Instance.geoposition.Coordinate.Latitude;
            geoCoordinate.Longitude = GeoLocation.Instance.geoposition.Coordinate.Longitude;
            myMap.Center = geoCoordinate;
            myMap.ZoomLevel = 10;

            int range = 500;

            foreach (UnknownUser unknUs in listWithUnknown)
            {
                if (IsInRange(geoCoordinate, unknUs, range))
                {
                    Ellipse myCircle = new Ellipse();
                    myCircle.Fill = new SolidColorBrush(Colors.Blue);
                    myCircle.Height = 20;
                    myCircle.Width = 20;
                    myCircle.Opacity = 50;
                    MapOverlay mapOverlay = new MapOverlay();
                    mapOverlay.Content = myCircle;
                    mapOverlay.GeoCoordinate = new GeoCoordinate(unknUs.os_lastLat, unknUs.os_lastLong);
                    mapOverlay.PositionOrigin = new Point(0, 0.5);
                    MapLayer myLocationLayer = new MapLayer();
                    unknUs.pointMapLayer = myLocationLayer;
                    myLocationLayer.Add(mapOverlay);
                    myCircle.MouseEnter += new MouseEventHandler((sender, e) => MyCircle_MouseEnter(sender, e, unknUs, false));
                    myCircle.MouseLeave += new MouseEventHandler((sender, e) => MyCircle_MouseLeave(sender, e, unknUs, false));
                    myMap.Layers.Add(myLocationLayer);
                }
            }
        }

        private bool IsInRange(GeoCoordinate geoCoordinate, UnknownUser unknUs, int range)
        {
            bool result;
            double distance = 0.0;
            GeoCoordinate unknUsCoord = new GeoCoordinate();
            unknUsCoord.Latitude = unknUs.os_lastLat;
            unknUsCoord.Longitude = unknUs.os_lastLong;
            distance = geoCoordinate.GetDistanceTo(unknUsCoord);

            if (distance > double.Parse(range.ToString()))
                result = false;
            else
                result = true;
            return result;
        }

        private void MyCircle_MouseLeave(object sender, MouseEventArgs e, object otherUser, bool isFriend)
        {
            if (isFriend)
            {
                Friend locFriend = (Friend)otherUser;
                myMap.Layers.Remove(locFriend.textMapLayer);
            }
            else
            {
                UnknownUser locUnknown = (UnknownUser)otherUser;
                myMap.Layers.Remove(locUnknown.textMapLayer);
            }
        }

        private void MyCircle_MouseEnter(object sender, MouseEventArgs e, object otherUser, bool isFriend)
        {
            OtherUser locOtherUser;
            if (isFriend)
                locOtherUser = (Friend)otherUser;
            else
                locOtherUser = (UnknownUser)otherUser;

            GeoCoordinate geoCoordinate = new GeoCoordinate();
            geoCoordinate.Latitude = locOtherUser.os_lastLat;
            geoCoordinate.Longitude = locOtherUser.os_lastLong;
            TextBlock textBlock = new TextBlock();
            textBlock.Foreground = new SolidColorBrush(Colors.Red);
            textBlock.FontSize = 30;
            textBlock.FontWeight = FontWeights.Bold;

            if(locOtherUser.name != "")
                textBlock.Text = locOtherUser.name;
            else
                textBlock.Text = locOtherUser.os_userID.ToString();

            MapOverlay myLocationOverlay2 = new MapOverlay();
            myLocationOverlay2.Content = textBlock;
            myLocationOverlay2.PositionOrigin = new Point(0.5, 0.5);
            myLocationOverlay2.GeoCoordinate = geoCoordinate;
            MapLayer myLocationLayer2 = new MapLayer();
            locOtherUser.textMapLayer = myLocationLayer2;
            myLocationLayer2.Add(myLocationOverlay2);
            myMap.Layers.Add(myLocationLayer2);
        }

        private GeoCoordinate GetAverageFromLoc(List<Friend> listWithFriendsLoc)
        {
            double avgLat = 0.0;
            double avgLong = 0.0;
            double sumOfLat = 0.0;
            double sumOfLong = 0.0;
            GeoCoordinate geoCord = new GeoCoordinate();

            foreach (Friend friend in listWithFriendsLoc)
            {
                sumOfLat += friend.os_lastLat;
                sumOfLong += friend.os_lastLong;
            }
            avgLat = sumOfLat / listWithFriendsLoc.Count;
            avgLong = sumOfLong / listWithFriendsLoc.Count;
            geoCord.Latitude = avgLat;
            geoCord.Longitude = avgLong;
            return geoCord;
        }
    }

}