using TaskManagement.Api.Enums;

namespace TaskManagement.Api.DTOs;

public class UpdateTaskDto
{
    // Nullable fields to support partial updates via PATCH
    public string? Title { get; set; }

    public string? Description { get; set; }

    public TaskItemStatus? Status { get; set; }

    public TaskPriority? Priority { get; set; }

    public DateTime? DueDate { get; set; }
}