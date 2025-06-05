using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EquipmentInventory.UI.Tests.Classes
{
    public class ContextMessage
    {
        private TestContext _context;

        public ContextMessage(TestContext testContext) 
        { 
            _context = testContext; 
        }

        public void StartMessage()
        {
            _context.WriteLine("TestRunDirectory {0}", _context.TestRunDirectory);
            _context.WriteLine("TestName {0}", _context.TestName);
            _context.WriteLine("CurrentTestOutcome {0}", _context.CurrentTestOutcome);
        }

        public void EndMessage()
        {
            _context.WriteLine("TestName (CleanUp) {0}", _context.TestName);
            _context.WriteLine("CurrentTestOutcome (CleanUp) {0}", _context.CurrentTestOutcome);
        }
    }
}
