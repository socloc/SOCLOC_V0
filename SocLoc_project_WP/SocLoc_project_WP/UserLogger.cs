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

        public UserLogger(string usrnm_C)
        {
            usrnm = usrnm_C;
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
