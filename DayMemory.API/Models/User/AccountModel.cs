using Microsoft.AspNetCore.Identity;
using DayMemory.Core.Models.Common;

namespace DayMemory.Web.Api.Models
{
    public class AccountModel
    {
        public AccountModel(User user, string accessToken, string refreshToken)
        {
            this.Email = user.Email;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.Id = user.Id;
            this.AccessToken = accessToken;
            this.RefreshToken = refreshToken;
            this.IsEncryptionEnabled = user.IsEncryptionEnabled;
            this.EncryptedText = user.EncryptedText;
        }

        public AccountModel(string accessToken, string refreshToken, string email, string firstName, string lastName, bool isEncryptionEnabled, string encryptedText)
        {
            this.Email = email;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.AccessToken = accessToken;
            this.RefreshToken = refreshToken;
            this.IsEncryptionEnabled = isEncryptionEnabled;
            this.EncryptedText = encryptedText;
        }

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
