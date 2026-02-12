using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using wx.Application.Common.Exceptions;
using wx.Application.Common.Interfaces;
using wx.Core.Domain.Security;
using wx.Core.IRepositories;
using wx.Infrastructure.Options;
using wx.Infrastructure.Services;

namespace wx.Infrastructure.Security;

[AutoRegister]
public class IdentityService : IIdentityService
{
    private readonly IConfiguration _configuration;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserRepository _userRepo;
    private readonly TokenOptions _tokenOptions;
    private readonly IRefreshTokenService _refreshTokenService;

    private const string LOGIN_ERROR = "Account or password is incorrect.";


    public IdentityService(IPasswordHasher passwordHasher, IUserRepository userRepo, IConfiguration configuration, IOptions<TokenOptions> tokenOptions, IRefreshTokenService refreshTokenService)
    {
        _passwordHasher = passwordHasher;
        _userRepo = userRepo;
        _configuration = configuration;
        _tokenOptions = tokenOptions.Value;
        _refreshTokenService = refreshTokenService;
    }

    public async Task<TokenDto> GetTokenAsync(string account, string password, string remoteIp, string? deviceId)
    {
        var user = await _userRepo.GetUserByAccountAsync(account);

        if (user == null) throw new BadRequestException(LOGIN_ERROR);

        var verify = _passwordHasher.Verify(password, user.Password);

        if (!verify) throw new BadRequestException(LOGIN_ERROR);

        var accessToken = GenerateToken(user, out var expiresAt);
        var refreshToken = await _refreshTokenService.GenerateTokenAsync(user.Id, deviceId, "", remoteIp);

        TokenDto token = new TokenDto(IdentityConstant.Bearer, accessToken, refreshToken, expiresAt, deviceId);
        return token;
    }

    public Task<TokenDto> RefreshTokenAsync(string refreshToken)
    {
        throw new NotImplementedException();
    }

    private string GenerateToken(User user, out DateTime expiresAt)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Account),
        };

        //foreach (var role in user.Roles)
        //{
        //    claims.Add(new Claim(ClaimTypes.Role, role.Name));
        //}

        expiresAt = DateTime.UtcNow.AddMinutes(_tokenOptions.ExpireMinutes);
        //var permissions = user.Roles.SelectMany(p => p.Permissions.Select(p => p.Code));

        //foreach (var permission in permissions)
        //{
        //    claims.Add(new Claim("permission", permission));
        //}
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOptions.SecretKey));
        var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _tokenOptions.Issuer,
            audience: _tokenOptions.Audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}