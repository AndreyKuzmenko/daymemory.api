using DayMemory.Core.Commands;
using DayMemory.Core.Interfaces.Repositories;
using DayMemory.Core.Models.Exceptions;
using MediatR;
using Microsoft.Extensions.Internal;

namespace DayMemory.Core.CommandHandlers
{
    internal class UpdateTagCommandHandler : AsyncRequestHandler<UpdateTagCommand>
    {
        private readonly ITagRepository _tagRepository;
        private readonly ISystemClock _clock;

        public UpdateTagCommandHandler(ITagRepository tagRepository, ISystemClock clock)
        {
            _tagRepository = tagRepository;
            _clock = clock;
        }

        protected override async Task Handle(UpdateTagCommand request, CancellationToken cancellationToken)
        {
            var item = await _tagRepository.LoadByIdAsync(request.TagId!, cancellationToken);
            if (item == null || item.UserId != request.UserId)
            {
                throw new ResourceNotFoundException("Tag is not found", request.TagId!);
            }

            item.Text = request.Text;
            item.OrderRank = request.OrderRank;
            item.ModifiedDate = _clock.UtcNow;
            item.IsEncrypted = request.IsEncrypted;
            await _tagRepository.UpdateAsync(item, cancellationToken);
        }
    }
}
