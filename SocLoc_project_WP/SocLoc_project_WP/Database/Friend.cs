using Microsoft.Phone.Maps.Controls;

namespace SocLoc_project_WP
{
    class Friend
    {
        public int f_userID;
        public double f_lastLong;
        public double f_lastLat;
        public MapLayer pointMapLayer;
        public MapLayer textMapLayer;
        public string name = "";


        public Friend(int usr_id, double last_long, double last_lat)
        {
            f_userID = usr_id;
            f_lastLong = last_long;
            f_lastLat = last_lat;
        }
    }
}