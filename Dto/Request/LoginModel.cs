using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto.Request
{
    public class LoginModel
    {
        public string user_name { get; set; }
        public string user_pwd { get; set; }

        public string url_referer { get; set; }
    }
}
