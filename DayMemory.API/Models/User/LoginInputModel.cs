﻿using System.ComponentModel.DataAnnotations;

namespace DayMemory.Web.Api.Models
{
    public class LoginInputModel
    {
        [Required(ErrorMessage = "E-mail is required."), MaxLength(128), EmailAddress(ErrorMessage = "E-mail address is invalid")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password is required."), MaxLength(128)]
        public required string Password { get; set; }
    }
}
