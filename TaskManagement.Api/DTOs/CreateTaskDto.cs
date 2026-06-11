using TaskManagement.Api.Enums;

namespace TaskManagement.Api.DTOs;

public class CreateTaskDto
{
    public string Title { get; set; } = "";

    public string Description { get; set; } = "";

    public TaskItemStatus Status { get; set; } = TaskItemStatus.Pending;

    public TaskPriority Priority { get; set; } = TaskPriority.Medium;

    public DateTime? DueDate { get; set; }
}