using System;
using Web_App_Shop_V2.DAL.Interfaces;
using Web_App_Shop_V2.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Web_App_Shop_V2.DAL.Repositoties;

public class UserRepository : IBaseRepository<User>
{
    private readonly AplicationDbContext _db;

    public UserRepository(AplicationDbContext db)
    {
        this._db = db;
    }

    public async Task Create(User model)
    {
        await _db.user.AddAsync(model);
        await _db.SaveChangesAsync();
    }

    public async Task Delete(User model)
    {
        _db.user.Remove(model);
        await _db.SaveChangesAsync();
    }

    public IQueryable<User> GetAll()
    {
        return _db.user;
    }

    public async Task<User> Update(User model)
    {
        _db.user.Update(model);
        await _db.SaveChangesAsync();

        return model;
    }
}

