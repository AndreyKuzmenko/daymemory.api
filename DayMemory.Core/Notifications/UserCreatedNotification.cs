using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayMemory.Core.Notifications
{
    public class UserCreatedNotification : INotification
    {
        public string? UserId { get; set; }
    }
}
