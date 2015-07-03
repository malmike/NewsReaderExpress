using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using HtmlAgilityPack;
using NewsReaderExpress.UtilityClasses;
using System.Net.Http;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace NewsReaderExpress
{
    public partial class FullStory : PhoneApplicationPage
    {
        public FullStory()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            try{
                string headLine = NavigationContext.QueryString["headLine"];
                string details = NavigationContext.QueryString["details"];
                string imagePath = NavigationContext.QueryString["imagePath"];

                DetailedNews news = new DetailedNews();
                news.headLine = headLine;
                news.newsDetails = details;
                news.image = imagePath;

                Details.Text = news.newsDetails;
                HeadLine.Text = news.headLine;
                try
                {
                    NewsImage.Source = new BitmapImage(new Uri(news.image));
                }
                catch (Exception)
                {

                }
            }
            catch
            {
                string parameterValue = NavigationContext.QueryString["parameter"];
                GetFullStory(parameterValue);
            }
            
            //HeadLine.Text = parameterValue;
        }

         protected async void GetFullStory(string website)
        {
            DetailedNews news = new DetailedNews();
            string htmlPage = "";
            using (var client = new HttpClient())
            {
                htmlPage = await client.GetStringAsync(website);
            }
            // var getHTMLWeb = new HtmlWeb();
            // var document = getHTMLWeb.Load(website);

            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(htmlPage);

            var heading = "";
            var mainImg = "http://www.newvision.co.ug";
            var newsDetails = "";

            List<HtmlNode> title = document.DocumentNode.Descendants().Where(x => (x.Name == "div" && x.Attributes["id"] != null && x.Attributes["id"].Value.Contains("ctl00_ContentPlaceHolder1_main_head"))).ToList();
            List<HtmlNode> img = document.DocumentNode.Descendants().Where(x => (x.Name == "div" && x.Attributes["id"] != null && x.Attributes["id"].Value.Contains("ctl00_ContentPlaceHolder1_maiming"))).ToList();
            List<HtmlNode> details = document.DocumentNode.Descendants().Where(x => (x.Name == "div" && x.Attributes["id"] != null && x.Attributes["id"].Value.Contains("ctl00_ContentPlaceHolder1_divdtls"))).ToList();

            foreach (var headLine in title)
            {
                heading = headLine.InnerText;

            }

            try
            {
                foreach (var image in img)
                {
                    mainImg += image.Descendants("img").ToList()[0].GetAttributeValue("src", null);
                }

            }
            catch (Exception)
            {
                mainImg = "Image missing";
            }

            foreach (var detail in details)
            {
                newsDetails = detail.InnerText;
            }

            
            news.headLine = heading.Trim();
            news.image = mainImg;
            news.newsDetails = newsDetails.Trim();

            Details.Text = news.newsDetails;
            HeadLine.Text = news.headLine;
            try 
            {
                NewsImage.Source = new BitmapImage(new Uri(news.image));
            }
            catch(Exception)
            {

            }
            
        }

    }
}