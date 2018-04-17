using Dal;
using Dto;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCrawler.Utility;

namespace WebCrawlerConsole.DoMain
{
    
    public class CsdnCrawler:Crawler
    {
        public CsdnCrawler()
        {
        }

        IQueryable<Fry_Tag> SmmCrawlerDetails;
        Fry_Tag current_crawler;
        public int Count { get; set; }
        /// <summary>
        /// 结束采集
        /// </summary>
        bool crawler_end = false;
        public override void DoCrawler()
        {
            try
            {
               
                log.Info($"采集开始***************************");
                Stopwatch watch = Stopwatch.StartNew();
                var sourceUrl = "https://so.csdn.net/so/search/s.do?q=React&t=blog&o=&s=&l=";
                BeginCrawler(sourceUrl);
                watch.Stop();
                log.Info($"采集结束,共采集{Count}条...共耗时{watch.ElapsedMilliseconds}毫秒********");
                log.Info("*******************************************************************************************");
                //crawlerManager.Save();
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
                    log.Error($"{current_crawler.TagName}采集异常,【document加载失败】");
                    return;
                }
                var jsonStr = document.Body.InnerHtml;
                if (string.IsNullOrEmpty(jsonStr))
                {
                    log.Error($"{current_crawler.TagName}采集异常,【json返回为空】");
                    return;
                }
                jsonStr = jsonStr.Substring(jsonStr.IndexOf("[{"));
                jsonStr = jsonStr.Substring(0, jsonStr.LastIndexOf("}]") + 2);
                //List<SmmNewsInfo> newsList;
                //try
                //{
                //    newsList = JsonHelper.DeserializeObject<List<SmmNewsInfo>>(jsonStr);
                //}
                //catch
                //{
                //    log.Error($"{current_crawler.SourcesName}采集异常,json解析失败");
                //    return;
                //}

                //foreach (var news in newsList)
                //{
                //    var index = newsList.IndexOf(news);
                //    //判断第一条新闻时间,如果未更新则结束
                //    //如果第一次运行则采集时间2017年9月1日以后的新闻
                //    var news_release_time = ConvertHelper.ConvertTimeSpanToDateTime(news.RenewDate);
                //    if (current_crawler.RunCount > 0)
                //    {
                //        //if (cur_page == 1 && index == 0)
                //        //{
                //        //    if (news_release_time <= RecentNewsTime)
                //        //    {
                //        //        log.Warn($"{current_crawler.SourcesName},新闻内容未更新,结束采集");
                //        //        crawler_end = true;
                //        //        return;
                //        //    }
                //        //}
                //        //else if (news_release_time <= RecentNewsTime)
                //        //{
                //        //    log.Warn($"{current_crawler.SourcesName},新闻内容已到上一次最后采集新闻,结束采集");
                //        //    crawler_end = true;
                //        //    break;
                //        //}
                //    }
                //    else if (news_release_time < new DateTime(2017, 8, 1))
                //    {
                //        log.Warn($"{current_crawler.SourcesName},新闻日期小于2017年8月1日,不再采集");
                //        crawler_end = true;
                //        break;
                //    }
                //    //if (cur_page == 1 && index == 0)
                //    //{
                //    //    current_crawler.RecentNewsTime = news_release_time;
                //    //}
                //    #region 去重

                //    if (ExistsSameNewsByTitle(news.Title))
                //    {
                //        log.Warn($"{ current_crawler.SourcesName},存在相同【标题】的文章:{ news.Title}");
                //        continue;
                //    }
                //    //获取内容
                //    var document_content = GetHtmlDocument(new HttpItem() { URL = $"https://news.smm.cn/news/{news.ID}", });
                //    if (document_content == null)
                //    {
                //        log.Warn($"{ current_crawler.SourcesName},文章详情采集失败,ID:{news.ID}");
                //        continue;
                //    }
                //    var div_news_detail = document_content.QuerySelector("div.news-detail");
                //    if (div_news_detail == null)
                //    {
                //        log.Warn($"{ current_crawler.SourcesName},文章详情div元素采集失败,ID:{news.ID}");
                //        continue;
                //    }
                //    var div_news_detail_p = div_news_detail.QuerySelectorAll("p");
                //    if (div_news_detail_p == null || div_news_detail_p.Count() == 0)
                //    {
                //        log.Warn($"{ current_crawler.SourcesName},文章详情P元素采集失败,ID:{news.ID}");
                //        continue;
                //    }
                //    var random_content = div_news_detail_p[new Random(div_news_detail_p.Count()).Next(div_news_detail_p.Count())].TextContent;
                //    //if (ExistsSameNewsByContent(random_content))
                //    //{
                //    //    log.Warn($"{ current_crawler.SourcesName},存在相同【内容】的文章:{ news.Title}");
                //    //    continue;
                //    //}
                //    #endregion

                //    #region 添加
                //    CrawlerNews crawlerNews = new CrawlerNews()
                //    {
                //        CrawlerDetailId = current_crawler.ID,
                //        CrawlerDetailName = current_crawler.SourcesName,
                //        NewsTitle = news.Title,
                //        NewsAuthor = news.Author,
                //        NewsSummary = "",
                //        NewsThumbnailImg = news.Thumb,
                //        NewsReleaseTime = news_release_time,
                //        NewsSourceLink = $"https://news.smm.cn/news/{news.ID}",
                //        CreateTime = DateTime.Now,
                //        IsOriginal = false,
                //        NewsContent = div_news_detail.InnerHtml,
                //    };
                //    crawlerNewsManager.Add(crawlerNews);
                //    #endregion
                //    Count += 1;
                //}
                //crawlerNewsManager.Save();
            }
            catch (Exception ex)
            {
                log.Error($"【{ current_crawler.TagName}】采集异常,{ex.Message}");
            }
        }
    }


}
