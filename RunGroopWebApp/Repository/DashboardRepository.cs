using RunGroopWebApp.Data;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;

namespace RunGroopWebApp.Repository;

public class DashboardRepository : IDashboardRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DashboardRepository(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<List<Club>> GetAllUserClubs()
    {
        var currentUser = _httpContextAccessor.HttpContext?.User.GetUserId();
        var userClubs = _dbContext.Clubs.Where(c => c.AppUser.Id == currentUser);
        return userClubs.ToList();
    }
    public async Task<List<Race>> GetAllUserRaces()
    {
        var currentUser = _httpContextAccessor.HttpContext?.User.GetUserId();
        var userRace = _dbContext.Races.Where(r => r.AppUser.Id == currentUser);
        return userRace.ToList();
    }
}