﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

    public async Task<AppUser> GetUserById(string id)
    {
        return await _dbContext.Users.FindAsync(id);
    }

    public async Task<AppUser> GetByIdNoTracking(string id)
    {
        return await _dbContext.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
    }

    public bool Update(AppUser user)
    {
        _dbContext.Users.Update(user);
        return Save();
    }

    public bool Save()
    {
        var saved = _dbContext.SaveChanges();
        return saved > 0 ? true : false;
    }
}