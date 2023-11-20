using Application.IRepositories;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMemoryCache cache;

        public UserRepository(ApplicationDbContext dbContext, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            cache = memoryCache;
        }

        public async Task<IActionResult> AddAsync(User entity)
        {
            cache.TryGetValue(entity.Login, out User? userFromCache);

            if (userFromCache != null)
            {
                return new BadRequestResult();
            }

            var userFromDB = _dbContext.Users.FirstOrDefault(x => x.Login.Equals(entity.Login));

            if (userFromDB != null)
            {
                return new BadRequestResult();
            }

            if (entity.Group.Code == Position.Admin)
            {
                return new BadRequestResult();
            }


            cache.Set(entity.Login, entity, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(5)));

            await Task.Delay(5000);

            var state = entity.State;
            var group = entity.Group;

            state.UserStateId = 0;
            state.Code = State.Active;
            group.UserGroupId = 0;
            entity.UserID = 0;

            await _dbContext.Users.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return new OkResult();
        }
        public async Task<IActionResult> DeleteAsync(long id)
        {
            var user = await _dbContext.Users.Include(p => p.State).Include(p => p.Group).FirstOrDefaultAsync(p => p.UserID == id);
            if (user == null)
            {
                return new NotFoundResult();
            }

            if (user.Group.Code == Position.Admin)
            {
                return new BadRequestResult();
            }

            user.State.Code = State.Blocked;
            await _dbContext.SaveChangesAsync();
            return new OkResult();
        }

        public async Task<IReadOnlyList<User>> GetAllAsync()
        {
            return await _dbContext.Users.Include(x => x.Group).Include(x => x.State).ToListAsync();
        }

        public async Task<User> GetByIdAsync(long id)
        {
            return await _dbContext.Users.Include(x => x.Group).Include(x => x.State).FirstOrDefaultAsync(p => p.UserID == id);
        }
    }
}
