using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DayMemory.Core.Models.Personal;
using Microsoft.AspNetCore.Identity;

namespace DayMemory.Core.Models.Common
{
    public class User : IdentityUser
    {
        [MaxLength(255)]
        public string? FirstName { get; set; }

        [MaxLength(255)]
        public string? LastName { get; set; }

        public bool IsEncryptionEnabled { get; set; }
    }
}
