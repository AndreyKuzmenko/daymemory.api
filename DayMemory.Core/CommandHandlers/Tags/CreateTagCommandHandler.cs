using DayMemory.Core.Commands;
using DayMemory.Core.Interfaces.Repositories;
using DayMemory.Core.Models.Personal;
using DayMemory.Core.Services;
using MediatR;
using Microsoft.Extensions.Internal;

namespace DayMemory.Core.CommandHandlers
{
    internal class CreateTagCommandHandler : IRequestHandler<CreateTagCommand, string>
    {
        private readonly ITagRepository _tagRepository;
        private readonly ISystemClock _clock;

        public CreateTagCommandHandler(ITagRepository tagRepository, ISystemClock clock)
        {
            _tagRepository = tagRepository;
            _clock = clock;
        }

        public async Task<string> Handle(CreateTagCommand request, CancellationToken cancellationToken)
        {
            var item = new Tag()
            {
                Id = request.TagId,
                UserId = request.UserId,
                Text = request.Text,
                OrderRank = request.OrderRank,
                CreatedDate = _clock.UtcNow,
                ModifiedDate = _clock.UtcNow,
            };

            await _tagRepository.CreateAsync(item, cancellationToken);
            return item.Id!;
        }
    }
}
