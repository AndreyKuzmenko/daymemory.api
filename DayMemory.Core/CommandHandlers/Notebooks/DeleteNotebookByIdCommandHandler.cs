﻿using DayMemory.Core.Commands;
using DayMemory.Core.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Internal;

namespace DayMemory.Core.CommandHandlers
{
    public class DeleteNotebookByIdCommandHandler : IRequestHandler<DeleteNotebookCommand>
    {
        private readonly INotebookRepository _notebookRepository;

        private readonly ISystemClock _clock;

        public DeleteNotebookByIdCommandHandler(INotebookRepository tagRepository, ISystemClock clock)
        {
            _notebookRepository = tagRepository;
            _clock = clock;
        }

        public async Task Handle(DeleteNotebookCommand request, CancellationToken cancellationToken)
        {
            var item = await _notebookRepository.LoadByIdAsync(request.NotebookId!, cancellationToken);
            if (item != null)
            {
                if (item.UserId != request.UserId)
                {
                    throw new InvalidOperationException("No permission to delete a notebook");
                }

                item.IsDeleted = true;
                item.ModifiedDate = _clock.UtcNow;
                await _notebookRepository.UpdateAsync(item, cancellationToken);
            }
        }
    }
}