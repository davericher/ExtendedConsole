using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace ExtendedConsole.tests
{
    [TestClass]
    public class HelpersTest
    {
        // Check to see if pluralization works in EN-US
        [TestMethod]
        [TestCategory("String Modifiers")]
        public void TestPlural()
        {
            string test1 = Helpers.Pluralize("knife");
            string test2 = Helpers.Pluralize("dog");
            Assert.AreEqual("knives",test1);
            Assert.AreNotEqual("knifes",test1);
            Assert.AreEqual("dogs",test2);
        }

//        [TestMethod]
//        [TestCategory("String Modifiers")]
//        public void TestSingular()
//        {
//            string test1 = Helpers.Singular("knives");
//            string test2 = Helpers.Singular("dogs");
//            Assert.AreEqual("knife", test1);
//            Assert.AreNotEqual("knive", test1);
//            Assert.AreEqual("dog", test2);
//        }

        // Test to see if Camel Case is split up correctly
        [TestMethod]
        [TestCategory("String Modifiers")]
        public void TestCamelCaseToUpperString()
        {
            string camelCase = Helpers.SplitCamelCase("thisIsATest");
            Assert.AreEqual("This Is A Test",camelCase);
        }

        // Test to see if first leter gets change to uppercase
        [TestMethod]
        [TestCategory("String Modifiers")]
        public void TestFirstToUppercase()
        {
            string firstToUpper = Helpers.UppercaseFirst("test");
            Assert.AreEqual("Test",firstToUpper);
        }
    }
}
