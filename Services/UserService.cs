// using Microsoft.EntityFrameworkCore;
// using MinimalApiTodoApi.Models;
// using MinimalApiTodoApi.Database;
// using System.Collections;

// namespace MinimalApiTodoApi.Services;

// public class UserService : IUserService
// {
//     private readonly AppDbContext _db;

//     public UserService(AppDbContext db)
//     {
//         _db = db;
//     }

//     public async Task<IEnumerable<User>> GetAllUsers()
//     {
//         return await _db.Users.ToListAsync();
//     }

//     public async Task<User?> GetUserById(int id)
//     {
//         return await _db.Users.FindAsync(id);
//     }

//     public async Task<User> CreateUser(User user)
//     {
//         var result = await _db.Users.AddAsync(user);
//         await _db.SaveChangesAsync();
//         return result.Entity;
//     }

//     public async Task<User> Authenticate(string username, string password)
//     {
//         return await _db.Users.FirstOrDefaultAsync(u => u.Username == username && u.PasswordHash == password);
//     }
// }