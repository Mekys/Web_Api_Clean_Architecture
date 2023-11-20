using Application;
using Application.IRepositories;
using Domain.Enums;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public AuthRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<AuthState> AuthorizeUser(string login, string password)
        {
            var user = await _dbContext.Users.Include(x => x.Group).FirstOrDefaultAsync(x => x.Login.Equals(login) && x.Password.Equals(password));

            if (user == null)
            {
                return AuthState.None;
            }

            if (user.Group.Code == Position.User)
            {
                return AuthState.User;
            }

            return AuthState.Admin;
        }
    }
}
