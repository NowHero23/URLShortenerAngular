using URLShortenerAngular.Server.Data.Domain.Repositories.Abstract;
using URLShortenerAngular.Server.Models;

namespace URLShortenerAngular.Server.Data.Domain.Repositories.EntityFramework
{
    public class EFUserRepository : IUserRepository
    {
        private readonly ApiDbContext _context;

        public EFUserRepository(ApiDbContext context)
        {
            _context = context;
        }

        public User Create(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        public User GetByLogin(string login) => _context.Users.FirstOrDefault(u => u.Login == login);
        public User GetById(int id) => _context.Users.FirstOrDefault(u => u.Id == id);

    }
}
