using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace NetWork {
    /// <summary>
    /// 封装了网络操作相关的方法
    /// </summary>
    public class NetOperation {

        #region 字段
        /// <summary>
        /// 网络请求超时（毫秒）
        /// </summary>
        public const int RequestTimeOut = 30000;

        /// <summary>
        /// 委托者
        /// </summary>
        public IResultsHandler Excutor {
            get;
            set;
        }

        #endregion

        #region 构造函数
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static NetOperation() {
            System.Net.ServicePointManager.DefaultConnectionLimit = 512;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public NetOperation() {
            System.Net.ServicePointManager.DefaultConnectionLimit = 512;
        }
        #endregion

        #region 方法
        /// <summary>
        /// 检测网络连接是否可用
        /// </summary>
        /// <returns></returns>
        public static bool CheckNetState() {
            return NativeMethods.InternetGetConnectedState(0, 0);
        }

        /// <summary>
        /// 检查是否接入到因特网
        /// </summary>
        /// <returns></returns>
        public static bool CheckInternet() {
            bool result = false;
            int errorCount = 0;
            try {

                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://www.baidu.com");
                Ping ping = new Ping();
                IPStatus states=ping.Send(httpWebRequest.Address.Host,3000).Status;
                if (states == IPStatus.Success) {
                    result = true;
                }
                else if (states == IPStatus.TimedOut) {
                    errorCount++;
                    result = CheckInternet();
                }
                else {
                    result = false;
                }

            }
            catch (Exception ex) {
                OutputLog(ex);
            }

            if (errorCount > 3) {
                return false;
            }
            else {
                return true;
            }

        }

        /// <summary>
        /// 在浏览器中打开网址
        /// </summary>
        /// <param name="url"></param>
        public static void OpenInBrowser(string url) {
            try {
                Process.Start(url);
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 输出日志类
        /// </summary>
        /// <param name="ex"></param>
        public static void OutputLog(Exception ex) {
            Console.WriteLine(ex.Message);
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="url"></param>
        /// <param name="desPath"></param>
        public static bool DownloadFile(string url, string desPath) {
            Stream stream = Stream.Null;
            Stream responseStream = Stream.Null;
            WebResponse response = null;
            try {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                response = request.GetResponse();
                responseStream = response.GetResponseStream();
                stream = File.Open(desPath, FileMode.Create);
                byte[] buffer = new byte[0x200];
                uint num = 0;
                for (num = (uint)responseStream.Read(buffer, 0, 0x200); num > 0; num = (uint)responseStream.Read(buffer, 0, 0x200)) {
                    stream.Write(buffer, 0, (int)num);
                }
                request.Abort();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally {
                if (stream != null) {
                    stream.Close();
                }
                if (response != null) {
                    response.Close();
                }
                if (responseStream != null) {
                    responseStream.Dispose();
                }
            }
        }

        /// <summary>
        /// 从网络获取图片
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static BitmapImage GetBitmapImageByUrl(string url) {
            BitmapImage bitmapImage = null;
            try {
                WebClient webClient = new WebClient();
                byte[] array = webClient.DownloadData(url);
                if (array.Length > 256) {
                    bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = new MemoryStream(array);
                    bitmapImage.EndInit();
                }
            }
            catch (Exception ex) {
                OutputLog(ex);
            }
            return bitmapImage;
        }

        /// <summary>
        /// 下载网络图片到本地
        /// </summary>
        /// <param name="logoUrl"></param>
        /// <param name="localPath"></param>
        /// <returns></returns>
        public static string SaveBitMapImageByUrl(string logoUrl, string localPath) {
            string text = string.Empty;
            try {
                WebClient webClient = new WebClient();
                byte[] array = webClient.DownloadData(logoUrl);
                if (array.Length > 512) {
                    BinaryWriter binaryWriter = new BinaryWriter(File.Create(localPath));
                    binaryWriter.Write(array);
                    binaryWriter.Close();
                }
            }
            catch (Exception ex) {
                OutputLog(ex);
            }
            return text;
        }

        /// <summary>
        /// 根据请求获取网络内容
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postdata"></param>
        public void GetWebContent(string url, string postdata, int httptag) {
            if (!NetOperation.CheckNetState()) {
                DidFailedRequest(NetworkState.NotConnected, httptag);
                return;
            }
            if (!NetOperation.CheckInternet()) {
                DidFailedRequest(NetworkState.Anomaly, httptag);
                return;
            }

            string textResults = string.Empty;
            try {
                byte[] byteArray = Encoding.UTF8.GetBytes(postdata);

                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.Timeout = RequestTimeOut;
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentLength = byteArray.Length;

                Stream newStream = httpWebRequest.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);
                newStream.Close();

                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.UTF8);
                textResults = streamReader.ReadToEnd();
                streamReader.Close();

                httpWebResponse.Close();
                httpWebRequest.Abort();

                if (textResults.Length > 0) {
                    DidFinishRequest(textResults, httptag);
                    return;
                }
            }
            catch (WebException ex) {
                if (ex.Status == WebExceptionStatus.Timeout) {
                    //网络超时
                    DidFailedRequest(NetworkState.TimeOut, httptag);
                    return;
                }
                else {
                    //DidFailedRequest(NetworkState.Anomaly, httptag);
                    OutputLog(ex);
                }
            }
        }

        /// <summary>
        /// 根据请求获取网络内容
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postdata"></param>
        public void GetWebContent(string url, int httptag)
        {
            if (!NetOperation.CheckNetState())
            {
                DidFailedRequest(NetworkState.NotConnected, httptag);
                return;
            }
            if (!NetOperation.CheckInternet())
            {
                DidFailedRequest(NetworkState.Anomaly, httptag);
                return;
            }

            string textResults = string.Empty;
            try
            {

                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.Timeout = RequestTimeOut;
                httpWebRequest.Method = "GET";
                httpWebRequest.ContentLength = 0;

                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.UTF8);
                textResults = streamReader.ReadToEnd();
                streamReader.Close();

                httpWebResponse.Close();
                httpWebRequest.Abort();

                if (textResults.Length > 0)
                {
                    DidFinishRequest(textResults, httptag);
                    return;
                }
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.Timeout)
                {
                    //网络超时
                    DidFailedRequest(NetworkState.TimeOut, httptag);
                    return;
                }
                else
                {
                    //DidFailedRequest(NetworkState.Anomaly, httptag);
                    OutputLog(ex);
                }
            }
        }

        #endregion

        #region 网络请求完成、失败情况下执行
        /// <summary>
        /// 请求正常结束后执行
        /// </summary>
        public void DidFinishRequest(string results, int httptag) {
            if (Excutor != null) {
                JObject jsonObj = (JObject)JsonConvert.DeserializeObject(results);
                var code = jsonObj["code"].ToString();
                if (code == "200") {
                    //成功
                    Excutor.RequestSuccessed(httptag, results);
                }
                else {
                    //错误
                    Excutor.RequestError(httptag, results);
                }
            }
        }
        /// <summary>
        /// 请求失败后执行,0 未连接 1 网络异常 2 网络连接超时
        /// </summary>
        public void DidFailedRequest(NetworkState type, int httptag) {
            if (Excutor != null) {
                Excutor.RequestFailed(httptag, (int)type);
            }
        }
        #endregion
    }
}
