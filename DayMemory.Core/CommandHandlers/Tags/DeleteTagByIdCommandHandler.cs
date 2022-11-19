using DayMemory.Core.Commands;
using DayMemory.Core.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Internal;

namespace DayMemory.Core.CommandHandlers
{
    public class DeleteTagByIdCommandHandler : IRequestHandler<DeleteTagCommand, Unit>
    {
        private readonly ITagRepository _tagRepository;
        private readonly ISystemClock _clock;

        public DeleteTagByIdCommandHandler(ITagRepository tagRepository, ISystemClock clock)
        {
            _tagRepository = tagRepository;
            _clock = clock;
        }

        public async Task<Unit> Handle(DeleteTagCommand request, CancellationToken cancellationToken)
        {
            var item = await _tagRepository.LoadByIdAsync(request.TagId!, cancellationToken);
            item.IsDeleted = true;
            item.ModifiedDate = _clock.UtcNow;
            await _tagRepository.UpdateAsync(item, cancellationToken);
            return Unit.Value;
        }
    }
}