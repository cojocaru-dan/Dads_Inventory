using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using DadsInventory.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DadsInventory.Authentication;
public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly FamilyRepository _familyRepository;
    public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            FamilyRepository familyRepository)
            : base(options, logger, encoder, clock)
    {
        _familyRepository = familyRepository;
    }
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization"))
            return AuthenticateResult.Fail("Missing Authorization Header");

        var authenticationHeaderValue = AuthenticationHeaderValue.Parse(Request.Headers.Authorization);

        string userInfoDecoded = System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(authenticationHeaderValue.Parameter));

        string memberName = userInfoDecoded.Split(":")[0];
        string memberPassword = userInfoDecoded.Split(":")[1];
        var familyMember = _familyRepository.GetAllMembers().Where(member => member.Name == memberName && member.Password == memberPassword).FirstOrDefault();

        if (familyMember == null) return AuthenticateResult.Fail("Invalid Name or Password!");

        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, familyMember.Name),
        };
        // add user roles as claims here
        foreach (var role in familyMember.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }
    public void AddClaimsTo(FamilyMember familyMember)
    {
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, familyMember.Name),
        };
        // add user roles as claims here
        foreach (var role in familyMember.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        AuthenticateResult.Success(ticket);
    }
}