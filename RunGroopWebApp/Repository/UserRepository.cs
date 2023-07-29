using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Data;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;

namespace RunGroopWebApp.Repository;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _dbContext;

    public UserRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public bool Add(AppUser user)
    {
        throw new NotImplementedException();
    }
    public bool Delete(AppUser user)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<AppUser>> GetAllUsers()
    {
        return await _dbContext.Users.ToListAsync();
    }
    public async Task<AppUser> GetUserById(string id)
    {
        return await _dbContext.Users.FindAsync(id);
    }
    public bool Save()
    {
        var saved = _dbContext.SaveChanges();
        return saved > 0 ? true : false;
    }
    public bool Update(AppUser user)
    {
        _dbContext.Update(user);
        return Save();
    }
}