using AngleSharp.Dom;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using Dal;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WebCrawler.Utility;
using WebCrawlerConsole;
using WebCrawlerConsole.DoMain;

namespace WebCrawler.Services
{
    class Program
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static void Main(string[] args)
        {
            log.Info("文章采集系统开始...");
            try
            {
                //Csdn
                Crawler CsdnCrawler = new CsdnCrawler();
                Task csdnTask = new Task(() => CsdnCrawler.DoCrawler());

                Task[] task_arry = {
                    csdnTask
                };

                foreach (var task in task_arry)
                    task.Start();

                Task.WaitAll(task_arry);
            }
            catch (Exception ex)
            {
                log.Error($"程序异常:{ex.StackTrace}");
            }
            log.Info("采集结束");
            Console.ReadKey();
        }
    }
}
