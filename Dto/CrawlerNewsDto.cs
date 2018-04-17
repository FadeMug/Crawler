using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto
{
    public class CrawlerNewsDto
    {
        public int id { get; set; }
        public int category_id { get; set; }
        public string category_name { get; set; }
        /// <summary>
        /// 所属行业
        /// </summary>
        public string articlecategory_name { get; set; }
        public string source { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public string author { get; set; }
        //文章发布时间
        public DateTime news_release_time { get; set; }
        /// <summary>
        /// 文章抓取时间
        /// </summary>
        public DateTime get_time { get; set; }
        public string source_link { get; set; }
        public bool? is_original { get; set; }

        public bool? is_read { get; set; }

        public int? apply_type { get; set; }
    }
}
