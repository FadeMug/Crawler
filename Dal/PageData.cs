using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    public class PageData<T>
    {
        public long rows { get; set; }
        public long pages { get; set; }
        public long cur_page { get; set; }
        public int size { get; set; }
        public IList<T> list { get; set; }

        public PageData(long count,int curpage, int pagesize, IList<T> items)
        {
            rows = count;
            list = items;
            cur_page = curpage;
            size = pagesize;
            if (items != null && items.Count > 0)
            {
                pages = count / pagesize;
                if (count % pagesize > 0)
                {
                    pages += 1;
                }
            }
        }
        public PageData(int pageCount, IList<T> items)
        {
            this.pages = pageCount;
            this.list = items;
        }
    }
}
