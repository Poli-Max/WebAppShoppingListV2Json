using System;
using Web_App_Shop_V2.DAL.Interfaces;
using Web_App_Shop_V2.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Web_App_Shop_V2.DAL.Repositoties;

public class ProductRepository : IBaseRepository<Product>
{
    private readonly AplicationDbContext _db;

    public ProductRepository (AplicationDbContext db)
    {
        this._db = db;
    }

    public async Task Create (Product model)
    {
        await _db.Product.AddAsync(model);
        await _db.SaveChangesAsync();
    }

    public async Task Delete(Product model)
    {
        _db.Product.Remove(model);
        await _db.SaveChangesAsync();
    }

    public IQueryable<Product> GetAll()
    {
        return _db.Product;
    }

    public async Task<Product> Update(Product model)
    {
        _db.Product.Update(model);
        await _db.SaveChangesAsync();

        return model;
    }
}

