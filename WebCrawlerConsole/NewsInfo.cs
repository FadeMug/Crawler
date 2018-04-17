using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawlerConsole
{
    public class NewsInfo
    {
        public int id { get; set; }
        public string title { get; set; }
        public string link_url { get; set; }
        public DateTime release_time { get; set; }
    }
}
