using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto
{
    public class CrawlerPriceDto
    {
        public int Id { get; set; }
        public int VarietyCategoryId { get; set; }
        public string VarietyCategory { get; set; }
        public string VarietyName { get; set; }
        public string MaxPrice { get; set; }
        public string MinPrice { get; set; }
        public string Change { get; set; }
        public string OriginPlace { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string AvgPrice { get; set; }
        public string Unit { get; set; }
        public string Tax { get; set; }
        public string Market { get; set; }
        public string Remark { get; set; }
        public DateTime? CreateTime { get; set; }




    }
}
