using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto
{
    public static class ApplicationEnums
    {
        public enum CrawlerCategory
        {
            /// <summary>
            /// 微信公众号
            /// </summary>
            WXOfficalAccounts = 1,
            /// <summary>
            /// 微信关键字
            /// </summary>
            WXKeyWords = 2,
            /// <summary>
            /// 百度关键字
            /// </summary>
            BDKeyWords = 3,
            /// <summary>
            /// 行业网站
            /// </summary>
            TradeWebSite = 4,
        }

        public enum ApplyType
        {
            /// <summary>
            /// 收藏
            /// </summary>
            Collect = 1,
            /// <summary>
            /// 待编辑
            /// </summary>
            PendingEdit = 2,
            /// <summary>
            /// 垃圾箱
            /// </summary>
            Dustbin = 3
           
        }

    }
}
