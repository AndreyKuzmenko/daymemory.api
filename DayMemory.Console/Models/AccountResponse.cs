using DayMemory.Core.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayMemory.Console.Models
{
    public class AccountResponse
    {
        public string? Id { get; set; }

        public string? AccessToken { get; set; }

        public string? RefreshToken { get; set; }

        public string? Email { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? ProfileImageId { get; set; }

        public bool IsEncryptionEnabled { get; set; }

        public string? EncryptedText { get; set; }
    }
}
