using URLShortenerAngular.Server.Models;

namespace URLShortenerAngular.Server.Data.Domain.Repositories.Abstract
{
    public interface IUserRepository
    {
        User Create(User user);
        User GetByLogin(string login);
        User GetById(int id);
    }
}
