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

        public override string ToString() => $"To do item '{Title}' with description: '{Description}' with a priority of: '{Priority}' due at {DueDate:ddd dd MM yy hh.mm}";
    }
}