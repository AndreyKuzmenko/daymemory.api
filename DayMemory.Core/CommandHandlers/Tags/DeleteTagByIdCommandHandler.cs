using DayMemory.Core.Commands;
using DayMemory.Core.Interfaces.Repositories;
using MediatR;

namespace DayMemory.Core.CommandHandlers
{
    public class DeleteTagByIdCommandHandler : IRequestHandler<DeleteTagCommand, Unit>
    {
        private readonly ITagRepository _tagRepository;

        public DeleteTagByIdCommandHandler(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public async Task<Unit> Handle(DeleteTagCommand request, CancellationToken cancellationToken)
        {
            await _tagRepository.DeleteByIdAsync(request.TagId!, cancellationToken);
            return Unit.Value;
        }
    }
}