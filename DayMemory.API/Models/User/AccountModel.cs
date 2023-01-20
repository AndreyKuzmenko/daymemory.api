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
            this.IsEncryptionEnabled = user.IsEncryptionEnabled;
            this.EncryptedText = user.EncryptedText;
        }

        public AccountModel(User user, string token)
        {
            this.Email = user.Email;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.Id = user.Id;
            this.Token = token;
            this.IsEncryptionEnabled = user.IsEncryptionEnabled;
            this.EncryptedText = user.EncryptedText;
        }

        public AccountModel(string token, string email, string firstName, string lastName, bool isEncryptionEnabled, string encryptedText)
        {
            this.Email = email;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Token = token;
            this.IsEncryptionEnabled = isEncryptionEnabled;
            this.EncryptedText = encryptedText;
        }

        public string? Id { get; set; }

        public string? Token { get; set; }

        public string? Email { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? ProfileImageId { get; set; }

        public bool IsEncryptionEnabled { get; set; }

        public string? EncryptedText { get; set; }
    }
}
