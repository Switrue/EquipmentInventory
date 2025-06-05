using EquipmentInventory.Classes.Helpers;
using EquipmentInventory.UI.Tests.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace EquipmentInventory.UI.Tests.Tests
{
	[TestClass]
	public class QueryParamsHelperTests
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
        public void ToQueryParams_ValidObject_ReturnsCorrectDictionary()
        {
            var testObj = new TestObject
            {
                Name = "Test",
                Date = new DateTime(2025, 06, 5),
                Value = 123.45f,
                IsActive = true
            };

            var expected = new Dictionary<string, string>
            {
                { "Name", "Test" },
                { "Date", "2025-06-05T00:00:00.0000000" }, // ISO 8601 формат
                { "Value", "123.45" },
                { "IsActive", "true" }
            };

            var actual = QueryParamsHelper.ToQueryParams(testObj);

            AssertQueryParams(expected, actual);
        }

        [TestMethod]
        public void ToQueryParams_ObjectWithNullProperties_ReturnsCorrectDictionary()
        {
            var testObj = new TestObject
            {
                Name = null,
                Date = DateTime.Now,
                Value = 0f,
                IsActive = false
            };

            var expected = new Dictionary<string, string>
            {
                { "Date", testObj.Date.ToString("o") }, // ISO 8601 формат
                { "Value", "0" },
                { "IsActive", "false" }
            };

            var actual = QueryParamsHelper.ToQueryParams(testObj);

            AssertQueryParams(expected, actual);
        }

        [TestMethod]
        public void ToQueryParams_NullObject_ReturnsEmptyDictionary()
        {
            var result = QueryParamsHelper.ToQueryParams(null);

            Assert.AreEqual(0, result.Count, "The resulting dictionary should not contain elements");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            contextMessage.EndMessage();
        }

        private void AssertQueryParams(Dictionary<string, string> expected, Dictionary<string, string> actual)
        {
            Assert.AreEqual(expected.Count, actual.Count, "The number of parameters does not match");
            foreach (var kvp in expected)
            {
                Assert.IsTrue(actual.ContainsKey(kvp.Key), $"The key '{kvp.Key}' is absent as a result");
                Assert.AreEqual(kvp.Value, actual[kvp.Key], $"Meaning for the key '{kvp.Key}' does not match");
            }
        }
    }
}
