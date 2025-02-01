namespace ToDoApp.Models
{
    public class ToDoItem
    {
        public int ID { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public string? Priority { get; set; }
        public DateTime DueDate { get; set; }
    }
}