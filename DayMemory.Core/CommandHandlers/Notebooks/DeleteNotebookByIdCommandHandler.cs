using DayMemory.Core.Commands;
using DayMemory.Core.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Internal;

namespace DayMemory.Core.CommandHandlers
{
    public class DeleteNotebookByIdCommandHandler : IRequestHandler<DeleteNotebookCommand, Unit>
    {
        private readonly INotebookRepository _notebookRepository;

        private readonly ISystemClock _clock;

        public DeleteNotebookByIdCommandHandler(INotebookRepository tagRepository, ISystemClock clock)
        {
            _notebookRepository = tagRepository;
            _clock = clock;
        }

        public async Task<Unit> Handle(DeleteNotebookCommand request, CancellationToken cancellationToken)
        {
            var item = await _notebookRepository.LoadByIdAsync(request.NotebookId!, cancellationToken);
            if (item != null)
            {
                item.IsDeleted = true;
                item.ModifiedDate = _clock.UtcNow;
                await _notebookRepository.UpdateAsync(item, cancellationToken);
            }
            return Unit.Value;
        }
    }
}