using MediatR;

namespace DayMemory.Core.Commands
{
    public class DeleteTagCommand : IRequest
    {
        public string? TagId { get; set; }
    }
}