using System.Collections.Generic;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// The user class used by AppDbContext.
// The to-do's are saved in JSON format in the database.
namespace ToDoApp.Models
{
    public class User
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(26)]
        public string? Username { get; set; }

        [Required]
        public string? Password { get; set; }

        [NotMapped]
        public List<ToDoItem>? ToDoItems { get; set; } = new();

        public string ToDoItemsJson
        {
            get => JsonSerializer.Serialize(ToDoItems);
            set => ToDoItems = string.IsNullOrEmpty(value)
                                ? new List<ToDoItem>()
                                : JsonSerializer.Deserialize<List<ToDoItem>>(value);
        }
    }
}