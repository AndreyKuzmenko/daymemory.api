using DayMemory.Core.Notifications;
using DayMemory.Core.Settings;
using DayMemory.Core.Models.Common;
using DayMemory.Core.Services;
using DayMemory.Web.Api.Models;
using DayMemory.Web.Components.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Web;
using System.IdentityModel.Tokens.Jwt;
using DayMemory.API.Models.User;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using DayMemory.Core.Models.Exceptions;
using DayMemory.Core.Interfaces.Repositories;
using DayMemory.Core.Models.Personal;

namespace DayMemory.Web.Areas.Mobile
{
    public enum AccountErrorCode
    {
        ValidationFailed = 100,

        UserIsNotFound = 101,

        PasswordsDontMatch = 102,

        InvalidRestorePasswordToken = 103,

        UserUpdateFailed = 104,

        UserCreationFailed = 105,

        PasswordChangeFailed = 106,

        InvalidEmailOrPassword = 107,

        UserAlreadyExists = 108,

        EmailIsRequired = 109,

        InvalidAccessOrRefreshToken = 110
    }

    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IJwTokenHelper _jwTokenHelper;
        private readonly IEmailSender _emailSender;
        private readonly IEmailTemplateGenerator _emailTemplateGenerator;
        private readonly IUserRepository _userRepository;

