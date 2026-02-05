using Masasamjant.Collections;

namespace Masasamjant.Http
{
    [TestClass]
    public class HttpPostRequestUnitTest : UnitTest
    {
        [TestMethod]
        public void Test_Constructor()
        {
            var data1 = new PostData("test");
            var request1 = new HttpPostRequest("api/Test", data1);
            Assert.IsTrue(ReferenceEquals(data1, request1.Data));
            Assert.AreEqual("api/Test", request1.GetFullRequestUri());

            var data2 = new CloneablePostData("test");
            var request2 = new HttpPostRequest<CloneablePostData>("api/Test", data2);
            Assert.IsTrue(ReferenceEquals(data2, request2.Data));
            Assert.AreEqual("api/Test", request2.GetFullRequestUri());
        }

        [TestMethod]
        public void Test_Clone()
        {
            var request1 = new HttpPostRequest("api/Test", new PostData("test"));
            var clone1 = request1.Clone();
            Assert.IsTrue(ReferenceEquals(clone1.Data, request1.Data));
            Assert.AreEqual(clone1.Data, request1.Data);
            Assert.AreEqual(clone1.GetFullRequestUri(), request1.GetFullRequestUri());
            object copy = ((ICloneable)request1).Clone();
            Assert.IsInstanceOfType<HttpPostRequest>(copy);
            request1 = (HttpPostRequest)copy;
            Assert.IsTrue(ReferenceEquals(clone1.Data, request1.Data));
            Assert.AreEqual(clone1.Data, request1.Data);
            Assert.AreEqual(clone1.GetFullRequestUri(), request1.GetFullRequestUri());

            request1 = new HttpPostRequest("api/Test", new CloneablePostData("test"));
            clone1 = request1.Clone();
            Assert.IsFalse(ReferenceEquals(clone1.Data, request1.Data));
            Assert.AreEqual(clone1.Data, request1.Data);
            Assert.AreEqual(clone1.GetFullRequestUri(), request1.GetFullRequestUri());

            var request2 = new HttpPostRequest<PostData>("api/Test", new PostData("test"));
            var clone2 = request2.Clone();
            Assert.IsTrue(ReferenceEquals(clone2.Data, request2.Data));
            Assert.AreEqual(clone2.Data, request2.Data);
            Assert.AreEqual(clone2.GetFullRequestUri(), request2.GetFullRequestUri());

            var request3 = new HttpPostRequest<CloneablePostData>("api/Test", new CloneablePostData("test"));
            var clone3 = request3.Clone();
            Assert.IsFalse(ReferenceEquals(clone3.Data, request3.Data));
            Assert.AreEqual(clone3.Data, request3.Data);
            Assert.AreEqual(clone3.GetFullRequestUri(), request3.GetFullRequestUri());

            copy = ((ICloneable)request3).Clone();
            Assert.IsInstanceOfType<HttpPostRequest<CloneablePostData>>(copy);
            clone3 = (HttpPostRequest<CloneablePostData>)copy;
            Assert.IsFalse(ReferenceEquals(clone3.Data, request3.Data));
            Assert.AreEqual(clone3.Data, request3.Data);
            Assert.AreEqual(clone3.GetFullRequestUri(), request3.GetFullRequestUri());
        }

        private class PostData
        {
            public PostData(string data)
            {
                Data = data;
            }
            public string Data { get; }

            public override bool Equals(object? obj)
            {
                return obj is PostData other && Data == other.Data;
            }

            public override int GetHashCode()
            {
                return Data.GetHashCode();
            }
        }

        private class CloneablePostData : PostData, ICloneable
        {
            public CloneablePostData(string data)
                : base(data)
            { }

            public object Clone()
            {
                return new CloneablePostData(Data);
            }
        }
    }
}
