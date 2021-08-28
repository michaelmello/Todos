using System.ComponentModel.DataAnnotations;

namespace TodoWithControllers.Models;

public class TodoRequest
{
    public int Id { get; set; }
    [Required]
    public string? Title { get; set; }
    public bool IsComplete { get; set; }
}