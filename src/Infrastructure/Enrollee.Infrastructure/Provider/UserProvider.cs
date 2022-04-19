﻿using System.Threading;
using System.Threading.Tasks;
using Enrollee.Application.Services.User;
using Enrollee.Domain.Models;
using Enrollee.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Enrollee.Infrastructure.Provider;

internal class UserProvider : IUserProvider
{
    private readonly ServerDbContext _dbContext;

    public UserProvider(ServerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> FindAsync(string login, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<User>()
            .AsNoTracking()
            .SingleOrDefaultAsync(user => user.Login == login, cancellationToken)
            .ConfigureAwait(false);
    }
    public async Task<User?> FindAsyncUser(string login, string password, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<User>()
            .AsNoTracking()
            .SingleOrDefaultAsync(user =>(user.Login == login)&&(user.Password == password), cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken)
    {
        _dbContext.Add(user);
        await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}
