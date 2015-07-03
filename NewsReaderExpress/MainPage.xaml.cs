using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using NewsReaderExpress.Resources;
using System.Net.Http;
using HtmlAgilityPack;
using NewsReaderExpress.UtilityClasses;
using System.Threading.Tasks;

namespace NewsReaderExpress
{
    public partial class MainPage : PhoneApplicationPage
    {

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            //DataSource source = DataSource.returnInstance();
            DataSource source = new DataSource();
            source.getPublicNews();
            await Task.Delay(TimeSpan.FromSeconds(2));
            NavigationService.Navigate(new Uri("/PanoramaPage1.xaml", UriKind.RelativeOrAbsolute));
        }

    }
}