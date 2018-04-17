using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using Dal;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WebCrawler.Utility;

namespace WebCrawlerConsole.DoMain
{
    public abstract class Crawler
    {
        protected static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        protected Repository<Fry_Article> articleManager;
        protected Repository<Fry_Tag> tagManager;
        protected int categoryId;

        protected DateTime compTime = DateTime.Now.Date;
        /// <summary>
        /// 最新文章时间(用来比较文章有没有更新)
        /// </summary>
        public DateTime? RecentNewsTime { get; set; }

        public IQueryable<Fry_Article> InitFetchData()
        {
            return articleManager.FindBy(m => m.ReleaseDate < compTime);
        }

        /// <summary>
        /// 开始采集
        /// </summary>
        public virtual void DoCrawler()
        { }

        /// <summary>
        /// 获取远程调用返回的dom结构体
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public IHtmlDocument GetHtmlDocument(HttpItem item)
        {
            var htmlStr = new HttpHelper().GetHtml(item);
            //解析dom
            HtmlParser htmlParser = new HtmlParser();
            return htmlParser.Parse(htmlStr.Html);
        }

        public HttpResult GetHttpResult(HttpItem item)
        {
            return new HttpHelper().GetHtml(item);
        }

        public bool ExistsSameNewsByTitle(string title, int detailId)
        {
            if (!string.IsNullOrEmpty(title))
            {
                var same_news_titles = new Repository<Fry_Article>().FindBy(m => m.Title == title);
                if (same_news_titles != null && same_news_titles.Count() > 0)
                    return true;
            }
            return false;
        }
        public bool ExistsSameNewsByTitle(string title)
        {
            if (!string.IsNullOrEmpty(title))
            {
                var same_news_titles = new Repository<Fry_Article>().FindBy(m => m.Title == title);
                if (same_news_titles != null && same_news_titles.Count() > 0)
                    return true;
            }
            return false;
        }
        public bool ExistsSameNewsByContent(string content)
        {
            if (!string.IsNullOrEmpty(content))
            {
                var same_news_contents = new Repository<Fry_Article>().FindBy(m => m.PageContext.IndexOf(content) > -1);
                if (same_news_contents != null && same_news_contents.Count() > 0)
                    return true;
            }
            return false;
        }
    }
}
