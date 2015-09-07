using App96.Services;
using Cnblogs.Service;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Xamarin.Forms;


[assembly: Dependency(typeof(FeedService))]
namespace App96.Services
{
    /// <summary>
    /// 数据服务类
    /// http://wcf.open.cnblogs.com/news/help
    /// http://wcf.open.cnblogs.com/blog/help
    /// </summary>
    public class FeedService: IFeedService
    {
        public async Task<TResult> SendRequst<TResult>(ServiceType type, NewsData data) where TResult : class
        {
            try
            {
                HttpClient http = new HttpClient();
                var result = await http.GetAsync(GetRequestUrl(type, data));
                //var stream = await http.GetStreamAsync(GetRequestUrl(type, data));
                XmlSerializer ser = null;
                object obj = null;
                switch (type)
                {
                    case ServiceType.Home:
                    case ServiceType.Hot:
                    case ServiceType.Recent:
                    case ServiceType.Recommend:
                    case ServiceType.PostComment:
                    case ServiceType.NewsComment:
                        ser = new XmlSerializer(typeof(NewsFeed));
                        break;
                    case ServiceType.Content:
                        ser = new XmlSerializer(typeof(NewsBody));
                        break;
                    case ServiceType.Search:
                        ser = new XmlSerializer(typeof(SearchBloggerFeed));
                        break;
                    case ServiceType.Blogger:
                        ser = new XmlSerializer(typeof(BloggerFeed));
                        break;
                    case ServiceType.PostContent:
                        ser = new XmlSerializer(typeof(PostBody));
                        break;
                    default:
                        break;
                }
                obj = ser.Deserialize(await result.Content.ReadAsStreamAsync());
                return obj as TResult;
            }
            catch (Exception se)
            {                
            }
            return default(TResult);
        }

        internal string GetRequestUrl(ServiceType type, NewsData data)
        {
            string url = "";

            switch (type)
            {
                case ServiceType.Hot:
                    url = string.Format("http://wcf.open.cnblogs.com/news/hot/{0}", data.PageSize);
                    break;
                case ServiceType.Recent:
                    url = string.Format("http://wcf.open.cnblogs.com/news/recent/paged/{0}/{1}", data.PageIndex, data.PageSize);
                    break;
                case ServiceType.Recommend:
                    url = string.Format("http://wcf.open.cnblogs.com/news/recommend/paged/{0}/{1}", data.PageIndex, data.PageSize);
                    break;
                case ServiceType.PostComment:
                    url = string.Format("http://wcf.open.cnblogs.com/blog/post/{0}/comments/{1}/{2}", data.Id, data.PageIndex, data.PageSize);
                    break;
                case ServiceType.NewsComment:
                    url = string.Format("http://wcf.open.cnblogs.com/news/item/{0}/comments/{1}/{2}", data.Id, data.PageIndex, data.PageSize);
                    break;
                case ServiceType.Content:
                    url = string.Format("http://wcf.open.cnblogs.com/news/item/{0}", data.Id);
                    break;
                case ServiceType.Search:
                    url = string.Format("http://wcf.open.cnblogs.com/blog/bloggers/search?t={0}", data.Key);
                    break;
                case ServiceType.Blogger:
                    url = string.Format("http://wcf.open.cnblogs.com/blog/u/{0}/posts/{1}/{2}", data.Key, data.PageIndex, data.PageSize);
                    break;
                case ServiceType.Home:
                    url = string.Format("http://wcf.open.cnblogs.com/blog/sitehome/paged/{0}/{1}", data.PageIndex, data.PageSize);
                    break;
                case ServiceType.PostContent:
                    url = string.Format("http://wcf.open.cnblogs.com/blog/post/body/{0}", data.Id);
                    break;
                default:
                    break;
            }
            return url;
        }

        /*
        internal HttpWebRequest _httpWebRequest;
        public void Request(ServiceType type, NewsData data, Action<object, bool> action)
        {
            _httpWebRequest = null;
            _httpWebRequest = (HttpWebRequest)WebRequest.Create(GetRequestUrl(type, data));
            _httpWebRequest.Method = "GET";
            _httpWebRequest.BeginGetResponse((result) => ResponseCallback(result, type, action), _httpWebRequest);
        }

        static object _obj = new object();
        internal void ResponseCallback(IAsyncResult result, ServiceType type, Action<object, bool> action)
        {
            HttpWebRequest request = (HttpWebRequest)result.AsyncState;
            HttpWebResponse httpWebResponse;
            try
            {
                httpWebResponse = (HttpWebResponse)request.EndGetResponse(result);
            }
            catch (WebException we)
            {
                action(we, false);
                return;
            }
            if (httpWebResponse != null)
            {
                lock (_obj)
                {
                    using (var stream = httpWebResponse.GetResponseStream())
                    {
                        try
                        {
                            XmlSerializer ser = null;
                            object obj = null;
                            switch (type)
                            {
                                case ServiceType.Home:
                                case ServiceType.Hot:
                                case ServiceType.Recent:
                                case ServiceType.Recommend:
                                case ServiceType.PostComment:
                                case ServiceType.NewsComment:
                                    ser = new XmlSerializer(typeof(NewsFeed));
                                    break;
                                case ServiceType.Content:
                                    ser = new XmlSerializer(typeof(NewsBody));
                                    break;
                                case ServiceType.Search:
                                    ser = new XmlSerializer(typeof(SearchBloggerFeed));
                                    break;
                                case ServiceType.Blogger:
                                    ser = new XmlSerializer(typeof(BloggerFeed));
                                    break;
                                case ServiceType.PostContent:
                                    ser = new XmlSerializer(typeof(PostBody));
                                    break;
                                default:
                                    break;
                            }
                            obj = ser.Deserialize(stream);
                            action(obj, true);
                        }
                        catch (Exception se)
                        {
                            action(se, false);
                            return;
                        }
                    }
                }
            }
        }
        */
    }
}
