using Microsoft.VisualStudio.TestTools.UnitTesting;
using URent.Models.Manager;

namespace URent.Tests.Manager
{
    /// <summary>
    /// Summary description for OptionManagerTest
    /// </summary>
    [TestClass]
    public class OptionManagerTest
    {
        private readonly OptionManager _manager;

        public OptionManagerTest()
        {
            _manager= new OptionManager();
        }

        /// <summary>
        /// This test works but it shouldn't. This fact underlines dependencies in the code.
        /// </summary>
        [TestMethod]
        public void ListOptions_ExistingOptionList_ItemsReturned()
        {
            //ARRANGE
            // TODO : Change the Helper class from static to instance
            // with an interface that will be passed to the class
            // This will remove dependencies
            //ACT
            var result = _manager.ListOptions();
            //ASSERT
            Assert.IsTrue(result.Count > 0);
        }

        /// <summary>
        /// This class doesn't work or the same reason the other doesn't.
        /// We need to fix a few things.
        /// Because of this we do not know:
        /// What happens if someone tampers with the file?
        /// When or how is the list null?
        /// etcera
        /// </summary>
        [TestMethod]
        public void ListOptions_NoCurrentOptionList_EmptyListReturned()
        {
            //ARRANGE
            //ACT
            var result = _manager.ListOptions();
            //ASSERT
            Assert.IsTrue(result.Count == 0);
        }
    }
}
