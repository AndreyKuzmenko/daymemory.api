﻿using DayMemory.Core.Queries.Tags.Projections;
using DayMemory.Core.Queries.Projections;
using DayMemory.Core.Queries.Sync.Projections;
using MediatR;

namespace DayMemory.Core.Queries
{
    public class GetSyncNotebooksQuery : IRequest<IList<SyncNotebookProjection>>
    {
        public string? UserId { get; set; }

        public long? LastSyncDateTime { get; set; }
    }
}