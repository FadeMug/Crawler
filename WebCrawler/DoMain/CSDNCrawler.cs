using Dal;
using Dto;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCrawler.Utility;

namespace WebCrawler.DoMain
{

    public class CsdnCrawler : Crawler
    {
        public CsdnCrawler()
        {
            tagManager = new Repository<Fry_Tag>();
            articleManager = new Repository<Fry_Article>();
            //获取所有国家电网板块
            fryTags = tagManager.FindBy(r => r.Id > 0);
        }

        IQueryable<Fry_Tag> fryTags;
        Fry_Article current_crawler = new Fry_Article();
        public int Count { get; set; }
        /// <summary>
        /// 结束采集
        /// </summary>
        bool crawler_end = false;
        public override void DoCrawler()
        {
            try
            {
                current_crawler.Domain = "https://blog.csdn.net";
                current_crawler.Source = "Csdn";
                foreach (var tag in fryTags)
                {
                    current_crawler.TagId = tag.Id;
                    current_crawler.Tag = tag.TagName;
                    log.Info($"采集开始***************************");
                    Stopwatch watch = Stopwatch.StartNew();
                    //var sourceUrl = $"https://so.csdn.net/so/search/s.do?q={tag.TagName}&t=blog&o=&s=&l=";
                    //var document = GetHtmlDocument(new HttpItem() { URL = sourceUrl, });
                    //if (document == null)
                    //{
                    //    log.Error($"{current_crawler.Tag}采集异常,【document加载失败】");
                    //    return;
                    //}
                    //var pagination = document.QuerySelector("div.csdn-pagination");
                    for (var page = 1; page <= 50; page++)
                    {
                        if (crawler_end) break;
                        var urlhz = "";
                        if (tag.TagName == "C#")
                        {
                            urlhz = $"https://so.csdn.net/so/search/s.do?p={page}&q=C%23&t=blog&domain=&o=&s=&u=&l=&f=&rbg=0";
                        }
                        else
                        {
                            urlhz = $"https://so.csdn.net/so/search/s.do?p={page}&q={tag.TagName}&t=blog&domain=&o=&s=&u=&l=&f=&rbg=0";
                        }
                        BeginCrawler(urlhz);
                    }
                    //var sourceUrl = $"https://so.csdn.net/so/search/s.do?p=1&q={tag.TagName}&t=blog&domain=&o=&s=&u=&l=&f=&rbg=0";
                    //BeginCrawler(sourceUrl);
                    watch.Stop();
                    log.Info($"采集结束,共采集{Count}条...共耗时{watch.ElapsedMilliseconds}毫秒********");
                    log.Info("*******************************************************************************************");
                    //crawlerManager.Save();
                }
            }
            catch (Exception ex)
            {
                log.Error($"采集失败,{ex.Message}");
            }
        }

