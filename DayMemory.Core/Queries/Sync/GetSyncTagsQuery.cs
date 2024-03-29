﻿using DayMemory.Core.Queries.Tags.Projections;
using DayMemory.Core.Queries.Projections;
using DayMemory.Core.Queries.Sync.Projections;
using MediatR;
using System.Text.Json.Serialization;

namespace DayMemory.Core.Queries
{
    public class GetSyncTagsQuery : IRequest<SyncListProjection<SyncTagProjection>>
    {
        [JsonIgnore]
        public string? UserId { get; set; }

        public long? LastSyncDateTime { get; set; }
    }
}