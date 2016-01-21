using System;
using System.Collections.Generic;
using Windows.Devices.Geolocation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Device.Location;
using System.Threading;

namespace SocLoc_project_WP
{
    class IntervalGetter
    {
        public Geoposition lastGeoposition;
        public enum typeOfGetter {getPosition=1};
        bool shouldThreadWork = true;
        int interval = 0;
        typeOfGetter type;
        public CancellationTokenSource tokenSource = new CancellationTokenSource();

        public IntervalGetter(int interv, typeOfGetter tp )
        {
            GeoLocation.AfterGetLocationEvent += GeoLocation_AfterGetLocationEvent;
            interval = interv;
            type = tp;
        }

        private void GeoLocation_AfterGetLocationEvent()
        {
            lastGeoposition = GeoLocation.Instance.geoposition;
        }

        public async Task getPositionAsync()
        {
            try
            {
                switch (type)
                {
                    case typeOfGetter.getPosition:
                        do
                        {
                            await Task.Delay(5000, tokenSource.Token);
                            GeoLocation.Instance.getOwnLocation();
                        } while (!tokenSource.IsCancellationRequested);
                    break;
                }
            }
            catch (TaskCanceledException ex)
            {

            }
            catch (Exception ex)
            {

            }

            //DispatcherTimer dispatcherTimer;
            //switch (type)
            //{
            //    case typeOfGetter.getPosition:
            //        dispatcherTimer = new DispatcherTimer();
            //        await WaitingForTimer(ref dispatcherTimer, interval);
            //        GeoLocation.Instance.getOwnLocation();
            //        break;

            //}

        }

        private Task WaitingForTimer(ref DispatcherTimer dt, int interval)
        {
            int sumOfTicks = 0;
            dt.Interval = new TimeSpan(0, 0, 1);
            dt.Tick += (sender, e) => { Dt_Tick(sender, e, ref sumOfTicks); };
            dt.Start();
            Task task = Task.Run(() =>
            {
                while (sumOfTicks <= interval) ;
            });
            return task;
        }

        private void Dt_Tick(object sender, EventArgs e, ref int sumOfTicks)
        {
            sumOfTicks += 1;
        }
    }
}
