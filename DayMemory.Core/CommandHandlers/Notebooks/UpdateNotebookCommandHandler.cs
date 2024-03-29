﻿using DayMemory.Core.Commands;
using DayMemory.Core.Interfaces.Repositories;
using DayMemory.Core.Models.Exceptions;
using DayMemory.Core.Models.Personal;
using MediatR;
using Microsoft.Extensions.Internal;

namespace DayMemory.Core.CommandHandlers
{
    internal class UpdateNotebookCommandHandler : IRequestHandler<UpdateNotebookCommand>
    {
        private readonly INotebookRepository _notebookRepository;
        private readonly ISystemClock _clock;

        public UpdateNotebookCommandHandler(INotebookRepository notebookRepository, ISystemClock clock)
        {
            _notebookRepository = notebookRepository;
            _clock = clock;
        }

        public async Task Handle(UpdateNotebookCommand request, CancellationToken cancellationToken)
        {
            var item = await _notebookRepository.LoadByIdAsync(request.NotebookId!, cancellationToken);
            if (item == null || item.UserId != request.UserId)
            {
                throw new ResourceNotFoundException("Notebook is not found", request.NotebookId!);
            }

            item.Title = request.Title;
            item.OrderRank = request.OrderRank;
            item.ShowInReview = request.ShowInReview;
            item.SortingType = request.SortingType;
            item.ModifiedDate = _clock.UtcNow;
            item.IsEncrypted = request.IsEncrypted;
            await _notebookRepository.UpdateAsync(item, cancellationToken);
        }
    }
}
