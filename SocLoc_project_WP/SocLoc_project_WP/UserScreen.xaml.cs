using System;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using System.Device.Location;
using Windows.Devices.Geolocation;
//using Microsoft.Phone.Controls.Maps;
using Microsoft.Phone.Maps.Controls;
//using Microsoft.Phone.Controls.Maps;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Reflection.Emit;
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
        public Geoposition lastGeoposition;
        private List<Friend> listWithFriendsLoc = new List<Friend>();
        public UserScreen()
        {
            //userName = usrNm;
            InitializeComponent();
            //IntervalGetter.typeOfGetter.getPosition
            int interval = 12;
            CreatePostionGetter(interval);
            GeoLocation.AfterGetLocationEvent += GeoLocation_AfterGetLocationEvent;
            DatabaseHandler.WhenFriendsLoc += DatabaseHandler_WhenFriendsLoc;
            DatabaseHandler.WhenNoFriends += DatabaseHandler_WhenNoFriends;
            DatabaseHandler.WhenGetName += DatabaseHandler_WhenGetName;
        }

        private void DatabaseHandler_WhenGetName(int userId, string name)
        {
            foreach(Friend friend in listWithFriendsLoc)
            {
                if (friend.f_userID == userId)
                    friend.name = name;
            }
        }

        private void DatabaseHandler_WhenNoFriends()
        {
            MessageBox.Show("Your friends have no current location !");
        }

        private void DatabaseHandler_WhenFriendsLoc(List<Friend> friendsList)
        {

            listWithFriendsLoc = friendsList;
            Deployment.Current.Dispatcher.BeginInvoke(new Action(() => seeFriendsLoc_button.Foreground = new SolidColorBrush(Colors.Red)));
            Deployment.Current.Dispatcher.BeginInvoke(new Action(() => seeFriendsLoc_button.Content = "Show Friends"));
            foreach (Friend friend in listWithFriendsLoc)
            {
                DatabaseHandler.GetFriendName(friend.f_userID);
            }
        }

        private void GeoLocation_AfterGetLocationEvent()
        {
            lastGeoposition = GeoLocation.Instance.geoposition;
            DatabaseHandler.GiveLocation(userId, lastGeoposition.Coordinate.Latitude, lastGeoposition.Coordinate.Longitude);
        }

        private void CreatePostionGetter(int interval)
        {
            intervalGetter = new IntervalGetter(interval, IntervalGetter.typeOfGetter.getPosition);
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
            //while (listWithFriendsLoc.Count == 0 && GeoLocation.Instance.geoposition == null)
            //{
            //    int i = 1;
            //}
            //ShowFriendsLoc();
            //myMap.Layers.Add(myLocationLayer);

            //Microsoft.Phone.Maps.Controls.MapPolygon mapPolygon = new Microsoft.Phone.Maps.Controls.MapPolygon();
            //mapPolygon.
            //myMap.MapElements.Add()
            //var pin = new MapIcon()
            //{
            //    Location = location.Coordinate.Point,
            //    Title = "You are here!",
            //    Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/pin.png")),
            //    NormalizedAnchorPoint = new Point() { X = 0.32, Y = 0.78 },
            //};
            //MapLayer layer1 = new MapLayer();
            //Microsoft.Phone.Maps.Controls.
            ////Pushpin pushpin1 = new Pushpin();
            //MapIcon
            //pushpin1.GeoCoordinate = MyGeoPosition;
            //pushpin1.Content = "My car";
            //MapOverlay overlay1 = new MapOverlay();
            //overlay1.Content = pushpin1;
            //overlay1.GeoCoordinate = MyGeoPosition;
            //layer1.Add(overlay1);

            //myMap.Layers.Add(layer1);
            //var watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
            //watcher.MovementThreshold = 20;
            //watcher.StatusChanged += new EventHandler<GeoPositionStatusChangedEventArgs>(watcher_StatusChanged);
            //watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(watcher_PositionChanged);
            //watcher.Start();
        }

        private void ShowFriendsLoc()
        {
            myMap.Visibility = Visibility.Visible;

            GeoCoordinate geoCoordinate = new GeoCoordinate();
            geoCoordinate.Latitude = GeoLocation.Instance.geoposition.Coordinate.Latitude;
            geoCoordinate.Longitude = GeoLocation.Instance.geoposition.Coordinate.Longitude;
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
                mapOverlay.GeoCoordinate = new GeoCoordinate(friend.f_lastLat, friend.f_lastLong);
                mapOverlay.PositionOrigin = new Point(0, 0.5);
                MapLayer myLocationLayer = new MapLayer();
                friend.pointMapLayer = myLocationLayer;
                myLocationLayer.Add(mapOverlay);
                myCircle.MouseEnter += new MouseEventHandler((sender, e) => MyCircle_MouseEnter(sender, e, friend));
                myCircle.MouseLeave += new MouseEventHandler((sender, e) => MyCircle_MouseLeave(sender, e, friend));
                //myCircle.MouseLeave += MyCircle_MouseLeave;
                myMap.Layers.Add(myLocationLayer);
            }
        }

        private void MyCircle_MouseLeave(object sender, MouseEventArgs e, Friend friend)
        {
            Friend locFriend = (Friend)friend;
            myMap.Layers.Remove(locFriend.textMapLayer);
        }

        private void MyCircle_MouseEnter(object sender, MouseEventArgs e, object friend)
        {
            Friend locFriend = (Friend)friend;
            GeoCoordinate geoCoordinate = new GeoCoordinate();
            geoCoordinate.Latitude = locFriend.f_lastLat;
            geoCoordinate.Longitude = locFriend.f_lastLong;
            TextBlock textBlock = new TextBlock();
            textBlock.Foreground = new SolidColorBrush(Colors.Red);
            textBlock.FontSize = 20;
            if(locFriend.name != "")
                textBlock.Text = locFriend.name;
            else
                textBlock.Text = locFriend.f_userID.ToString();
            MapOverlay myLocationOverlay2 = new MapOverlay();
            myLocationOverlay2.Content = textBlock;
            myLocationOverlay2.PositionOrigin = new Point(0.5, 0.5);
            myLocationOverlay2.GeoCoordinate = geoCoordinate;
            MapLayer myLocationLayer2 = new MapLayer();
            locFriend.textMapLayer = myLocationLayer2;
            myLocationLayer2.Add(myLocationOverlay2);
            myMap.Layers.Add(myLocationLayer2);
        }

    }
}