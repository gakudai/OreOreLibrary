using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using System.Threading;

namespace OreOreLibrary
{
    public static class Downloder
    {
        
        /// <summary>
        /// ダウンロードメソッド
        /// </summary>
        /// <param name="url">ダウンロードURL</param>
        /// <param name="saveFolderPath">保存場所</param>
        /// <param name="fileName">保存ファイル名</param>
        /// <param name="extension">拡張子</param>
        public static void Download(string url, string saveFolderPath, string fileName, string extension)
        {
            var hashCode = url.GetHashCode();
            using (var httpClient = new HttpClient())
            {
                var streamData = httpClient.GetStreamAsync(url).Result;

                using (var fileStream = new System.IO.FileStream(Path.Combine(saveFolderPath, fileName+ "["+ hashCode +"]" + extension), System.IO.FileMode.Create))
                {
                    streamData.CopyTo(fileStream);
                }
            }

            /*
            //responseにはヘッダー情報とかも入ってるぉ！
            using (var response = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result)
            {
                if (response.IsSuccessStatusCode)
                {
                    using (var fileStream = new System.IO.FileStream(Path.Combine(saveFolderPath, fileName + extension), System.IO.FileMode.Create))
                    {
                        response.Content.CopyToAsync(fileStream).Wait();
                    }
                }
            }
             */
        }
    }

    /// <summary>
    /// 並列ダウンロード
    /// </summary>
    public class ParallelDownloder
    {
        private HashSet<string> completed = new HashSet<string>();

        private readonly List<Task> downlodTask = new List<Task>();
        private Queue<DownloadInfo> waitingDownloder = new Queue<DownloadInfo>();
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="number">並列ダウンロード数</param>
        /// <param name="second">監視時間の間隔</param>
        public ParallelDownloder(int number,int second)
        {
            for (var i = 0; i < number; i++)
            {
                downlodTask.Add(Task.Run(() =>
                {
                    while (true)
                    {
                        if (this.waitingDownloder.Count != 0)
                        {
                            var downloadInfo = waitingDownloder.Dequeue();
                            Downloder.Download(downloadInfo.url, downloadInfo.saveFolderPath, downloadInfo.fileName,downloadInfo.extension);
                        }
                        Thread.Sleep(second * 1000);
                    }
                }));
            }
        }

        /// <summary>
        /// ダウンロード待ちへ追加する
        /// </summary>
        /// <param name="url">ダウンロードURL</param>
        /// <param name="saveFolderPath">保存場所</param>
        /// <param name="fileName">保存名</param>
        public void AddDownlod(string url,string saveFolderPath,string fileName,string extension)
        {
            var info = new DownloadInfo()
            {
                url = url,
                saveFolderPath = saveFolderPath,
                fileName = fileName,
                extension = extension
            };

            if (this.completed.Add(url))
            {
                waitingDownloder.Enqueue(info);
            }
            /*
            if (!waitingDownloder.Contains(info))
                waitingDownloder.Enqueue(info);
            */
        }
    }

    internal class DownloadInfo
    {
        public string url {get; set; }
        public string saveFolderPath {get; set; }
        public string fileName {get; set; }
        public string extension { get; set; }

        public override int GetHashCode()
        {
            return url.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return ((DownloadInfo)obj).url == this.url;
        }
    }
}
