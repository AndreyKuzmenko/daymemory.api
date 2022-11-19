using DayMemory.Core.Commands;
using DayMemory.Core.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Internal;

namespace DayMemory.Core.CommandHandlers
{
    public class DeleteNoteItemByIdCommandHandler : IRequestHandler<DeleteNoteItemCommand, Unit>
    {
        private readonly INoteItemRepository _noteRepository;
        private readonly ISystemClock _clock;

        public DeleteNoteItemByIdCommandHandler(INoteItemRepository taskRepository, ISystemClock clock)
        {
            _noteRepository = taskRepository;
            _clock = clock;
        }

        public async Task<Unit> Handle(DeleteNoteItemCommand request, CancellationToken cancellationToken)
        {
            var item = await _noteRepository.LoadByIdAsync(request.NoteItemId!, cancellationToken);
            item.IsDeleted = true;
            item.ModifiedDate = _clock.UtcNow;
            await _noteRepository.UpdateAsync(item, cancellationToken);
            return Unit.Value;
        }
    }
}