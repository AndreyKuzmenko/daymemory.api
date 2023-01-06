using MediatR;
using System.Text.Json.Serialization;

namespace DayMemory.Core.Commands
{
    public class DeleteTagCommand : IRequest
    {
        public required string TagId { get; set; }

        [JsonIgnore]
        public required string UserId { get; set; }
    }
}