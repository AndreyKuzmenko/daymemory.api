using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayMemory.Core.Queries.Sync.Projections
{
    public enum SyncItemStatus
    {
        None = 0,

        CreatedOrChanged = 1,

        Deleted = 2,
    }
}
