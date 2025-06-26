namespace Masasamjant.Http
{
    [TestClass]
    public class HttpParameterCollectionUnitTest : UnitTest
    {
        [TestMethod]
        public void Test_Constructor()
        {
            var parameters = new HttpParameterCollection();
            Assert.IsTrue(parameters.Count == 0);
        }

        [TestMethod]
        public void Test_Add()
        {
            var parameters = new HttpParameterCollection();
            var parameter = new HttpParameter("name", "1");
            parameters.Add(parameter);
            Assert.IsTrue(parameters.Count == 1);
            Assert.IsTrue(parameters.Contains(parameter));
            Assert.ThrowsException<ArgumentException>(() => parameters.Add(new HttpParameter("name", "2")));
            parameter = parameters.Add("test", "2");
            Assert.IsTrue(parameter.Value?.Equals("2"));
            Assert.IsTrue(parameter.Name == "test");
            Assert.IsTrue(parameters.Count == 2);
        }

        [TestMethod]
        public void Test_Clear()
        {
            var headers = new HttpParameterCollection();
            headers.Add("1", "1");
            headers.Add("2", "2");
            Assert.IsTrue(headers.Count == 2);
            headers.Clear();
            Assert.IsTrue(headers.Count == 0);
        }

        [TestMethod]
        public void Test_Contains()
        {
            var parameters = new HttpParameterCollection();
            var parameter = new HttpParameter("name", "1");
            parameters.Add(parameter);
            Assert.IsTrue(parameters.Contains(parameter));
            Assert.IsTrue(parameters.Contains("name"));
            Assert.IsFalse(parameters.Contains(new HttpParameter("test", "1")));
            Assert.IsFalse(parameters.Contains("test"));
        }

        [TestMethod]
        public void Test_Remove()
        {
            var parameters = new HttpParameterCollection();
            Assert.IsFalse(parameters.Remove("name"));
            var parameter = new HttpParameter("name", "test");
            parameters.Add(parameter);
            Assert.IsTrue(parameters.Remove("name"));
            Assert.IsFalse(parameters.Remove(parameter));
            parameters.Add("name", "foo");
            Assert.IsTrue(parameters.Remove(parameter));
        }

        [TestMethod]
        public void Test_Create()
        {
            object instance = new Pet() 
            {
                Name = "Wolf",
                Age = 10
            };
            var parameters = HttpParameterCollection.Create(instance);
            var nameParameter = parameters.FirstOrDefault(x => x.Name == "name");
            var ageParameter = parameters.FirstOrDefault(x => x.Name == "age");
            Assert.IsTrue(nameParameter != null && nameParameter.Value!.Equals("Wolf"));
            Assert.IsTrue(ageParameter != null && ageParameter.Value!.Equals(10));
        }

        [TestMethod]
        public void Test_Get()
        {
            var parameters = new HttpParameterCollection();
            var parameter = parameters.Get("name");
            Assert.IsNull(parameter);
            parameters.Add("name", "test");
            parameter = parameters.Get("name");
            Assert.IsNotNull(parameter);
        }

        private class Pet
        {
            [HttpParameter("name")]
            public string? Name { get; set; }

            [HttpParameter("age")]
            public int Age { get; set; }
        }
    }
}
