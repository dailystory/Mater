using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mater.Library
{
    public class PagedList<TValue> : List<TValue>
    {
        public int PageIndex { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int PageOffset
        {
            get
            {
                return (PageIndex * PageSize);
            }
        }
        public int TotalPages
        {
            get
            {
                return (TotalCount + (PageSize - 1)) / PageSize;
            }
        }
    }
}
