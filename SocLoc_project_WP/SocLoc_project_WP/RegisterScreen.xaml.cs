using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;

namespace SocLoc_project_WP
{
    public partial class RegisterScreen : PhoneApplicationPage
    {
        public RegisterScreen()
        {
            InitializeComponent();
            DatabaseHandler.WhenDownloaded_whenReg += DatabaseHandler_WhenDownloaded_whenReg;
        }

        private void DatabaseHandler_WhenDownloaded_whenReg()
        {
            if (DatabaseHandler.isRegisterSuccess)
            {
                //Application.Current.Dispatcher
                Deployment.Current.Dispatcher.BeginInvoke(new Action(() => infoTextBlock.Text = "Register was succesfull, please confirm your registration !"));
                Deployment.Current.Dispatcher.BeginInvoke(new Action(() => goToMainPageTextBlock.Visibility= Visibility.Visible));
            }
            else
            {
                Deployment.Current.Dispatcher.BeginInvoke(new Action(() => infoTextBlock.Foreground = new SolidColorBrush(Colors.Red)));
                Deployment.Current.Dispatcher.BeginInvoke(new Action(() => infoTextBlock.Text = "Unsuccesfull registration ! Please check your personal data !"));
            }
        }

        private void regInButton_Click(object sender, RoutedEventArgs e)
        {
            string name = regNameTextBox.Text;
            string password = regPasswdTextBox.Password.ToString();
            string surname = regSurnameTextBox.Text;
            string city = regCityTextBox.Text;
            string phone = regPhoneTextBox.Text;
            string email = regEmailTextBox.Text;
            if (name != null && password != null && surname != null && surname != null && city != null && phone != null && email != null)
            {
                DatabaseHandler.RegisterIn(name, surname, city, phone, email, password);
            }
            else
            {
                infoTextBlock.Foreground = new SolidColorBrush(Colors.Red);
                infoTextBlock.Text = "Probably you left empty one or more of the textbox";
            }
        }

        private void goToMainPageTextBlock_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }
    }
}