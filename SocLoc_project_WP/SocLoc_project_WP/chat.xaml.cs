using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using SocLoc_project_WP.Chat;

namespace SocLoc_project_WP
{


    public partial class chat : UserControl
    {



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
              
                DatabaseHandler.chat_send(receiver_id, message);

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
            DatabaseHandler.chat_message_receive();
            DatabaseHandler.chat_message_receive1();
            received_messages.Text = chat_class.message2;
            sent_messages.Text = chat_class.message1;


        }
    }
}
