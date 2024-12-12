using Microsoft.EntityFrameworkCore;
using URLShortenerAngular.Server.Data;
using URLShortenerAngular.Server.Data.Domain.Repositories.Abstract;
using URLShortenerAngular.Server.Data.Domain.Repositories.EntityFramework;
using URLShortenerAngular.Server.Data.Enums;
using URLShortenerAngular.Server.Models;

namespace URLShortenerAngular.UnitTests
{
    [TestFixture]
    public class EFUrlItemRepositoryTests
    {
        private IUrlItemRepository _urlRepository;
        private IUserRepository _userRepository;

        private ApiDbContext _context;
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var options = new DbContextOptionsBuilder<ApiDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .EnableSensitiveDataLogging()
            .Options;
            _context = new ApiDbContext(options);
            _urlRepository = new EFUrlItemRepository(_context);
            _userRepository = new EFUserRepository(_context);
        }
        [OneTimeTearDown]
        public void Cleanup()
        {
            _context.Dispose();
        }

        [SetUp]
        public void Setup()
        {
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            _context.ChangeTracker.Clear();

            User user = new User()
            {
                Id = 1,
                Login = "urlTest",
                Password = "urlTest",
                AccessLevel = AccessLevelEnum.user,
                RegisterDate = DateTime.UtcNow
            };
            _userRepository.Create(user);
        }


        [Test, Order(1)]
        public void Create()
        {
            // Arrange
            var urlItem = new UrlItem()
            {
                AuthorId = 1,
                CreatedDate = DateTime.UtcNow,
                OriginalUrl = "OriginalUrlTest1",
                ShortUrl = "ShortUrlTest1",
                TransitionsCount = 0
            };

            // Act
            var result = _urlRepository.Create(urlItem);
            var resultUrl = _urlRepository.GetDetailsById(1);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(result.ShortUrl, urlItem.ShortUrl);
            Assert.True(result.OriginalUrl == urlItem.OriginalUrl);
        }

        [Test, Order(1)]
        public void Create2()
        {
            // Arrange
            var urlItem = new UrlItem()
            {
                AuthorId = 1,
                CreatedDate = DateTime.UtcNow,
                OriginalUrl = "OriginalUrlTest2",
                ShortUrl = "ShortUrlTest2",
                TransitionsCount = 0
            };

            // Act
            var result = _urlRepository.Create(urlItem);
            var resultUrl = _urlRepository.GetDetailsByShortUrl(urlItem.ShortUrl);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(resultUrl.ShortUrl, urlItem.ShortUrl);
            Assert.True(resultUrl.OriginalUrl == urlItem.OriginalUrl);
        }
    }
}
