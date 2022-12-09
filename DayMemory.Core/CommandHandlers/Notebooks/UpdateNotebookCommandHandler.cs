using DayMemory.Core.Commands;
using DayMemory.Core.Interfaces.Repositories;
using DayMemory.Core.Models.Exceptions;
using DayMemory.Core.Models.Personal;
using MediatR;
using Microsoft.Extensions.Internal;

namespace DayMemory.Core.CommandHandlers
{
    internal class UpdateNotebookCommandHandler : AsyncRequestHandler<UpdateNotebookCommand>
    {
        private readonly INotebookRepository _notebookRepository;
        private readonly ISystemClock _clock;

        public UpdateNotebookCommandHandler(INotebookRepository notebookRepository, ISystemClock clock)
        {
            _notebookRepository = notebookRepository;
            _clock = clock;
        }

        protected override async Task Handle(UpdateNotebookCommand request, CancellationToken cancellationToken)
        {
            var item = await _notebookRepository.LoadByIdAsync(request.NotebookId!, cancellationToken);
            if (item == null)
            {
                throw new ResourceNotFoundException("Notebook is not found", request.NotebookId!);
            }
            item.Title = request.Title;
            item.ModifiedDate = _clock.UtcNow;
            await _notebookRepository.UpdateAsync(item, cancellationToken);
        }
    }
}
