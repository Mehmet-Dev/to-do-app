using ToDoApp.Models;
using ToDoApp.Services;

namespace ToDoApp.Controllers
{
    public class UserController
    {
        private readonly AuthService? _authService;
        public UserController(AuthService service) => _authService = service;

        public bool Register(string username, string password) => _authService.Register(username, password);

        public User Login(string username, string password) => _authService.Login(username, password);
    }
}