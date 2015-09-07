using System;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using System.Windows;
using System.ComponentModel;
using System.Collections.Generic;
using App96.Models;

namespace Cnblogs.Service
{
    public class Feed
    {
        [XmlElement("title")]
        public string Title { get; set; }

        [XmlElement("id")]
        public string Id { get; set; }

        [XmlElement("updated")]
        public DateTime Updated { get; set; }
    }

    [XmlRoot("feed", Namespace = "http://www.w3.org/2005/Atom")]
    public class NewsFeed : Feed
    {
        [XmlElement("entry")]
        public List<CnBlogsEntry> Items { get; set; }
    }

    [XmlRoot("feed", Namespace = "http://www.w3.org/2005/Atom")]
    public class HomeFeed : Feed
    {
        [XmlElement("entry")]
        public SearchBlogger[] Items { get; set; }
    }

    [XmlRoot("feed", Namespace = "http://www.w3.org/2005/Atom")]
    public class SearchBloggerFeed : Feed
    {
        [XmlElement("entry")]
        public SearchBlogger[] Items { get; set; }
    }

    [XmlRoot("feed", Namespace = "http://www.w3.org/2005/Atom")]
    public class BloggerFeed : Feed
    {
        [XmlElement("logo")]
        public string Logo { get; set; }

        [XmlElement("author")]
        public Author AuthorInfo { get; set; }

        [XmlElement("postcount")]
        public string PostCount { get; set; }

        [XmlElement("entry")]
        public CnBlogsEntry[] Items { get; set; }
    }

    public class Author
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("uri")]
        public string Uri { get; set; }

        [XmlElement("avatar")]
        public string Avatar { get; set; }
    }

    public class CnBlogsEntry : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        [XmlElement("id")]
        public string Id { get; set; }
        
        private string title;

        [XmlElement("title")]
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                NotifyPropertyChanged("Title");
            }
        }
        

        public string TitleDisplay
        {
            get
            {
                if (!string.IsNullOrEmpty(Title))
                {
                    return ChineseWordHelper.GetString(Title, 14);
                }
                return Title;
            }
        }


        private string summary;

        [XmlElement("summary")]
        public string Summary
        {
            get { return summary; }
            set { summary = value; NotifyPropertyChanged("Summary"); }
        }
        

        public string DescImage
        {
            get
            {
                string url = "";
                if (!string.IsNullOrEmpty(Summary))
                {
                    url = getImageUrl(Summary);
                }
                return url;
            }
        }

        private string name;
        public string Name
        {
            get
            {
                if (AuthorInfo != null && string.IsNullOrEmpty(AuthorInfo.Name))
                {
                    if (!string.IsNullOrEmpty(AuthorInfo.Name))
                    {
                        return AuthorInfo.Name;
                    }
                }
                return SourceName??"博客园";
            }
            set
            {
                name = value;
                NotifyPropertyChanged("Name");
            }
        }

        private string avatar;
        public string Avatar
        {
            get
            {
                if (AuthorInfo != null)
                {
                    if (!string.IsNullOrEmpty(AuthorInfo.Avatar))
                    {
                        return AuthorInfo.Avatar;
                    }
                    //return string.Format("http://pic.cnblogs.com/avatar/a{0}.jpg", Id);
                }
                return "http://images2015.cnblogs.com/news_topic/20170223124948632-724058622.png";
            }
            set
            {
                avatar = value;
                NotifyPropertyChanged("Avatar");
            }
        }

        [XmlElement("link")]
        public Link Link { get; set; }

        [XmlElement("published")]
        public DateTime Published { get; set; }

        [XmlElement("updated")]
        public DateTime Updated { get; set; }

        [XmlElement("author")]
        public Author AuthorInfo { get; set; }

        [XmlElement("blogapp")]
        public string BlogApp { get; set; }

        [XmlElement("diggs")]
        public int Diggs { get; set; }

        [XmlElement("views")]
        public long Views { get; set; }

        [XmlElement("comments")]
        public long Comments { get; set; }

        [XmlElement("sourceName")]
        public string SourceName { get; set; }

        public static string getImageUrl(string html)
        {
            string url = "";
            var match = Regex.Match(html, "<img\\b[^<>]*?\\bsrc\\s*=\\s*[\"']?\\s*(?<imgUrl>[^\\s\"'<>]*)[^<>]*?/?\\s*>");
            if (match != null)
            {
                url = match.Groups["imgUrl"].Value;
            }
            return url;
        }

        [XmlElement("content")]
        public string Content { get; set; }

        //private Visibility _loadMoreVisibility = Visibility.Collapsed;
        //public Visibility LoadMoreVisibility
        //{
        //    get
        //    {
        //        return _loadMoreVisibility;
        //    }
        //    set
        //    {
        //        _loadMoreVisibility = value;
        //        NotifyPropertyChanged("LoadMoreVisibility");
        //    }
        //}
    }

    public class Topic
    {

    }

    public class Link
    {
        [XmlAttribute("href")]
        public string Uri { get; set; }
    }

    [XmlRoot("string")]
    public class PostBody
    {
        [XmlText]
        public string Content { get; set; }
    }

    public class NewsBody
    {
        public string Title { get; set; }

        public string SourceName { get; set; }

        public string SubmitDate { get; set; }

        public string Content { get; set; }

        public string ImageUrl { get; set; }

        public string PrevNews { get; set; }

        public string NextNews { get; set; }
    }


    public class SearchBlogger
    {
        [XmlElement("id")]
        public string Id { get; set; }

        [XmlElement("title")]
        public string Title { get; set; }

        [XmlElement("updated")]
        public DateTime Updated { get; set; }

        [XmlElement("blogapp")]
        public string BlogApp { get; set; }

        [XmlElement("avatar")]
        public string Avatar { get; set; }

        [XmlElement("postcount")]
        public long PostCount { get; set; }
    }

    public enum ServiceType
    {
        /// <summary>
        /// 首页
        /// </summary>
        Home,
        /// <summary>
        /// 热门新闻
        /// </summary>
        Hot,
        /// <summary>
        /// 最新新闻
        /// </summary>
        Recent,
        /// <summary>
        /// 推荐新闻
        /// </summary>
        Recommend,
        /// <summary>
        /// 新闻评论
        /// </summary>
        NewsComment,
        /// <summary>
        /// 博客正文的评论
        /// </summary>
        PostComment,
        /// <summary>
        /// 新闻的正文
        /// </summary>
        Content,
        /// <summary>
        /// 搜索博主
        /// </summary>
        Search,
        /// <summary>
        /// 文章详细内容
        /// </summary>
        PostContent,
        /// <summary>
        /// 博主页面的文章
        /// </summary>
        Blogger,
    }

    public class NewsData
    {
        /// <summary>
        /// 新闻的id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 每页的新闻个数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 当前的页码
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 搜索的关键字
        /// </summary>
        public string Key { get; set; }

        public NewsData()
        {
            PageSize = 20;
            PageIndex = 1;
        }

        public NewsData(int page, int size)
        {
            this.PageIndex = page;
            this.PageSize = size;
        }
    }
}
