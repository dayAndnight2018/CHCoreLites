using System;
using System.Collections.Generic;
using System.Text;
using WebLite.ValidationAttributes;

namespace WebLite.Models
{
    public class PageUtil
    {
        [MinInteger(ReferInteger = 1)]
        public int Page { get; set; }

        [MinInteger(ReferInteger = 1)]
        public int Num { get; set; }
    }
}
