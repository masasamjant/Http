namespace Masasamjant.Http
{
    [TestClass]
    public class HttpHeaderValidatorUnitTest : UnitTest
    {
        [TestMethod]
        public void Test_IsValidHeaderName()
        {
            Assert.IsFalse(HttpHeaderValidator.IsValidHeaderName(string.Empty));
            Assert.IsFalse(HttpHeaderValidator.IsValidHeaderName("  "));
            Assert.IsTrue(HttpHeaderValidator.IsValidHeaderName("header"));
            Assert.IsTrue(HttpHeaderValidator.IsValidHeaderName("Header1"));
            Assert.IsTrue(HttpHeaderValidator.IsValidHeaderName("1234567890"));
            Assert.IsTrue(HttpHeaderValidator.IsValidHeaderName("Header-1"));
            Assert.IsTrue(HttpHeaderValidator.IsValidHeaderName("Header_1"));
            Assert.IsTrue(HttpHeaderValidator.IsValidHeaderName("ABCDEFGHIJKLMNOPQRSTUVWXYZ"));
            Assert.IsTrue(HttpHeaderValidator.IsValidHeaderName("abcdefghijklmnopqrstuvwxyz"));
            Assert.IsTrue(HttpHeaderValidator.IsValidHeaderName("-_"));
            Assert.IsFalse(HttpHeaderValidator.IsValidHeaderName("ABCD+1"));
            Assert.IsFalse(HttpHeaderValidator.IsValidHeaderName("ÄÄNI-1"));
            Assert.IsFalse(HttpHeaderValidator.IsValidHeaderName("!"));
        }

        [TestMethod]
        public void Test_IsValidHeaderValue()
        {
            Assert.IsTrue(HttpHeaderValidator.IsValidHeaderValue(null));
            Assert.IsTrue(HttpHeaderValidator.IsValidHeaderValue(string.Empty));
            Assert.IsTrue(HttpHeaderValidator.IsValidHeaderValue("  "));
            Assert.IsTrue(HttpHeaderValidator.IsValidHeaderValue("_ :;.,\\/\"'?!(){}[]@<>=-+*#$&`|~^%"));
            Assert.IsFalse(HttpHeaderValidator.IsValidHeaderValue("½"));
            Assert.IsTrue(HttpHeaderValidator.IsValidHeaderValue("1234567890"));
            Assert.IsFalse(HttpHeaderValidator.IsValidHeaderValue("Ä"));
            Assert.IsFalse(HttpHeaderValidator.IsValidHeaderValue("ä"));
            Assert.IsFalse(HttpHeaderValidator.IsValidHeaderValue("Ö"));
            Assert.IsFalse(HttpHeaderValidator.IsValidHeaderValue("ö"));
            Assert.IsFalse(HttpHeaderValidator.IsValidHeaderValue("Å"));
            Assert.IsFalse(HttpHeaderValidator.IsValidHeaderValue("å"));
            Assert.IsTrue(HttpHeaderValidator.IsValidHeaderValue("ABCDEFGHIJKLMNOPQRSTUVWXYZ"));
            Assert.IsTrue(HttpHeaderValidator.IsValidHeaderValue("abcdefghijklmnopqrstuvwxyz"));
        }
    }
}
