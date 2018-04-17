using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto.Request
{
    public class UpdateCrawlerDetail
    {
        public int id { get; set; }
        public string name { get; set; }
        public int category_id { get; set; }
        public int industry_id { get; set; }
        public string source_url { get; set; }
        public bool is_valid { get; set; }
    }

    public class AddCrawlerDetail
    {
        public string name { get; set; }
        public int category_id { get; set; }
        public int industry_id { get; set; }
        public string source_url { get; set; }
        public bool is_valid { get; set; }
    }
}
