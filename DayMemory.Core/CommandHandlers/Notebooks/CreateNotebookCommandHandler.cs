using DayMemory.Core.Commands;
using DayMemory.Core.Interfaces.Repositories;
using DayMemory.Core.Models.Personal;
using DayMemory.Core.Services;
using MediatR;
using Microsoft.Extensions.Internal;

namespace DayMemory.Core.CommandHandlers
{
    internal class CreateNotebookCommandHandler : IRequestHandler<CreateNotebookCommand, string>
    {
        private readonly INotebookRepository _tagRepository;
        private readonly ISystemClock _clock;

        public CreateNotebookCommandHandler(INotebookRepository tagRepository, ISystemClock clock)
        {
            _tagRepository = tagRepository;
            _clock = clock;
        }

        public async Task<string> Handle(CreateNotebookCommand request, CancellationToken cancellationToken)
        {
            var item = new Notebook()
            {
                Id = request.NotebookId,
                UserId = request.UserId,
                Title = request.Title,
                OrderRank= request.OrderRank,
                CreatedDate = _clock.UtcNow,
                ModifiedDate = _clock.UtcNow,
            };

            await _tagRepository.CreateAsync(item, cancellationToken);
            return item.Id!;
        }
    }
}
