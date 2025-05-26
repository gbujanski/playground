using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Linq;
using AuthApi.Db;
using AuthApi.Services;
using AuthApi.Models;

namespace AuthApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthDbContext _context;
    private readonly TokenService _tokenService;

    public AuthController(AuthDbContext context, TokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(string email, string password)
    {

        if (await _context.Users.AnyAsync(u => u.Email == email))
            return BadRequest("User already exists.");

        User user = new User
        {
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(string email, string password)
    {
        Console.WriteLine($"Login attempt for email: {email}");
        Console.WriteLine($"Password provided: {password}");
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            return BadRequest("Email and password are required.");
            
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            return Unauthorized();

        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();
        user.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            accessToken,
            refreshToken = refreshToken.Token
        });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(string refreshToken)
    {
        var user = await _context.Users
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.RefreshTokens.Any(rt => rt.Token == refreshToken && !rt.Revoked));

        if (user == null)
            return Unauthorized();

        var oldToken = user.RefreshTokens.First(rt => rt.Token == refreshToken);
        if (oldToken.ExpiresAt < DateTime.UtcNow)
            return Unauthorized("Refresh token expired.");

        oldToken.Revoked = true;
        var newRefresh = _tokenService.GenerateRefreshToken();
        user.RefreshTokens.Add(newRefresh);

        await _context.SaveChangesAsync();

        var newAccess = _tokenService.GenerateAccessToken(user);
        return Ok(new
        {
            accessToken = newAccess,
            refreshToken = newRefresh.Token
        });
    }
}
