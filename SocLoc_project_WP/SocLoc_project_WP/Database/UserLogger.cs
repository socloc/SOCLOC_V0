using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocLoc_project_WP
{
    class UserLogger: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string usrnm;
        private int userId;

        public UserLogger(string usrnm_C, int usr_id)
        {
            usrnm = usrnm_C;
            userId = usr_id;
        }
        protected void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public string UserName
        {
            get
            {
                return usrnm;
            }
            set
            {
                usrnm = value;
                RaisePropertyChanged("UserName");
            }
        }
    }
}
