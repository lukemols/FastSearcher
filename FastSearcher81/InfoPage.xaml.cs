using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Windows.ApplicationModel;
using FastSearcher.Classes.Support;
using System.Windows.Media.Imaging;

namespace FastSearcher81
{
    public partial class InfoPage : PhoneApplicationPage
    {
        public InfoPage()
        {
            InitializeComponent();
            SetUpInterface();
        }

        void SetUpInterface()
        {
            Package package = Package.Current;
            PackageId packageId = package.Id;

            var v = packageId.Version;
            VersionTB.Text = $"{v.Major}.{v.Minor}.{v.Build}.{v.Revision}";

            AuthorTB.Text = packageId.Publisher;

            Visibility lightBackgroundVisibility = (Visibility)Application.Current.Resources["PhoneLightThemeVisibility"];
            string webFile = @"/Assets/Images/WebIcon";
            string twitterFile = @"/Assets/Images/TwitterIcon";

            if (lightBackgroundVisibility == Visibility.Visible)
            {
                webFile += "Black";
                twitterFile += "Black";
            }
            webFile += ".png";
            twitterFile += ".png";

            Uri searchImage = new Uri(webFile, UriKind.Relative);
            webRectImageBrush.ImageSource = new BitmapImage(searchImage);

            Uri twitterImage = new Uri(twitterFile, UriKind.Relative);
            twitterRectImageBrush.ImageSource = new BitmapImage(twitterImage);
        }

        #region Click sui pulsanti
        private void ContactButton_Click(object sender, RoutedEventArgs e)
        {
            EmailHelper.Instance.ComposeEmailToPublisher();
        }

        private void ReviewButton_Click(object sender, RoutedEventArgs e)
        {
            MarketplaceHelper.Instance.ReviewApp();
        }

        private void TwitterButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            TwitterHelper.Instance.GoToTwitterProfile();
        }
        private void WebButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            SiteHelper.Instance.GoToPublisherWebsite();
        }

        #endregion
    }
}