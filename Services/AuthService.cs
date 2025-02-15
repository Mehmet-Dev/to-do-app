using ToDoApp.Models;
using System.Linq;

// The authentication service, only used to register and log a user in.
namespace ToDoApp.Services
{
    public class AuthService
    {
        private readonly AppDbContext _dbContext;

        public AuthService(AppDbContext context) => _dbContext = context;

        public bool Register(string username, string password)
        {
            if(_dbContext.Users.Any(u => u.Username == username)) return false;

            var user = new User { Username = username, Password = password };
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            return true;
        }

        public User? Login(string username, string password) => _dbContext.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
    }
}