namespace ToDoApp.Helpers;

public class UserOperationCanceledException : Exception
{
    public UserOperationCanceledException(string message) : base(message) { }
}
