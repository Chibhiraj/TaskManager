using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagerApi.Entities;

public enum TaskStatus
{
    TODO,
    IN_PROGRESS,
    DONE
}
public enum TaskPriority
{
    LOW,
    MEDIUM,
    HIGH
}

public class TaskItem
{
    public int Id { get; set; }

    [Required]
    [MinLength(3)]
    [MaxLength(100)]
    public string Title { get; set; }

    [MaxLength(500)]
    public string Description { get; set; }

    [Required]
    public TaskStatus Status { get; set; } = TaskStatus.TODO;

    [Required]
    public TaskPriority Priority { get; set; } = TaskPriority.MEDIUM;

    public DateTime? DueDate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Foreign key to User
    public int? AssignedToId { get; set; }
    [ForeignKey("AssignedToId")]
    public User AssignedTo { get; set; }
}
