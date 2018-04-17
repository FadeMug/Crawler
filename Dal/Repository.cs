using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    public class Repository<T> : AbstractRepository<FryEntities, T> where T : class
    {

    }
}
