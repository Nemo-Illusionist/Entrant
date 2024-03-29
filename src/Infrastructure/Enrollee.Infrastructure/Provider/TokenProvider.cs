using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Enrollee.Application.Services.User;
using Enrollee.Domain.Models;
using Microsoft.IdentityModel.Tokens;
using Enrollee.Application.Setting;
using System;


namespace Enrollee.Infrastructure.Provider;

public class TokenProvider: ITokenProvider
{
    public TokenProvider()
    {
        
    }

    public  Token CreateToken(Account account)
    {
        ArgumentNullException.ThrowIfNull(account);
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenKey = Encoding.UTF8.GetBytes(AuthOptions.KEY);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, account.Login)                    
            }),
            Expires = null,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey),SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return  new Token { Value = tokenHandler.WriteToken(token) };
    }
}