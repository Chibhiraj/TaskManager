
using System;
using TaskManagerApi.Entities;

namespace TaskManagerApi.DTOs
{
    public record CreateTaskDto(
        string Title,
        string Description,
        TaskStatus Status,
        TaskPriority Priority,
        DateTime? DueDate,
        int? AssignedToId
    );

    public record UpdateTaskDto(
        string Title,
        string Description,
        TaskStatus Status,
        TaskPriority Priority,
        DateTime? DueDate,
        int? AssignedToId
    );

    public record UpdateTaskStatusDto(TaskStatus Status);
}
