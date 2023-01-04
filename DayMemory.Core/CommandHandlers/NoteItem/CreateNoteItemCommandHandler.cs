using DayMemory.Core.Commands;
using DayMemory.Core.Interfaces.Repositories;
using DayMemory.Core.Models.Personal;
using DayMemory.Core.Services;
using MediatR;
using Microsoft.Extensions.Internal;

namespace DayMemory.Core.CommandHandlers
{
    internal class CreateNoteItemCommandHandler : IRequestHandler<CreateNoteItemCommand, string>
    {
        private readonly INoteItemRepository _noteItemRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly ISystemClock _clock;

        public CreateNoteItemCommandHandler(INoteItemRepository noteItemRepository, ILocationRepository locationRepository, ISystemClock clock)
        {
            _noteItemRepository = noteItemRepository;
            this._locationRepository = locationRepository;
            _clock = clock;
        }

        public async Task<string> Handle(CreateNoteItemCommand request, CancellationToken cancellationToken)
        {
            string? locationId = null;
            if (request.Location != null && request.Location.Address != null)
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

            var item = new NoteItem()
            {
                Id = request.NoteId == null ? StringUtils.GenerateUniqueString() : request.NoteId,
                NotebookId = request.NotebookId,
                UserId = request.UserId!,
                LocationId = locationId,
                Text = request.Text ?? "",
                Date = DateTimeOffset.FromUnixTimeMilliseconds(request.Date),
                CreatedDate = _clock.UtcNow,
                ModifiedDate = _clock.UtcNow,
            };


            item.SetFiles(request.Files ?? new string[] { });

            await _noteItemRepository.CreateAsync(item, cancellationToken);



            return item.Id;
        }
    }
}
