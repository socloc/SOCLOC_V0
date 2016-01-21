﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using SocLoc_project_WP.Resources;
using System.Windows.Media;
using System.IO;

namespace SocLoc_project_WP
{
    public partial class MainPage : PhoneApplicationPage
    {
        static string userName;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            DatabaseHandler.WhenDownloaded_whenLog += DatabaseHandler_WhenDownloaded_whenLog;
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private void DatabaseHandler_WhenDownloaded_whenLog()
        {
            if (DatabaseHandler.authentification)
            {
                //UserScreen usrScreen = new UserScreen(userName);
                //UserScreen usrScreen = new UserScreen();
                //DataContext = usrScreen;
                //Uri logPageUri = new Uri("/UserScreen.xaml", UriKind.Relative);
                //Frame.Navigate(typeof(UserScreen));
                Deployment.Current.Dispatcher.BeginInvoke(new Action(() => NavigationService.Navigate(new Uri("/UserScreen.xaml?par=" + userName +"_" + DatabaseHandler.userId, UriKind.Relative))));
            }
            else
            {
                Deployment.Current.Dispatcher.BeginInvoke(new Action(() => infoTextBlock.Foreground = new SolidColorBrush(Colors.Red)));
                Deployment.Current.Dispatcher.BeginInvoke(new Action(() => infoTextBlock.Text = "Wrong user name or password !"));
            }
        }

        private void logInButton_Click(object sender, RoutedEventArgs e)
        {
            userName = loginTextBox.Text;
            string password = passwdTextBox.Password.ToString();

            if (userName != "1" && password != "1")
            {
                if (userName != null && password != null)
                {
                    //WaitForAuthentification(userName, password);
                    DatabaseHandler.LogIn(userName, password);
                }
                else
                    infoTextBlock.Text = "Insert user name and/or password";
            }
            else
            {
                //UserScreen usrScreen = new UserScreen(userName);
                //UserScreen usrScreen = new UserScreen();
                //DataContext = usrScreen;
                //NavigationService.Navigate(new Uri("/UserScreen.xaml", UriKind.Relative));
                NavigationService.Navigate(new Uri("/UserScreen.xaml?par=" + '1' + "_" + '1', UriKind.Relative));
            }
            //ServiceReference1.Service1Client cln = new ServiceReference1.Service1Client();
            //cln.GetDataCompleted += Cln_GetDataCompleted;
            //cln.GetDataAsync(userName, password);
        }

        //private async void WaitForAuthentification(string userName, string password)
        //{
        //    await 
        //}

        private void registerTextBlock_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            registerTextBlock.Foreground = new SolidColorBrush(Colors.Red);
        }

        private void registerTextBlock_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            registerTextBlock.Foreground = new SolidColorBrush(Colors.Black);
        }

        private void registerTextBlock_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            RegisterScreen regScreen = new RegisterScreen();
            NavigationService.Navigate(new Uri("/RegisterScreen.xaml", UriKind.Relative));

        }

        //private void Cln_GetDataCompleted(object sender, ServiceReference1.GetDataCompletedEventArgs e)
        //{
        //   if(e.Result == "true")
        //        //user is logged
        //    else
        //        //user is not logged in
        //}


        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}