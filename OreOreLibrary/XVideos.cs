using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.IO;

namespace OreOreLibrary
{
    public static class XVideos
    {
        /// <summary>
        /// 動画本体URLを返す
        /// </summary>
        /// <param name="url">動画再生ページURL</param>
        /// <returns>動画本体URL</returns>
        public static XvideosInfo ParseVideoUrl(string url)
        {
            using (var httpclient = new HttpClient())
            {
                if (httpclient.GetAsync(url).Result.IsSuccessStatusCode)
                {
                    var html = httpclient.GetStringAsync(url).Result;
                    var match = Regex.Match(html, @"flv_url=(?<url>.*?);",
                        RegexOptions.Singleline);
                    var videoUrl = HttpUtility.UrlDecode(match.Groups["url"].Value);
                    return new XvideosInfo()
                    {
                        VideoUrl = videoUrl,
                        VideoTitle = url
                    };
                }
                else
                    return null;
            }
        }
    }

    public class XvideosInfo
    {
        public string VideoUrl { get; internal set; }

        private string videoTitle;
        public string VideoTitle
        {
            get{return this.videoTitle;}
            set{this.videoTitle = Path.GetFileNameWithoutExtension(value);}

        }
    }
}
