using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoAPI.Models;

[Table(name: "todos")]
public class TodoModel
{
    [Key, Column(name: "id")]
    public Guid Id { get; set; }
    [Column(name: "task", TypeName = "text")]
    public string? Task { get; set; }
    [Column(name: "status")]
    public bool? Status { get; set; }
    [Column(name: "created_at")]
    public DateTime CreatedAt { get; set; }
    [Column(name: "updated_at")]
    public DateTime UpdatedAt { get; set; }
}