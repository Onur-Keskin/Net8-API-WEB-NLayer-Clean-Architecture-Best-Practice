﻿using App.Application.Contracts.Persistance;

namespace App.Persistance;
public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public Task<int> SaveChangeAsync() => context.SaveChangesAsync();
}

