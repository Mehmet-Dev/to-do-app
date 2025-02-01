using ToDoApp.Models;
using ToDoApp.Services;

namespace ToDoApp.Controllers
{
    public class ToDoController
    {
        private readonly ToDoService _service;

        public ToDoController(ToDoService service) => _service = service;

        public void AddToDoItem(int userId, string title, string description, string priority, DateTime dueDate) => _service.AddToDoItem(userId, title, description, priority, dueDate);
        public void CompleteToDoItem(int userId, int index) => _service.CompleteToDoItem(userId, index);

        public List<ToDoItem> GetToDoItems(int userId) => _service.GetToDoItems(userId);
    }
}