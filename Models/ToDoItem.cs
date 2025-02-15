namespace ToDoApp.Models
{
    // The to-do class. Most of this is self explanatory.
    public class ToDoItem
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public string? Priority { get; set; }
        public DateTime DueDate { get; set; }

        public override string ToString() => $"To do item '{Title}'\nwith description: '{Description}'\nwith a priority of: '{Priority}'\ndue at {DueDate:ddd dd MM yy hh.mm}";
    }
}