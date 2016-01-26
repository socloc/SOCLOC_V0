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
        
        public enum typeOfGetter {getPosition=1};
        bool shouldThreadWork = true;
        int interval = 0;
        typeOfGetter type;
        public CancellationTokenSource tokenSource = new CancellationTokenSource();

        public IntervalGetter(int interv, typeOfGetter tp )
        {
            
            interval = interv;
            type = tp;
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
                            await Task.Delay(interval * 1000, tokenSource.Token);
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
