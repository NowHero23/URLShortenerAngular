using Microsoft.EntityFrameworkCore;
using URLShortenerAngular.Server.Data.Domain.Repositories.Abstract;
using URLShortenerAngular.Server.Models;

namespace URLShortenerAngular.Server.Data.Domain.Repositories.EntityFramework
{
    public class EFUrlItemRepository : IUrlItemRepository
    {
        private readonly ApiDbContext _context;

        public EFUrlItemRepository(ApiDbContext context)
        {
            _context = context;
        }

        UrlItem IUrlItemRepository.Create(UrlItem url)
        {
            _context.UrlItems.Add(url);
            _context.SaveChanges();

            return url;
        }

        bool IUrlItemRepository.Delete(UrlItem url)
        {
            var a = _context.UrlItems.Remove(url);
            _context.SaveChanges();
            Console.WriteLine(a);
            return true;
        }

        DbSet<UrlItem> IUrlItemRepository.GetAll() => _context.UrlItems;

        UrlItem? IUrlItemRepository.GetDetailsById(int urlId) => _context.UrlItems.Include(u => u.Author).FirstOrDefault(u => u.Id == urlId);
        UrlItem? IUrlItemRepository.GetDetailsByShortUrl(string shortUrl) => _context.UrlItems.Include(u => u.Author).FirstOrDefault(u => u.ShortUrl == shortUrl);
    }
}
