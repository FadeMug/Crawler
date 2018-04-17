using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class CrawlerDetail
    {
        public int ID { get; set; }
        public int CrawlerCategoryId { get; set; }
        public int ArticleCategoryId { get; set; }
        public string CategoryName { get; set; }
        public string SourcesName { get; set; }
        public string SourcesUrl { get; set; }
        public string IndustryOwned { get; set; }
        public int Isvalid { get; set; }
        public DateTime CreateTime { get; set; }
        public int IsSucessFetch { get; set; }
        public DateTime RunTime { get; set; }
        public int RunCount { get; set; }
    }
}