        public AccountController(UserManager<IdentityUser> userManager, ILogger<AccountController> logger,
            IJwTokenHelper jwTokenHelper, IEmailSender emailSender, IEmailTemplateGenerator emailTemplateGenerator, IUserRepository userRepository)
        {

            _logger = logger;
            _jwTokenHelper = jwTokenHelper;
            _emailSender = emailSender;
            _emailTemplateGenerator = emailTemplateGenerator;
            this._userRepository = userRepository;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("api/account/change-password")]
        [Authorize]
        public async Task<ActionResult> ChangePassword(ChangePasswordInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorDto("Input validation failed", (int)AccountErrorCode.ValidationFailed, ModelState));
            }

            if (model.NewPassword != model.ConfirmationPassword)
            {
                return BadRequest(new ErrorDto("Password and Confirm password don't match", (int)AccountErrorCode.PasswordsDontMatch));
            }

            var userId = User.Identity!.Name!;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest(new ErrorDto("Can't find a user with the specified e-mail", (int)AccountErrorCode.UserIsNotFound));
            }

            var passChangeResult = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.ConfirmationPassword);
            if (!passChangeResult.Succeeded)
            {
                return BadRequest(new ErrorDto("Can't change password", (int)AccountErrorCode.PasswordChangeFailed, passChangeResult.Errors));
            }

            return Ok();
        }

        [HttpPost]
        [Route("api/account/enable-encryption")]
        [Authorize]
        public async Task<ActionResult> EnableEncryption(EnableEncryptionInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorDto("Input validation failed", (int)AccountErrorCode.ValidationFailed, ModelState));
            }

            var userId = User.Identity!.Name!;
            var user = await _userManager.FindByIdAsync(userId) as User;
            if (user == null)
            {
                return BadRequest(new ErrorDto("Can't find a user with the specified e-mail", (int)AccountErrorCode.UserIsNotFound));
            }

            user.IsEncryptionEnabled = true;
            user.EncryptedText = model.EncryptedText;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(new ErrorDto("Can't update the user data", (int)AccountErrorCode.InvalidRestorePasswordToken, result.Errors));
            }

            return Ok();
        }

        [HttpPost]
        [Route("api/account/restore-password")]
        public async Task<ActionResult> RestorePassword(RestorePasswordInputModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorDto("Input validation failed", (int)AccountErrorCode.ValidationFailed, ModelState));
            }

            var user = await _userManager.FindByEmailAsync(model.Email) as User;
            if (user == null)
            {
                return BadRequest(new ErrorDto("Can't find a user with the specified e-mail", (int)AccountErrorCode.UserIsNotFound));
            }

            var tokenVerificationRes = await _userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultPhoneProvider, "ResetPasswordPurpose", model.Token);
            if (!tokenVerificationRes)
            {
                return BadRequest(new ErrorDto("Invalid Token", (int)AccountErrorCode.InvalidRestorePasswordToken));
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var res = await _userManager.ResetPasswordAsync(user, token, model.Password);
            if (!res.Succeeded)
            {
                return BadRequest("Can't restore password. Please contact Administrator.");
            }

            var tokenResult = await ConfigureToken(user, ct);
            return Ok(tokenResult);
        }

        [HttpPost]
        [Route("api/account/forgot-password")]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorDto("Input validation failed", (int)AccountErrorCode.ValidationFailed, ModelState));
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return BadRequest(new ErrorDto("Can't find a user with the specified e-mail", (int)AccountErrorCode.UserIsNotFound));
            }

            var token = await _userManager.GenerateUserTokenAsync(user, TokenOptions.DefaultPhoneProvider, "ResetPasswordPurpose");

            //SEND EMAIL
            var content = _emailTemplateGenerator.GenerateMailTemplate("RestorePassword", "en",
                new Dictionary<string, string>() {
                                { "Token", token }
                });

            _emailSender.SendMail(model.Email!, "Password Recovery", content);

            return Ok();
        }


        [HttpPost]
        [Route("api/account/login")]
        public async Task<ActionResult> Login(LoginInputModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorDto("Input validation failed", (int)AccountErrorCode.ValidationFailed, ModelState));
            }

            var user = await _userManager.FindByEmailAsync(model.Email) as User;
            if (user == null)
            {
                return BadRequest(new ErrorDto("Invalid e-mail or password", (int)AccountErrorCode.InvalidEmailOrPassword));
            }

            if (!await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return BadRequest(new ErrorDto("Invalid e-mail or password", (int)AccountErrorCode.InvalidEmailOrPassword));
            }

            var tokenResult = await ConfigureToken(user, ct);
            return Ok(tokenResult);
        }

        private async Task<AccountModel> ConfigureToken(User user, CancellationToken ct)
        {
            var accessToken = await _jwTokenHelper.GenerateAccessToken(user);
            var refreshToken = _jwTokenHelper.GenerateRefreshToken();

            var userToken = new UserToken()
            {
                Id = StringUtils.GenerateUniqueString(),
                CreatedDate = DateTimeOffset.UtcNow,
                ModifiedDate = DateTimeOffset.UtcNow,
                UserId = user.Id,
                RefreshToken = refreshToken
            };

            await _userRepository.CreateTokenAsync(userToken, ct);
            return new AccountModel(user, accessToken, refreshToken);
        }

        [HttpPost]
        [Route("api/account/emailcheck")]
        public async Task<ActionResult<AccountModel>> CheckEmail(CheckEmailInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorDto("Input validation failed", (int)AccountErrorCode.ValidationFailed, ModelState));
            }

            var user = await _userManager.FindByEmailAsync(model.Email) as User;
            if (user != null)
            {
                return BadRequest(new ErrorDto("User with such e-mail already exists", (int)AccountErrorCode.UserAlreadyExists));
            }

            return Ok();
        }

        [HttpPost]
        [Route("api/account/signup")]
        public async Task<ActionResult<AccountModel>> Signup(SignupInputModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorDto("Input validation failed", (int)AccountErrorCode.ValidationFailed, ModelState));
            }

            var user = await _userManager.FindByEmailAsync(model.Email) as User;
            if (user != null)
            {
                return BadRequest(new ErrorDto("User with such e-mail already exists", (int)AccountErrorCode.UserAlreadyExists));
            }

            user = new User { UserName = model.Email, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(new ErrorDto("User creation failed", (int)AccountErrorCode.UserCreationFailed));
            }
            user = await _userManager.FindByEmailAsync(model.Email) as User;
            if (user == null)
            {
                return BadRequest(new ErrorDto("Can't find a user with the specified e-mail", (int)AccountErrorCode.UserIsNotFound));
            }
            //await _mediator.Publish(new UserCreatedNotification() { UserId = user!.Id });

            var tokenResult = await ConfigureToken(user, ct);
            return Ok(tokenResult);
        }

        [HttpPost]
        [Route("api/account/signup/social")]
        public async Task<ActionResult> SocialSignup(SocialSignupInputModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorDto("Input validation failed", (int)AccountErrorCode.ValidationFailed, ModelState));
            }

            var user = await _userManager.FindByLoginAsync(model.ProviderType, model.Id) as User;
            if (user != null)
            {
                var tokenRes = await ConfigureToken(user, ct);
                return Ok(tokenRes);
            }

            if (string.IsNullOrEmpty(model.Email))
            {
                return BadRequest(new ErrorDto("E-mail is required", (int)AccountErrorCode.EmailIsRequired));
            }

            user = await _userManager.FindByEmailAsync(model.Email) as User;
            if (user == null)
            {
                user = new User
                {
                    Id = StringUtils.GenerateUniqueString(),
                    UserName = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email
                };
                var result = await _userManager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    _logger.LogError("Failed to create an account");
                    return BadRequest(new ErrorDto("User creation failed", (int)AccountErrorCode.UserCreationFailed, result.Errors));
                }
            }

            await _userManager.AddLoginAsync(user, new UserLoginInfo(model.ProviderType, model.Id, model.FirstName + model.LastName));
            var tokenResult = await ConfigureToken(user, ct);
            return Ok(tokenResult);
        }

        [HttpPost]
        [Route("api/account/token/refresh")]
        public async Task<IActionResult> RefreshToken(TokenModel tokenModel, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorDto("Input validation failed", (int)AccountErrorCode.ValidationFailed, ModelState));
            }

            var principal = _jwTokenHelper.GetPrincipalFromExpiredToken(tokenModel.AccessToken);
            if (principal == null)
            {
                return BadRequest(new ErrorDto("Invalid access token or refresh token", (int)AccountErrorCode.InvalidAccessOrRefreshToken));
            }

            string userId = principal!.Identity!.Name!;

            var user = await _userManager.FindByIdAsync(userId) as User;
            if (user == null)
            {
                return BadRequest(new ErrorDto("Invalid access token or refresh token", (int)AccountErrorCode.InvalidAccessOrRefreshToken));
            }

            var userToken = await _userRepository.GetTokenAsync(tokenModel.RefreshToken, user.Id, ct);
            if (userToken == null || userToken.RefreshToken != tokenModel.RefreshToken)
            {
                return BadRequest(new ErrorDto("Invalid access token or refresh token", (int)AccountErrorCode.InvalidAccessOrRefreshToken));
            }

            var newAccessToken = await _jwTokenHelper.GenerateAccessToken(user);
            //var newRefreshToken = _jwTokenHelper.GenerateRefreshToken();

            //userToken.RefreshToken = newRefreshToken;
            //userToken.ModifiedDate = DateTimeOffset.UtcNow;
            //await _userRepository.UpdateTokenAsync(userToken, ct);

            return Ok(new
            {
                accessToken = newAccessToken,
                refreshToken = userToken.RefreshToken //newRefreshToken
            });
        }
    }
}