using FlexibleProxy;
using FlexibleProxy.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlexibleProxyTests
{
    [TestClass]
    public class ArgumentsParserTests
    {
        [TestMethod]
        public void ParseTest()
        {
            var parser = new ArgumentsParser(new[] { "-Arg1", "Value1", "-Arg2", "/Arg3", "Value2" });
            Assert.IsTrue(parser.GetValue<bool>("Arg2"));
            Assert.AreEqual(parser.GetValue<string>("Arg1"), "Value1");
            Assert.AreEqual(parser.GetValue<string>("Arg3"), "Value2");
        }
    }
}
