using EquipmentInventory.Classes.Helpers;
using EquipmentInventory.UI.Tests.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EquipmentInventory.UI.Tests.Tests
{
	[TestClass]
	public class ValidationHelperTests
	{
        public TestContext TestContext { get; set; }
        public ContextMessage contextMessage;

        [TestInitialize]
        public void TestInitialize()
        {
            contextMessage = new ContextMessage(TestContext);
            contextMessage.StartMessage();
        }

        [DataTestMethod]
        [DataRow("123", true)]
        [DataRow("12.34", false)]
        [DataRow("abc", false)]
        [DataRow("12a34", false)]
        [DataRow("", false)]
        [DataRow(null, false)]
        public void IsValidNumber_ShouldValidateCorrectly(string input, bool expected)
        {
            bool result = ValidationHelper.IsValidNumber(input);

            Assert.AreEqual(expected, result, $"The wrong template. Verified value:: {input}");
        }

        [DataTestMethod]
        [DataRow("12.12.2023", true)]
        [DataRow("12122023", true)]
        [DataRow("12/12/2023", false)]
        [DataRow("12-Dec-2023", false)]
        [DataRow("", false)]
        [DataRow(null, false)]
        public void IsValidDate_ShouldValidateCorrectly(string input, bool expected)
        {
            bool result = ValidationHelper.IsValidDate(input);

            Assert.AreEqual(expected, result, $"The wrong template. Verified value:: {input}");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            contextMessage.EndMessage();
        }
    }
}
