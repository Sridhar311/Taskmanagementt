using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TaskManagement.Api.Data;
using TaskManagement.Api.DTOs;
using TaskManagement.Api.Entities;
using TaskManagement.Api.Enums;

namespace TaskManagement.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/tasks")]
public class TasksController : ControllerBase
{
    private readonly AppDbContext _context;

    public TasksController(AppDbContext context)
    {
        _context = context;
    }

    private Guid GetCurrentUserId()
    {
        return Guid.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier)!
            .Value);
    }

    // CREATE TASK
    [HttpPost]
    public async Task<IActionResult> Create(
        CreateTaskDto dto)
    {
        var task = new TaskItem
        {
            Title = dto.Title,
            Description = dto.Description,
            Status = dto.Status,
            Priority = dto.Priority,
            DueDate = ConvertToUtc(dto.DueDate),
            UserId = GetCurrentUserId()
        };

        _context.Tasks.Add(task);

        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetById),
            new { id = task.Id },
            task);
    }

    // GET TASK BY ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var task =
            await _context.Tasks
            .FirstOrDefaultAsync(x =>
                x.Id == id &&
                x.UserId == GetCurrentUserId());

        if (task == null)
            return NotFound(new
            {
                success = false,
                message = "Task not found"
            });

        return Ok(new
        {
            success = true,
            data = task
        });
    }

    // GET ALL TASKS
    [HttpGet]
    public async Task<IActionResult> GetTasks(
        TaskItemStatus? status,
        string? search,
        string? sortBy,
        int page = 1,
        int pageSize = 10)
    {
        var query =
            _context.Tasks
            .Where(x =>
                x.UserId ==
                GetCurrentUserId())
            .AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(x =>
                x.Status == status.Value);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x =>
                x.Title.Contains(search));
        }

        query = sortBy switch
        {
            "dueDate" =>
                query.OrderBy(x => x.DueDate),

            "priority" =>
                query.OrderBy(x => x.Priority),

            _ =>
                query.OrderByDescending(
                    x => x.CreatedAt)
        };

        var totalCount =
            await query.CountAsync();

        var items =
            await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return Ok(new
        {
            page,
            pageSize,
            totalCount,
            totalPages =
                (int)Math.Ceiling(
                    totalCount /
                    (double)pageSize),

            items
        });
    }

    // UPDATE TASK
    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateTaskDto dto)
    {
        var task =
            await _context.Tasks
            .FirstOrDefaultAsync(x =>
                x.Id == id &&
                x.UserId == GetCurrentUserId());

        if (task == null)
            return NotFound(new
            {
                success = false,
                message = "Task not found"
            });

        if (dto.Title != null)
            task.Title = dto.Title;

        if (dto.Description != null)
            task.Description = dto.Description;

        if (dto.Status.HasValue)
            task.Status = dto.Status.Value;

        if (dto.Priority.HasValue)
            task.Priority = dto.Priority.Value;

        if (dto.DueDate.HasValue)
            task.DueDate = ConvertToUtc(dto.DueDate);

        task.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(new
        {
            success = true,
            data = task
        });
    }

    private static DateTime? ConvertToUtc(DateTime? date)
    {
        if (!date.HasValue)
            return null;

        return date.Value.Kind switch
        {
            DateTimeKind.Utc => date.Value,
            DateTimeKind.Local => date.Value.ToUniversalTime(),
            _ => DateTime.SpecifyKind(date.Value, DateTimeKind.Utc)
        };
    }

    // DELETE TASK
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(
        Guid id)
    {
        var task =
            await _context.Tasks
            .FirstOrDefaultAsync(x =>
                x.Id == id &&
                x.UserId == GetCurrentUserId());

        if (task == null)
            return NotFound(new
            {
                success = false,
                message = "Task not found"
            });

        _context.Tasks.Remove(task);

        await _context.SaveChangesAsync();

        return Ok(new
        {
            success = true,
            message = "Task deleted successfully"
        });
    }
}