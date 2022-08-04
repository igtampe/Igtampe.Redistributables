using Igtampe.Actions;
using Igtampe.ChopoSessionManager;
using Igtampe.ChopoSessionManager.Exceptions;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Igtampe.Redistributables.Actions.Test {
    public class SessionedActionAgentTests {

        private readonly ISessionManager Manager = SessionManager.Manager;
        private readonly SessionedActionAgent<TestContext> Agent;

        private const string TEST_USER = "Dingus Dongus";
        private const string NOT_TEST_USER = "Dongus Dingus";

        public SessionedActionAgentTests() {
            TestContext Context = SetupContext();
            Agent = new SessionedActionAgent<TestContext>(Context, Manager);
        }

        private static TestContext SetupContext() { 
            var Factory = new TestContextFactory(nameof(SessionedActionAgentTests));
            var Context = Factory.Create();

            //Any setup goes here

            return Context;
        }

        [Test]
        public async Task GetSessionThatExists() {
            Guid ID = Manager.LogIn(TEST_USER);
            Manager.LogIn(NOT_TEST_USER);
            Manager.LogIn(NOT_TEST_USER);
            Manager.LogIn(NOT_TEST_USER);

            var S = await Agent.GetSession(ID);

            Assert.AreEqual(TEST_USER,S.Username, "Incorrect session found");
        }

        [Test]
        public void GetSessionThatDoesnotExist() => 
            Assert.ThrowsAsync<SessionNotFoundException>(async () => await Agent.GetSession(Guid.Empty));

    }
}