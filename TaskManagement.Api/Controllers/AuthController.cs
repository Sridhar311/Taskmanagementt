using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Api.Data;
using TaskManagement.Api.DTOs;
using TaskManagement.Api.Entities;
using TaskManagement.Api.Services;

namespace TaskManagement.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly JwtService _jwtService;

    public AuthController(
        AppDbContext context,
        JwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(
        RegisterDto dto)
    {
        var existingUser =
            await _context.Users
            .FirstOrDefaultAsync(
                x => x.Email == dto.Email);

        if (existingUser != null)
        {
            return BadRequest(new
            {
                message = "Email already exists"
            });
        }

        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,

            PasswordHash =
                BCrypt.Net.BCrypt.HashPassword(
                    dto.Password)
        };

        _context.Users.Add(user);

        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "User registered successfully"
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        LoginDto dto)
    {
        var user =
            await _context.Users
            .FirstOrDefaultAsync(
                x => x.Email == dto.Email);

        if (user == null)
        {
            return Unauthorized(new
            {
                message = "Invalid credentials"
            });
        }

        bool valid =
            BCrypt.Net.BCrypt.Verify(
                dto.Password,
                user.PasswordHash);

        if (!valid)
        {
            return Unauthorized(new
            {
                message = "Invalid credentials"
            });
        }

        var token =
            _jwtService.GenerateToken(user);

        return Ok(new
        {
            token
        });
    }
}