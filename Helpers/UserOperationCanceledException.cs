namespace ToDoApp.Helpers;

// A small class that throws an exception.
public class UserOperationCanceledException : Exception
{
    public UserOperationCanceledException(string message) : base(message) { }
}