        public void BeginCrawler(string url)
        {
            try
            {
                var document = GetHtmlDocument(new HttpItem() { URL = string.Format(url), });
                if (document == null)
                {
                    log.Error($"{current_crawler.Tag}采集异常,【document加载失败】");
                    return;
                }
                var container = document.QuerySelector("div.main-container");
                if (container == null)
                {
                    log.Error($"{current_crawler.Tag}采集异常,【container加载失败】");
                    return;
                }
                var titleContainer = container.QuerySelector("div.search-list-con");
                if (titleContainer == null)
                {
                    log.Error($"{current_crawler.Tag}采集异常,【titleContainer加载失败】");
                    return;
                }
                var titleList = titleContainer.QuerySelectorAll("dl.search-list");
                if (titleList == null || titleList.Count() == 0)
                {
                    log.Error($"{current_crawler.Tag}采集异常,【titleList加载失败】");
                    return;
                }
                var newsCount = titleList.Length;
                for (var index = 0; index < newsCount; index++)
                {
                    var news = titleList[index];
                    //判断第一条新闻时间,如果未更新则结束
                    //如果第一次运行则采集时间2017年1月1日以后的新闻

                    var dt_title = news.QuerySelector("dt");
                    if (dt_title == null)
                    {
                        log.Error($"{current_crawler.Tag}采集异常,【文章列表dt元素加载失败】");
                        return;
                    }
                    //标题去重
                    var checkTitle = dt_title.TextContent.Trim().Replace(" ", "").Replace("\t", "").Replace("\n", "");
                    if (ExistsSameNewsByTitle(checkTitle))
                    {
                        log.Warn($"{ current_crawler.Tag},存在相同【标题】的文章:{ checkTitle}");
                        continue;
                    }
                    current_crawler.Title = checkTitle;
                    var title_a = dt_title.QuerySelector("a");
                    if (title_a == null)
                    {
                        log.Error($"{current_crawler.Tag}采集异常,【文章列表列表a元素加载失败】");
                        return;
                    }
                    var search_detail = news.QuerySelector("dd.search-detail");
                    if (search_detail == null)
                    {
                        log.Error($"{current_crawler.Tag}采集异常,【文章列表详情加载失败】");
                        return;
                    }
                    var Abstract = search_detail.TextContent.Trim();
                    current_crawler.Abstract = Abstract;
                    //标题/文章详情/文章来源
                    var newsDetailUrl = "";
                    var link = title_a.GetAttribute("href");
                    newsDetailUrl = link;
                    current_crawler.RealUrl = newsDetailUrl;

                    var document_content = GetHtmlDocument(
                                new HttpItem() { URL = newsDetailUrl, });
                    if (document_content == null)
                    {
                        log.Error($"{ current_crawler.Tag},文章详情采集失败,地址:{newsDetailUrl}");
                        continue;
                    }
                    var title = "";
                    var artContent = "";
                    var auther = "";
                    DateTime date = new DateTime();
                    var div_container = document_content.QuerySelector("div#article_details");
                    if (div_container == null)
                    {
                        var div_article = document_content.QuerySelector("article");
                        if (div_article == null)
                        {
                            log.Error($"{ current_crawler.Tag},文章详情采集失败,地址:{newsDetailUrl}");
                            continue;
                        }
                        var div_title = div_article.QuerySelector("h1.csdn_top");
                        if (div_title == null)
                        {
                            log.Error($"{ current_crawler.Tag},文章标题集失败,地址:{newsDetailUrl}");
                            continue;
                        }
                        var articleContent = div_article.QuerySelector("div#article_content");
                        if (articleContent == null)
                        {

                            log.Error($"{ current_crawler.Tag},文章内容采集失败,地址:{newsDetailUrl}");
                            continue;
                        }
                        var articleTime = div_article.QuerySelector("span.time");
                        if (articleTime == null)
                        {
                            log.Error($"{ current_crawler.Tag},发布时间采集失败,地址:{newsDetailUrl}");
                            continue;
                        }
                        var dateStr = articleTime.TextContent.Trim();
                        date = Convert.ToDateTime(dateStr);
                        title = div_title.TextContent.Trim();
                        artContent = articleContent.InnerHtml;
                        current_crawler.PageContext = artContent;
                        current_crawler.Title_ = title;
                        current_crawler.ReleaseDate = date;
                    }
                    else
                    {
                        var div_title = div_container.QuerySelector("div.article_title");
                        if (div_title == null)
                        {
                            log.Error($"{ current_crawler.Tag},{newsDetailUrl},文章标题div标签获取失败");
                            continue;
                        }
                        title = div_title.TextContent.Trim();

                        var articleContent = div_container.QuerySelector("div#article_content");
                        if (articleContent == null)
                        {
                            log.Error($"{ current_crawler.Tag},{newsDetailUrl},文章内容获取失败");
                            continue;
                        }
                        var articleTime = div_container.QuerySelector("span.link_postdate");
                        if (articleTime == null)
                        {
                            log.Error($"{ current_crawler.Tag},发布时间采集失败,地址:{newsDetailUrl}");
                            continue;
                        }
                        var dateStr = articleTime.TextContent.Trim();
                        date = Convert.ToDateTime(dateStr);
                        artContent = articleContent.InnerHtml;
                        current_crawler.Title_ = title;
                        current_crawler.PageContext = artContent;
                        current_crawler.ReleaseDate = date;

                    }
                    var userContainer = document_content.QuerySelector("div#container");
                    if (userContainer == null)
                    {
                        var userinfo = document_content.QuerySelector("div.user_info");
                        if (userinfo == null)
                        {
                            log.Error($"{ current_crawler.Tag},{newsDetailUrl},用户信息获取失败");
                        }
                        var div_author = userinfo.QuerySelector("dd h3 a");
                        if (div_author == null)
                        {
                            log.Error($"{ current_crawler.Tag},{newsDetailUrl},作者获取失败");
                        }
                        auther = div_author.TextContent.Trim();
                        current_crawler.Author = auther;
                    }
                    else
                    {
                        var panel_Profile = userContainer.QuerySelector("div#panel_Profile");
                        if (panel_Profile == null)
                        {
                            var blog_title = userContainer.QuerySelector("div#blog_title");
                            if (blog_title == null)
                            {
                                log.Error($"{ current_crawler.Tag},{newsDetailUrl},作者获取失败");
                            }
                        }
                        var authorName = panel_Profile.QuerySelector("div.user_info h3");
                        if (authorName == null)
                        {
                            log.Error($"{ current_crawler.Tag},{newsDetailUrl},用户信息获取失败");
                        }
                        auther = authorName.TextContent.Trim();
                        current_crawler.Author = auther;
                    }

                    #region 去重

                    if (ExistsSameNewsByTitle(title))
                    {
                        log.Warn($"{ current_crawler.Tag},存在相同【标题】的文章:{ news.TextContent}");
                        continue;
                    }

                    #endregion

                    #region 添加
                    using (var db = new FryEntities())
                    {
                        var article = new Fry_Article()
                        {
                            TagId = current_crawler.TagId,
                            Tag = current_crawler.Tag,
                            Source = current_crawler.Source,
                            Title = current_crawler.Title,
                            Abstract = current_crawler.Abstract,
                            RealUrl = current_crawler.RealUrl,
                            Domain = current_crawler.Domain,
                            IsVerify = 1,
                            PageContext = current_crawler.PageContext,
                            Author = current_crawler.Author,
                            Title_ = current_crawler.Title_,
                            CreateTime = DateTime.Now,
                            ReleaseDate = current_crawler.ReleaseDate
                        };
                        db.Fry_Article.Add(article);
                        db.SaveChanges();
                    }
                    #endregion
                    Count += 1;
                }
                //crawlerNewsManager.Save();
            }
            catch (Exception ex)
            {
                log.Error($"【{ current_crawler.Tag}】采集异常,{ex.Message}");
            }
        }
    }


}
