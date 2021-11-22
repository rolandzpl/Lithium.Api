using Lithium.Api.Accounts;
using Microsoft.AspNetCore.Mvc;
using NewAccountDto = Lithium.Api.Accounts.AspNetCore.Controllers.Dto.NewAccountDto;
using AccountDto = Lithium.Api.Accounts.AspNetCore.Controllers.Dto.AccountDto;
using Mapster;
using LinqSpecs;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;

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
        var user = accountRepository
            .GetAccounts(new AdHocSpecification<Account>(_ => _.Login == username))
            .SingleOrDefault();
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Login),
            // new Claim("FullName", user.FullName),
            // new Claim(ClaimTypes.Role, "Editor"),
        };
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties
        {
            //AllowRefresh = <bool>,
            // Refreshing the authentication session should be allowed.

            //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
            // The time at which the authentication ticket expires. A 
            // value set here overrides the ExpireTimeSpan option of 
            // CookieAuthenticationOptions set with AddCookie.

            //IsPersistent = true,
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
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
             new ClaimsPrincipal(claimsIdentity), authProperties);
        return Ok();
    }

    [HttpPost("/account/logout")]
    public async Task Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    [HttpGet("/account/{username}")]
    public async Task<AccountDto> GetAccount(string username)
    {
        return accountRepository
            .GetAccounts(new AdHocSpecification<Account>(_ => _.Login == username))
            .Adapt<AccountDto>();
    }

    [HttpGet("/account")]
    public async Task<IEnumerable<AccountDto>> GetAllAccounts()
    {
        return accountRepository
            .GetAccounts(new AdHocSpecification<Account>(_ => true))
            .Select(_ => _.Adapt<AccountDto>());
    }

    [Authorize("AdminAccess")]
    [HttpPost("/account")]
    public async Task AddAccount(NewAccountDto account)
    {
        await accountService.AddAccountAsync(account.Adapt<Lithium.Api.Accounts.NewAccountDto>());
    }
}
