using ToDoApp.Models;

namespace ToDoApp.Services
{
    public class ToDoService
    {
        private readonly AppDbContext _dbContext;

        public ToDoService(AppDbContext context) => _dbContext = context;

        public void AddToDoItem(int userId, string title, string description, string priority, DateTime dueDate)
        {
            var user = _dbContext.Users.Find(userId);
            if(user != null)
            {
                user.ToDoItems?.Add(new ToDoItem
                {
                    Title = title,
                    Description = description,
                    IsCompleted = false,
                });
                _dbContext.SaveChanges();
            }
        }

        public void CompleteToDoItem(int userId, int index)
        {
            var user = _dbContext.Users.Find(userId);
            if(user != null)
            {
                user.ToDoItems[index - 1].IsCompleted = true;
                _dbContext.SaveChanges();
            }
        }

        public List<ToDoItem> GetToDoItems(int userId)
        {
            var user = _dbContext.Users.Find(userId);
            return user?.ToDoItems ?? new List<ToDoItem>();
        }
    }
}