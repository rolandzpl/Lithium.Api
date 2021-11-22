using Microsoft.AspNetCore.Mvc;
using Mapster;
using LinqSpecs;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Lithium.Api.AspNetCore;
using Microsoft.AspNetCore.Http;
using Lithium.Api.Accounts.AspNetCore.Controllers.Dto;

namespace Lithium.Api.Accounts.AspNetCore.Controllers;

[Authorize("AdminAccess")]
[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountService accountService;
    private readonly IAccountRepository accountRepository;

    public AccountController(IAccountService accountService, IAccountRepository accountRepository)
    {
        this.accountService = accountService;
        this.accountRepository = accountRepository;
    }

    [AllowAnonymous]
    [HttpPost("/account/login")]
    public async Task<IActionResult> Login(string username, string password)
    {
        if (!await accountService.ValidatePasswordAsync(username, password))
        {
            return Unauthorized();
        }
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(
                new ClaimsIdentity(CollectClaims(await GetUserLogin(username)),
                CookieAuthenticationDefaults.AuthenticationScheme)),
            GetAuthProperties());
        return Ok();
    }

    private async Task<string> GetUserLogin(string username) =>
        (await accountRepository.GetAccountByLoginAsync<UserOnlyLoginDto>(username))?.Login;

    class UserOnlyLoginDto
    {
        public string Login { get; init; }
    }

    private static List<Claim> CollectClaims(string login)
    {
        return new List<Claim>
        {
            new Claim(ClaimTypes.Name, login),
            // new Claim("FullName", user.FullName),
            // new Claim(ClaimTypes.Role, "Editor"),
        };
    }

    private static AuthenticationProperties GetAuthProperties() =>
        new AuthenticationProperties
        {
            AllowRefresh = true,
            // Refreshing the authentication session should be allowed.

            //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
            // The time at which the authentication ticket expires. A 
            // value set here overrides the ExpireTimeSpan option of 
            // CookieAuthenticationOptions set with AddCookie.

            IsPersistent = true,
            // Whether the authentication session is persisted across 
            // multiple requests. When used with cookies, controls
            // whether the cookie's lifetime is absolute (matching the
            // lifetime of the authentication ticket) or session-based.

            //IssuedUtc = <DateTimeOffset>,
            // The time at which the authentication ticket was issued.

            //RedirectUri = <string>
            // The full path or absolute URI to be used as an http 
            // redirect response value.
        };

    [HttpPost("/account/logout")]
    public async Task Logout() =>
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

    [HttpGet("/account/{username}")]
    public async Task<AccountDto> GetAccount(string username) =>
        await accountRepository.GetAccountByLoginAsync<AccountDto>(username)
        ?? throw new HttpResponseException(StatusCodes.Status404NotFound, username);

    [HttpGet("/account")]
    public async Task<IEnumerable<AccountDto>> GetAllAccounts() =>
        await accountRepository.GetAccountsAsync<AccountDto>(new AdHocSpecification<Account>(_ => true));

    [HttpPost("/account")]
    public async Task AddAccount(NewAccountDto account) =>
        await accountService.AddAccountAsync(account.Adapt<Lithium.Api.Accounts.NewAccountDto>());
}
