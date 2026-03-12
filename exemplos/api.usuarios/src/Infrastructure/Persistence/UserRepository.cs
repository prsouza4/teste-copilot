using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Usuario.Domain.Entities;
using Usuario.Domain.Interfaces;

namespace Usuario.Infrastructure.Persistence
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _context;

        public UserRepository(UserDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
    }
}
