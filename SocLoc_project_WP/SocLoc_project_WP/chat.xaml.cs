using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;



namespace SocLoc_project_WP
{


    public partial class chat : UserControl
    {
        int userId;
        string userName;



        public chat()
        {
            InitializeComponent();

        }

     



        private void send_Click(object sender, RoutedEventArgs e)
        {
            string receiver_id = email_to_box.Text;
            string message = to_send_box.Text;

         

            if (receiver_id != null && message != null)
            {
              
                DatabaseHandler.chat_send(userId, receiver_id, message);

            }

            else
            {
                //infoTextBlock.Text = "Insert user name and/or password";
               
            }
        }

        private void connect_button_Click(object sender, RoutedEventArgs e)
        {

        }



        private void refresh_Click(object sender, RoutedEventArgs e)
        {


            /*string text = "[{*id*:3,*sender_id*:3,*receiver_id*:2,*contents*:*Tresc mojej wiadomosci*,*created_at*:*2016-01-26 13:30:58*,*updated_at*:*2016-01-26 13:30:58*}";
            char[] delimiterChars = { ' ', ',', '.', ':', '*' };
            string[] words = text.Split(delimiterChars);

            foreach (string s in words)
            {
                my_messages.Text = my_messages.Text + '\n' + s;
            }*/



        }
    }
}
