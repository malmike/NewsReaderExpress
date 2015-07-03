using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace NewsReaderExpress
{
    public class SharedInformation
    {
        private static SharedInformation instance = new SharedInformation();

        public ObservableCollection<Datum> newsData = new ObservableCollection<Datum>();

        public void setNewsData(ObservableCollection<Datum> newsData)
        {
            this.newsData = newsData;
        }

        public ObservableCollection<Datum> getNewsData()
        {
            return this.newsData;
        }

        private SharedInformation() { }

        public static SharedInformation getInstance()
        {
            return instance;
        }

    }

}
