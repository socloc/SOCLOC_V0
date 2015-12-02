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
    public partial class RegisterScreen : PhoneApplicationPage
    {
        public RegisterScreen()
        {
            InitializeComponent();
        }

        private void regInButton_Click(object sender, RoutedEventArgs e)
        {
            string userName = regLoginTextBox.Text;
            string password = regPasswdTextBox.Password.ToString();
            if (userName != "1" && password != "1")
            {
                if (userName != null && password != null)
                {
                    if (DatabaseHandler.RegisterIn(userName, password))
                    {
                        //UserScreen usrScreen = new UserScreen(userName);
                        //DataContext = usrScreen;
                        NavigationService.Navigate(new Uri("UserScreen.xaml", UriKind.Relative));
                    }
                    else
                    {
                        infoTextBlock.Text = "Wrong user name or password !";
                    }
                }
                else
                    infoTextBlock.Text = "Insert user name and/or password";
            }
            else
            {
                //UserScreen usrScreen = new UserScreen(userName);
                //DataContext = usrScreen;
                //NavigationService.Navigate(new Uri("/UserScreen.xaml", UriKind.Relative));
                NavigationService.Navigate(new Uri("/UserScreen.xaml?par=" + userName, UriKind.Relative));
            }

        }

    }
}