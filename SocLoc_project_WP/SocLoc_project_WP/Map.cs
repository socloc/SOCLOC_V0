using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Phone.Tasks;
using System.Device.Location;
using Windows.Devices.Geolocation;

namespace SocLoc_project_WP
{
    class Map
    {
        BingMapsTask bingMapsTask = new BingMapsTask();
        private static Map single_oInstance = null;

        public static Map Instance
        {
            get
            {
                if (single_oInstance == null)
                    single_oInstance = new Map();
                return single_oInstance;
            }
        }
        public void TurnOnMap(double longitude, double latitude)
        {
            //bingMapsTask.SearchTerm = "coffee";
            bingMapsTask.Center = new GeoCoordinate(latitude, longitude);
            bingMapsTask.ZoomLevel = 64;
            bingMapsTask.Show();
        }

    }
}
