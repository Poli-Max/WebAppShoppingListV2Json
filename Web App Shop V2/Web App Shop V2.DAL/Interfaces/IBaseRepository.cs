using System;

namespace Web_App_Shop_V2.DAL.Interfaces;

public interface IBaseRepository<T>
{
    Task Create(T model);

    Task Delete(T model);

    Task<T> Update(T model);

    IQueryable<T> GetAll();
}

