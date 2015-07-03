using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Net.Http;
using System.Collections.ObjectModel;
using NewsReaderExpress.UtilityClasses;

namespace NewsReaderExpress
{
    public partial class ShowPublicNews : PhoneApplicationPage
    {
        
        //DataSource source = DataSource.returnInstance();
        DataSource source = new DataSource();
        

        public ShowPublicNews()
        {
            InitializeComponent();
                     
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            source.getPublicNews();
            lstMovies.ItemsSource = SharedInformation.getInstance().getNewsData();
            
        }

        private void FullStory(object sender, System.Windows.Input.GestureEventArgs e)
        {
            string headLine = (lstMovies.SelectedItem as Datum).Heading;
            string details = (lstMovies.SelectedItem as Datum).Details;
            string imagePath = (lstMovies.SelectedItem as Datum).ImagePath;
           

            NavigationService.Navigate(new Uri("/FullStory.xaml?headLine=" + headLine + "&details=" + details + "&imagePath=" +imagePath, UriKind.RelativeOrAbsolute));
        }

        
         
    }
}