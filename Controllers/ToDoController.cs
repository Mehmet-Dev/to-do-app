using ToDoApp.Models;
using ToDoApp.Services;

namespace ToDoApp.Controllers
{
    // I will not be adding any comments on Controller classes, because these are mostly self-explanatory. They just call functions from Service classes.
    public class ToDoController
    {
        private readonly ToDoService _service;

        public ToDoController(ToDoService service) => _service = service;

        public void AddToDoItem(int userId, string title, string description, string priority, DateTime dueDate) => _service.AddToDoItem(userId, title, description, priority, dueDate);
        public void CompleteToDoItem(int userId, int index) => _service.CompleteToDoItem(userId, index);
        public void RemoveCompletedAndOverdueItems(int userId) => _service.RemoveCompletedAndOverdueItems(userId);
        public void UpdateToDo(int userId, int index, string title, string description, string priority, DateTime dueDate) => _service.UpdateToDo(userId, index, title, description, priority, dueDate);
        public void DeleteToDo(int userId, int index) => _service.DeleteToDo(userId, index);    

        public List<ToDoItem> GetToDoItems(int userId) => _service.GetToDoItems(userId);
    }
}