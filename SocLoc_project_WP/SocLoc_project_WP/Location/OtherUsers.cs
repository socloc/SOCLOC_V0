using Microsoft.Phone.Maps.Controls;

namespace SocLoc_project_WP
{
    abstract class OtherUser
    {
        public int os_userID;
        public double os_lastLong;
        public double os_lastLat;
        public MapLayer pointMapLayer;
        public MapLayer textMapLayer;
        public string name = "";

        public OtherUser()
        {
        }

    }

    class Friend: OtherUser
    {
        public Friend(int usr_id, double last_long, double last_lat)
        {
            os_userID = usr_id;
            os_lastLong = last_long;
            os_lastLat = last_lat;
        }
    }

    class UnknownUser : OtherUser
    {
        public UnknownUser(int usr_id, double last_long, double last_lat)
        {
            os_userID = usr_id;
            os_lastLong = last_long;
            os_lastLat = last_lat;
        }
    }
}