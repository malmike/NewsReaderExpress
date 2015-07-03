using HtmlAgilityPack;
using NewsReaderExpress.UtilityClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NewsReaderExpress.GetData
{
    class GetNewsDetails
    {
        public DetailedNews detailedNews { get; private set; }
        protected async void FullStory(string website)
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
                heading = headLine.InnerHtml;

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
                newsDetails = detail.InnerHtml;
            }

            
            news.headLine = heading;
            news.image = mainImg;
            news.newsDetails = newsDetails;

            this.detailedNews = news;

        }
    }
}
