using Igtampe.Actions;
using Igtampe.ChopoAuth;
using Igtampe.ChopoImageHandling;
using Igtampe.ChopoImageHandling.Exceptions;
using Igtampe.ChopoSessionManager;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Igtampe.Redistributables.Actions.Test {
    public class ImageAgentTest {

        private static readonly User[] Users = { new() { Username = "Chopo", IsAdmin = true, Name = "Chopo" }, };

        private static readonly Image[] Images = {
            new() { ID = Guid.NewGuid(), Data=new byte[] { 0x1, 0x2, 0x3 } },
            new() { ID = Guid.NewGuid(), Data=new byte[] { 0x4, 0x5, 0x6 } },
            new() { ID = Guid.NewGuid(), Data=new byte[] { 0x7, 0x8, 0x9 } },
        };

        private readonly ISessionManager Manager = SessionManager.Manager;
        private readonly ImageAgent<TestContext, User> Agent;
        private TestContext Context;

        public ImageAgentTest() {
            Context = SetupContext();
            Agent = new(Context, Manager);
        }

        private TestContext SetupContext() {
            var Factory = new TestContextFactory(nameof(ImageAgentTest));
            Context = Factory.Create();

            Manager.Reset();
            FillContext();
            return Context;
        }

        private void FillContext() {
            Context.AddRange(Users);
            Context.AddRange(Images);
            Context.SaveChanges();
        }

        [Test]
        public void GetImageFailTest() 
            => _ = Assert.ThrowsAsync<ImageNotFoundException>(async () => await Agent.GetImage(System.Guid.Empty), "An image that doesn't exist was found!");

        [Test]
        public async Task GetImageTest() {
            Image I = await Agent.GetImage(Images[0].ID);
            Assert.AreEqual(Images[0].Data, I.Data);
        }

        [Test]
        public async Task CreateImageTest() {

            var SessionID = Manager.LogIn(Users[0].Username);

            Image ToCreate = new() { Data = new byte[] { 0x15, 0x16, 0x17 } };
            Image Created = await Agent.CreateImage(SessionID, ToCreate);
            Assert.AreEqual(ToCreate.Data, Created.Data);
            Assert.IsNotNull(Created.ID);
            Assert.AreNotEqual(System.Guid.Empty, Created.ID);
        }
    }
}