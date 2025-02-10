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
                    Priority = priority,
                    DueDate = dueDate,
                    IsCompleted = false,
                });
                _dbContext.SaveChanges();
            }
        }
        
        /// <summary>
        /// Sets the specified to do item in the list by its index
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="index">Index of the to-do.</param>
        public void CompleteToDoItem(int userId, int index)
        {
            var user = _dbContext.Users.Find(userId);
            if(user != null)
            {
                user.ToDoItems[index - 1].IsCompleted = true;
                _dbContext.SaveChanges();
            }
        }
        
        /// <summary>
        /// Returns all of the to do items of specified user.
        /// </summary>
        /// <param name="userId">Index of the user.</param>
        /// <returns></returns>
        public List<ToDoItem> GetToDoItems(int userId)
        {
            var user = _dbContext.Users.Find(userId);
            return user?.ToDoItems ?? new List<ToDoItem>();
        }

        /// <summary>
        /// Removes all completed to do items of a user.
        /// </summary>
        /// <param name="userId">Index of the user... how many more times do I have to write this?</param>
        public void RemoveCompletedItems(int userId)
        {
            var user = _dbContext.Users.Find(userId);
            for(int i = 0; i < user.ToDoItems.Count; i++)
            {
                if(user.ToDoItems[i].IsCompleted) user?.ToDoItems.RemoveAt(i);
            }
            _dbContext.SaveChanges();
        }
    }
}