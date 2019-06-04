using System;
using System.Collections.Generic;
using System.Text;

namespace WebLite.Models
{
    public class EntityBase
    {
        public long Id { get; set; }

        public DateTime CreateTime { get; set; }

        public bool Deleted { get; set; }
    }
}
