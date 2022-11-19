using Microsoft.AspNetCore.Identity;
using DayMemory.Core.Models.Common;

namespace DayMemory.Web.Api.Models
{
    public class AccountModel
    {
        public AccountModel()
        {

        }

        public AccountModel(User user)
        {
            this.Email = user.Email;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.Id = user.Id;
        }

        public AccountModel(string token, string email, string firstName, string lastName)
        {
            Token = token;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
        }

        public string? Id { get; set; }

        public string? Token { get; set; }

        public string? Email { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? ProfileImageId { get; set; }
    }
}
