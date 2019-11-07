using IDGeneratorLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SqlLite
{
    public class EntityBase
    {
        public long ID { get; set; } = IDGenerateFactory.NewID();

        public EntityState State { get; set; } = EntityState.right;

        public DateTime CreateTime { get; } = DateTime.UtcNow;

        public DateTime LastModifyTime { get; set; } = DateTime.UtcNow;

        public Nullable<long> Auditor { get; set; }

        public Nullable<DateTime> AuditTime { get; set; }

    }
}
