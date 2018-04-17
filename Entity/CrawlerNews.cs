using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class CrawlerNews
    {
        public int ID { get; set; }
        public int CrawlerDetailId { get; set; }
        public string CrawlerDetailName { get; set; }
        public string NewsTitle { get; set; }
        public string NewsSummary { get; set; }
        public string NewsContent { get; set; }
        public string NewsAuthor { get; set; }
        public string NewsThumbnailImg { get; set; }
        public DateTime NewsReleaseTime { get; set; }
        public string NewsSourceLink { get; set; }  //源链接地址
        public DateTime CreateTime { get; set; }
        public int IsOriginal { get; set; } //是否原创
        public int IsDeleted { get; set; }//是否删除 1是 0=否
        public bool IsRead { get; set; }
        public int ApplyType { get; set; }
    }
}
