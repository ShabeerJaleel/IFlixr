using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using ScrapySharp.Extensions;
using System.Drawing;
using System.Net;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace IFlixr.WebScraper
{
    public class ImageScraper : Scraper
    {
        private static readonly string SearchUrl = "https://www.google.co.uk/search?as_st=y&tbm=isch&hl=en&as_q={0}&safe=images&tbs=iar:t";
  
        public List<string> GetImageLinks(string searchText)
        {
            string url = String.Format(SearchUrl, searchText.Replace(" ", "+"));

            var mainDoc = GetDocument(url);
            var imageFullLinks = mainDoc.DocumentNode.CssSelect("a").Where(x => x.Attributes.Contains("href"))
                                .Select(x => x.Attributes["href"].Value)
                                .Where(x => x.Length < 512)
                                .ToList();

            var links = new List<string>();
            foreach (var imgLink in imageFullLinks)
            {
                int startIndex = imgLink.IndexOf("imgurl");
                if (startIndex != -1)
                {
                    int endIndex = imgLink.IndexOf("&", startIndex);
                    startIndex += 7;
                    links.Add(imgLink.Substring(startIndex, endIndex - startIndex));
                }
            }


            return links;

        }

        public bool DownloadImage(string uri, string fileName)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Timeout = 10000;
            request.ReadWriteTimeout = 30000;
            HttpWebResponse response;
            
            try
            {
                Debug.WriteLine("Downloading Image => " + uri);
                response = (HttpWebResponse)request.GetResponse();


                // Check that the remote file was found. The ContentType
                // check is performed since a request for a non-existent
                // image file might be redirected to a 404-page, which would
                // yield the StatusCode "OK", even though the image was not
                // found.
                if ((response.StatusCode == HttpStatusCode.OK ||
                    response.StatusCode == HttpStatusCode.Moved ||
                    response.StatusCode == HttpStatusCode.Redirect) &&
                    response.ContentType.StartsWith("image", StringComparison.OrdinalIgnoreCase))
                {

                    // if the remote file was found, download it
                    using (Stream inputStream = response.GetResponseStream())
                    using (Stream outputStream = File.OpenWrite(fileName))
                    {
                        byte[] buffer = new byte[4096];
                        int bytesRead;
                        do
                        {
                            bytesRead = inputStream.Read(buffer, 0, buffer.Length);
                            outputStream.Write(buffer, 0, bytesRead);
                        } while (bytesRead != 0);
                    }
                    Debug.WriteLine("Downloaded Image => " + uri);
                    return true;
                }
                else
                {
                    Debug.WriteLine("Some other issues Image => " + uri);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception => " + uri + ". Exception: " + ex.Message);
                return false;
            }

        }


    }
}
