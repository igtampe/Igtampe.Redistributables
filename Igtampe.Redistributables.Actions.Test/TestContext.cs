using Igtampe.ChopoAuth;
using Igtampe.ChopoImageHandling;
using Igtampe.DBContexts;
using Igtampe.Notifier;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igtampe.Redistributables.Actions.Test {

    /// <summary>This is a test context that runs in memory and contains tables for all needed entities for testing</summary>
    internal class TestContext : DbContext, IImageContext, INotificationContext<Notification<User>, User> {

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public TestContext(DbContextOptions<TestContext> options) : base(options) { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public DbSet<User> User { get; set; }

        public DbSet<Image> Image { get; set; }

        public DbSet<Notification<User>> Notification { get; set; }

        public IQueryable<User> ApplyAutoIncludes(IQueryable<User> Set) => Set;

        public override void Dispose() {
            User.RemoveRange(User);
            Image.RemoveRange(Image);
            Notification.RemoveRange(Notification);
            base.Dispose(); 
        }
    }

    internal class TestContextFactory {
        private readonly DbContextOptions<TestContext> _options;

        public TestContextFactory(string Name = "TEST") => _options = new DbContextOptionsBuilder<TestContext>().UseInMemoryDatabase(Name).Options;

        public TestContextFactory(DbContextOptions<TestContext> options) => _options = options;
        public TestContext Create() => new(_options);
    }
}
