using EquipmentInventory.Classes.Services;
using EquipmentInventory.UI.Tests.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EquipmentInventory.UI.Tests.Tests
{
    [TestClass]
    public class UserAccountServiceTests
    {
        public TestContext TestContext { get; set; }
        public ContextMessage contextMessage;

        [TestInitialize]
        public void TestInitialize()
        {
            contextMessage = new ContextMessage(TestContext);
            contextMessage.StartMessage();
        }

        [TestMethod]
        public void GetGeneratedPassword_ShouldReturnValidString()
        {
            const int minLength = 5;
            const int maxLength = 15;

            var password = UserAccountService.GetGeneratedPassword();

            Assert.IsTrue(password.Length >= minLength,
                $"Password length should be >= {minLength}. Actual: {password.Length}");
            Assert.IsTrue(password.Length <= maxLength,
                $"Password length should be <= {maxLength}. Actual: {password.Length}");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            contextMessage.EndMessage();
        }
    }
}
