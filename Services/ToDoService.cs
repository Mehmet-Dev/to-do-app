using ToDoApp.Models;


// To do service. Handles all of the changes in the database whether it comes to adding a new to do or updating one. Plainly simple.
namespace ToDoApp.Services
{
    public class ToDoService
    {
        private readonly AppDbContext _dbContext;

        public ToDoService(AppDbContext context) => _dbContext = context;

        /// <summary>
        /// Adds a new to-do.
        /// </summary>
        /// <param name="userId">User ID of the logged in person.</param>
        /// <param name="title">Title of the to-do.</param>
        /// <param name="description">Description of the to-do.</param>
        /// <param name="priority">Priority level of the to-do (can be Low, Medium or high).</param>
        /// <param name="dueDate">The due date of the to-do.</param>
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
        public void RemoveCompletedAndOverdueItems(int userId)
        {
            var user = _dbContext.Users.Find(userId);
            for(int i = 0; i < user.ToDoItems.Count; i++)
            {
                if(user.ToDoItems[i].IsCompleted || user.ToDoItems[i].DueDate < DateTime.Now) user?.ToDoItems.RemoveAt(i);
            }
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Updates a to do.
        /// </summary>
        /// <param name="userId">User ID of the logged in person.</param>
        /// <param name="title">Title of the to-do.</param>
        /// <param name="desc">Description of the to-do.</param>
        /// <param name="prio">Priority level of the to-do (can be Low, Medium or high).</param>
        /// <param name="dueDate">The due date of the to-do.</param>
        public void UpdateToDo(int userId, int index, string? title, string? desc, string prio, DateTime dueDate)
        {
            var user = _dbContext.Users.Find(userId);
            
            var todo = user.ToDoItems[index - 1];

            todo.Title = title;
            todo.Description = desc;
            todo.Priority = prio;
            todo.DueDate = dueDate;

            user.ToDoItems[index - 1] = todo;
            _dbContext.SaveChanges();
        }

        public void DeleteToDo(int userId, int index)
        {
            var user = _dbContext.Users.Find(userId);

            user.ToDoItems.RemoveAt(index - 1);
        }
    }
}