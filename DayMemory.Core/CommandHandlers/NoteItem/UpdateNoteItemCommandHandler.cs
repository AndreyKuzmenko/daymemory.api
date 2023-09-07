using DayMemory.Core.Commands;
using DayMemory.Core.Interfaces.Repositories;
using DayMemory.Core.Models.Exceptions;
using DayMemory.Core.Models.Personal;
using DayMemory.Core.Services;
using MediatR;
using Microsoft.Extensions.Internal;

namespace DayMemory.Core.CommandHandlers
{
    internal class UpdateNoteItemCommandHandler : AsyncRequestHandler<UpdateNoteItemCommand>
    {
        private readonly INoteItemRepository _noteItemRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly ISystemClock _clock;

        public UpdateNoteItemCommandHandler(INoteItemRepository noteItemRepository, ILocationRepository locationRepository, ISystemClock clock)
        {
            _noteItemRepository = noteItemRepository;
            this._locationRepository = locationRepository;
            _clock = clock;
        }

        protected override async Task Handle(UpdateNoteItemCommand request, CancellationToken cancellationToken)
        {
            var note = await _noteItemRepository.LoadByIdAsync(request.NoteId!, cancellationToken);
            if (note == null || note.UserId != request.UserId)
            {
                throw new ResourceNotFoundException("Note is not found", request.NoteId!);
            }
            
            string? locationId = note.LocationId;
            if (note.Location == null && request.Location != null && request.Location.Address != null)
            {
                var location = new Location()
                {
                    Id = StringUtils.GenerateUniqueString(),
                    Address = request.Location.Address,
                    Latitude = request.Location.Latitude,
                    Longitude = request.Location.Longitude,
                    CreatedDate = _clock.UtcNow,
                    ModifiedDate = _clock.UtcNow
                };
                locationId = location.Id;
                await _locationRepository.CreateAsync(location);
            }

            note.Text = request.Text;
            note.SetFiles(request.MediaFiles ?? Array.Empty<string>());
            note.LocationId = locationId;
            note.NotebookId = request.NotebookId;
            note.Date = DateTimeOffset.FromUnixTimeMilliseconds(request.Date);
            note.IsEncrypted = request.IsEncrypted;
            note.ModifiedDate = _clock.UtcNow;
            await _noteItemRepository.UpdateAsync(note, cancellationToken);
        }
    }
}
