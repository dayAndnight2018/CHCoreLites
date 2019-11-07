using IDGeneratorLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebLite.Models
{
    public class EntityBase
    {
        public EntityBase()
        {
            Id = IDGenerateFactory.NewID();
            CreateTime = DateTime.Now;
            Deleted = false;
        }

        public long Id { get; set; }

        public DateTime CreateTime { get; set; }

        public bool Deleted { get; set; }
    }
}
