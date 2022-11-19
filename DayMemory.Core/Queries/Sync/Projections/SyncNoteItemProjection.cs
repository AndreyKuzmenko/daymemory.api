﻿using DayMemory.Core.Queries.Categories.Projections;
using DayMemory.Core.Queries.Projections;

namespace DayMemory.Core.Queries.Sync.Projections
{
    public class SyncNoteItemProjection
    {
        public string? Id { get; set; }

        public long ModifiedDate { get; set; }

        public SyncItemStatus Status { get; set; }

        public NoteItemProjection? Item { get; set; }
    }
}