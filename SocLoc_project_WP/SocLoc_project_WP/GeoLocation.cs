using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace SocLoc_project_WP
{
    /// <summary>
    /// GeoLocation class is implemented by singleton Pattern. This class is needed to get geolocation of device or friends' devices.
    /// </summary>
    class GeoLocation
    {
        private static GeoLocation single_oInstance = null;
        public Geoposition geoposition;
        public delegate void AfterGetLocation();
        public static event AfterGetLocation AfterGetLocationEvent;
        public static GeoLocation Instance
        {
            get
            {
                if (single_oInstance == null)
                    single_oInstance = new GeoLocation();
                return single_oInstance;
            }
        }
        public async void getOwnLocation()
        {
            Geolocator geolocator = new Geolocator();
            geolocator.DesiredAccuracyInMeters = 50;

            try
            {
                geoposition = await geolocator.GetGeopositionAsync(
                maximumAge: TimeSpan.FromMinutes(5),
                timeout: TimeSpan.FromSeconds(10)
                );
                AfterGetLocationEvent();
            }
            catch (Exception ex)
            {
                if ((uint)ex.HResult == 0x80004004)
                {
                    // the application does not have the right capability or the location master switch is off
                    //StatusTextBlock.Text = "location  is disabled in phone settings.";
                }
                //else
                {
                    // something else happened acquring the location
                }
            }
        }

    }
}
