using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto
{
    public class CrawlerDetailDto
    {
       public int id { get; set; }
        public int category_id { get; set; }
        public string category_name { get; set; }
        public string source_name { get; set; }
        public int industry_id { get; set; }
        public string industry_owned { get; set; }
        public bool is_valid { get; set; }
        public bool is_sucess_fetch { get; set; }
        public DateTime? run_time { get; set; }
        public int run_count { get; set; }
        public int  news_count { get; set; }
        public string source_url { get; set; }
    }
}
