using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace NewsReaderExpress
{
    public class Datum
    {
        public string id { get; set; }
        public string Heading { get; set; }
        public string ImagePath { get; set; }
        public string NewsType { get; set; }
        public string Details { get; set; }
        public string Date { get; set; }
    }

    public class RootObject
    {
        public List<Datum> data { get; set; }
    }


    class DataSource
    {

        private HttpClient httpClient;
        HttpResponseMessage response;
        public ObservableCollection<Datum> newsItems { get; set; }

        public ObservableCollection<Datum> DataElements(String json)
        {
            try
            {
                var rootObject = JsonConvert.DeserializeObject<RootObject>(json);
                ObservableCollection<Datum> obItems = new ObservableCollection<Datum>(rootObject.data);
                return obItems;
            }
            catch(Exception)
            {
                return null;
            }
        }

        public async void getPublicNews()
        {
            //string phpAddress = "http://localhost/NewsReaderExpress/viewNewsPost.php";
            string phpAddress = "http://localhost:21750/NewsReaderExpressPHP/viewNewsPost.php";
        
            httpClient = new HttpClient();

            // Add a user-agent header
            var headers = httpClient.DefaultRequestHeaders;

            // HttpProductInfoHeaderValueCollection is a collection of 
            // HttpProductInfoHeaderValue items used for the user-agent header

            headers.UserAgent.ParseAdd("ie");
            headers.UserAgent.ParseAdd("Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");




            response = new HttpResponseMessage();
            Uri resourceUri;
            if (!Uri.TryCreate(phpAddress.Trim(), UriKind.Absolute, out resourceUri))
            {
                return;
            }
            if (resourceUri.Scheme != "http" && resourceUri.Scheme != "https")
            {
                return;
            }
            // ---------- end of test---------------------------------------------------------------------

            string responseText;

            try
            {
                response = await httpClient.GetAsync(resourceUri);

                response.EnsureSuccessStatusCode();

                responseText = await response.Content.ReadAsStringAsync();

            }
            catch (Exception ex)
            {
                // Need to convert int HResult to hex string
                responseText = "Error = " + ex.HResult.ToString("X") +
                    "  Message: " + ex.Message;
                return;
            }
          

            string jsonString = responseText.ToString();

            
            newsItems = DataElements(jsonString);

            SharedInformation share = SharedInformation.getInstance();
            share.setNewsData(newsItems);
            
        }

        //private static DataSource instance = new DataSource();
 
        //private DataSource() { }

        //public static DataSource returnInstance()
        //{
        //    return instance;
        //}

        //public void updateNews(string phpAddress)
        //{
        //    newsItems = new ObservableCollection<Datum>();
        //    getPublicNews(phpAddress);
        //}

        
    }

    public class getStaticNews
    {

    }

}