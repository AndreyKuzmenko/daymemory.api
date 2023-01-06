using MediatR;
using System.Text.Json.Serialization;

namespace DayMemory.Core.Commands
{
    public class DeleteTagCommand : IRequest
    {
        public string? TagId { get; set; }

        [JsonIgnore]
        public string? UserId { get; set; }
    }
}