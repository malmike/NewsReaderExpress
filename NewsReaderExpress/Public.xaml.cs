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
using Microsoft.Phone.Tasks;
using System.IO;
using System.Windows.Media.Imaging;
using System.Net.Http.Headers;

namespace NewsReaderExpress
{
    public partial class Public : PhoneApplicationPage
    {

        //The Windows.Web.Http.HttpClient class provides the main class for 
        // sending HTTP requests and receiving HTTP responses from a resource identified by a URI.

        SharedInformation info = SharedInformation.getInstance();
        private HttpClient httpClient;
        private HttpResponseMessage response;
        private RadioButton rb;
        MemoryStream ms;

        PhotoChooserTask photoChooserTask;
        public string fileName { get; private set; }

        // Image stream variables
        private MemoryStream photoStream;


        // This is the url that will checked by the php file
        //private String phpAddress = "http://localhost/NewsReaderExpress/insNewsPost.php?";
        private String phpAddress = "http://localhost:21750/NewsReaderExpressPHP/insNewsPost.php?";
        

        // This is the complete url that must be checked and verified by the php file
        // private String phpAddress = "http://localhost/NewsReaderExpress/insNewsPost.php?";

        public Public()
        {
            InitializeComponent();

            photoChooserTask = new PhotoChooserTask();
            photoChooserTask.Completed += new EventHandler<PhotoResult>(photoChooserTask_Completed);

            httpClient = new HttpClient();

            // Add a user-agent header
            var headers = httpClient.DefaultRequestHeaders;

            // HttpProductInfoHeaderValueCollection is a collection of 
            // HttpProductInfoHeaderValue items used for the user-agent header

            headers.UserAgent.ParseAdd("ie");
            headers.UserAgent.ParseAdd("Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");

        }

        private async void Save(object sender, RoutedEventArgs e)
        {

            phpAddress = "http://localhost/NewsReaderExpress/insNewsPost.php"; //?headLine=" + headLine.Text + "&type=" + rb.Content + "&details=" + details.Text + "&fileName=" + fileName;
            phpAddress = "http://localhost:21750/NewsReaderExpressPHP/insNewsPost.php"; 

          
            response = new HttpResponseMessage();

            byte[] image = PhotoStreamToBase64();

            Uri resourceUri;
            if (!Uri.TryCreate(phpAddress.Trim(), UriKind.Absolute, out resourceUri))
            {
                phpStatus.Text = "Invalid URI, please re-enter a valid URI";
                return;
            }
            if (resourceUri.Scheme != "http" && resourceUri.Scheme != "https")
            {
                phpStatus.Text = "Only 'http' and 'https' schemes supported. Please re-enter URI";
                return;
            }
            // ---------- end of test---------------------------------------------------------------------

            string responseText;
            phpStatus.Text = "Waiting for response ...";

            try
            {
                MultipartFormDataContent content = new MultipartFormDataContent();
                content.Add((new StringContent(headLine.Text, System.Text.Encoding.UTF8, "text/plain")), "headLine");
                content.Add((new StringContent((string)rb.Content, System.Text.Encoding.UTF8, "text/plain")), "type");
                content.Add((new StringContent(details.Text, System.Text.Encoding.UTF8, "text/plain")), "details");
                content.Add((new StringContent(fileName, System.Text.Encoding.UTF8, "text/plain")), "fileName");

                //Uploading the image
                var imageContent = new ByteArrayContent(image);
                imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                content.Add(imageContent, "image", headLine.Text+".jpg");
                /***********/

                response = await httpClient.PostAsync(resourceUri, content);
                response.EnsureSuccessStatusCode();
                responseText = await response.Content.ReadAsStringAsync();

            }
            catch (Exception ex)
            {
                // Need to convert int HResult to hex string
                phpStatus.Text = "Error = " + ex.HResult.ToString("X") +
                    "  Message: " + ex.Message;
                responseText = "";
            }
            phpStatus.Text = response.StatusCode + " " + response.ReasonPhrase;

            // now 'responseText' contains the response as a verified text.
            // next 'responseText' is displayed 


            phpStatus.Text = responseText.ToString();

            //DataSource update= DataSource.returnInstance();
            //update.updateNews(phpAddress2);

            NavigationService.Navigate(new Uri("/PanoramaPage1.xaml", UriKind.RelativeOrAbsolute));
        }


        private void newsType(object sender, RoutedEventArgs e)
        {
            rb = sender as RadioButton;
        }

        private void choosephoto_Click(object sender, RoutedEventArgs e)
        {
            photoChooserTask.Show();
        }

        public void photoChooserTask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {


                // initialize the result photo stream
                photoStream = new MemoryStream();
                // Save the stream result (copying the resulting stream)
                e.ChosenPhoto.CopyTo(photoStream);
                // save the original file name
                string filePath = e.OriginalFileName;
                fileName = Path.GetFileName(filePath);

                ms = new MemoryStream();
                // display the chosen picture
                var bitmapImage = new BitmapImage();
                bitmapImage.SetSource(photoStream);
                this.newsphoto.Source = bitmapImage;

            }

        }

        byte[] PhotoStreamToBase64()
        {
            byte[] pixeBuffer = null;
            pixeBuffer = photoStream.ToArray();

            return pixeBuffer;
        }

    }
}
