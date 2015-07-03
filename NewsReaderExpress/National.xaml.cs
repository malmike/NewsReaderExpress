using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using NewsReaderExpress.GetData;
using HtmlAgilityPack;
using System.Net.Http;
using NewsReaderExpress.UtilityClasses;
using System.Windows.Media;

namespace NewsReaderExpress
{
    public partial class National : PhoneApplicationPage
    {
        public National()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            NewsItem("http://www.newvision.co.ug/section/53-1-National.html");
            
        }


        public async void NewsItem(string website)
        {
            List<NewsItem> getNewsItems = new List<UtilityClasses.NewsItem>();
            string htmlPage = "";
            string mainImg = "http://www.newvision.co.ug";

            

            using (var client = new HttpClient())
            {
                htmlPage = await client.GetStringAsync(website);
            }
            // var getHTMLWeb = new HtmlWeb();
            // var document = getHTMLWeb.Load(website);

            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(htmlPage);

            List<string> aLink = new List<string>();
            List<string> text = new List<string>();
            List<string> image = new List<string>();
            List<string> sampleText = new List<string>();
            var uriLink = "http://www.newvision.co.ug";
            var imgString = "http://www.newvision.co.ug";

            NewsItem newsItem;

            List<HtmlNode> items = document.DocumentNode.Descendants().Where(x => (x.Name == "div" && x.Attributes["class"] != null && x.Attributes["class"].Value.Contains("main-category_section"))).ToList();

            foreach (var item in items)
            {

                var fill = item.Descendants().Where(x =>
                    (x.Name == "a" && x.Attributes["href"] != null) ||
                    (x.Name == "div" && x.Attributes["class"] != null && x.Attributes["class"].Value.Contains("sec_news_synopsis"))).ToList();

                foreach (var link in fill)
                {
                    if (link.Attributes["href"] != null && link.InnerHtml != "Read More..." && link.InnerText != "Read More...")
                    {
                        aLink.Add(uriLink + link.Attributes["href"].Value);
                        imgString += link.Attributes["href"].Value;
                        text.Add(link.InnerHtml);
                        //getImage(imgString);


                        //Label1.Text += uriLink + link.Attributes["href"].Value + "<br/>";
                        //Label1.Text += link.InnerHtml + "<br/>";
                        //Label1.Text += getImage(imgString) + "<br/>"; 

                        if (imgString != "http://www.newvision.co.ug")
                        {
                            //string page = "";
                            using (var soucre = new HttpClient())
                            {
                                try
                                {
                                    htmlPage = await soucre.GetStringAsync(imgString);
                                }
                                catch (Exception)
                                {
                                    mainImg = website;
                                }

                            }

                            HtmlDocument documentImg = new HtmlDocument();
                            documentImg.LoadHtml(htmlPage);

                            List<HtmlNode> img = documentImg.DocumentNode.Descendants().Where(x => (x.Name == "div" && x.Attributes["id"] != null && x.Attributes["id"].Value.Contains("ctl00_ContentPlaceHolder1_maiming"))).ToList();
                            try
                            {
                                foreach (var imageSource in img)
                                {
                                    mainImg += imageSource.Descendants("img").ToList()[0].GetAttributeValue("src", null);
                                }

                            }
                            catch (Exception)
                            {
                                mainImg = "image missing";
                            }

                            image.Add(mainImg);
                        }


                    }
                    else if (link.Attributes["href"] == null)
                    {
                        sampleText.Add(link.InnerHtml);
                        // Label1.Text += link.InnerHtml + "<br/><br/>"; 

                    }
                }
            }

            //foreach (var lk in aLink)
            //{
            //    Label1.Text += lk + "<br/><br/>";
            //}
            //Label1.Text += "-----------------------<br/>";
            string[] lnk = aLink.ToArray();
            string[] txt = text.ToArray();
            string[] imgSource = image.ToArray();
            string[] sample = sampleText.ToArray();

            int limit = lnk.Length;

            //foreach (var lk in lnk)
            //{
            //    Label1.Text += lk + "<br/><br/>";
            //}

            //Label1.Text += "-----------------------<br/>";
            for (int i = 0; i < limit; i++)
            {
                newsItem = new NewsItem();
                newsItem.link = lnk[i];
                newsItem.headLine = txt[i];
                newsItem.imagePath = imgSource[i];
                newsItem.sampleText = sampleText[i];

                //Label1.Text += newsItem.link + "<br/><br/>";
                getNewsItems.Add(newsItem);
            }
            lstMovies.ItemsSource = getNewsItems;
        }

        private void FullStory(object sender, System.Windows.Input.GestureEventArgs e)
        {
 
            string lnk = (lstMovies.SelectedItem as NewsItem).link;
            NavigationService.Navigate(new Uri("/FullStory.xaml?parameter="+lnk, UriKind.RelativeOrAbsolute));
        }
       
    }
}