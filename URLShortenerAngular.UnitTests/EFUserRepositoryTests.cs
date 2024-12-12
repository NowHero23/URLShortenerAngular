using Microsoft.EntityFrameworkCore;
using URLShortenerAngular.Server.Data;
using URLShortenerAngular.Server.Data.Domain.Repositories.Abstract;
using URLShortenerAngular.Server.Data.Domain.Repositories.EntityFramework;
using URLShortenerAngular.Server.Data.Enums;
using URLShortenerAngular.Server.Models;

namespace URLShortenerAngular.UnitTests
{
    public class EFUserRepositoryTests
    {
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
        }

        [Test, Order(1)]
        public void Create()
        {
            // Arrange
            User user = new User()
            {
                Login = "userTest1",
                Password = "userTest1",
                AccessLevel = AccessLevelEnum.user,
                RegisterDate = DateTime.UtcNow
            };

            // Act
            var result = _userRepository.Create(user);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Id);
            Assert.True(result.Login == user.Login);
        }

        [Test, Order(2)]
        public void GetByLogin()
        {
            // Arrange
            User user = new User()
            {
                Login = "userTest2",
                Password = "userTest2",
                AccessLevel = AccessLevelEnum.user,
                RegisterDate = DateTime.UtcNow
            };

            // Act
            _userRepository.Create(user);
            var result = _userRepository.GetByLogin(user.Login);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Id);
            Assert.True(result.Login == user.Login);
        }
        [Test, Order(3)]
        public void GetById()
        {
            // Arrange
            User user = new User()
            {
                Login = "userTest3",
                Password = "userTest3",
                AccessLevel = AccessLevelEnum.user,
                RegisterDate = DateTime.UtcNow
            };

            // Act
            var id = _userRepository.Create(user).Id;
            var result = _userRepository.GetById(id);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Id);
            Assert.AreEqual(result.Id, id);
            Assert.True(result.Login == user.Login);
        }
    }
}
