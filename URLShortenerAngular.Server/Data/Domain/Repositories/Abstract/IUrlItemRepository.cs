using Microsoft.EntityFrameworkCore;
using URLShortenerAngular.Server.Models;

namespace URLShortenerAngular.Server.Data.Domain.Repositories.Abstract
{
    public interface IUrlItemRepository
    {
        UrlItem Create(UrlItem url);
        bool Delete(UrlItem url);
        UrlItem? GetDetailsById(int urlId);
        UrlItem? GetDetailsByShortUrl(string shortUrl);
        DbSet<UrlItem> GetAll();
    }
}
