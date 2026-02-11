using Application.Configuration;

// Execute inside /src folder: dotnet test
namespace MyProject.Tests
{
    [TestClass]
    public class RepositoryTests
    {

        [TestMethod]
        public void SampleTest()
        {
            byte[] data = new byte[0];
            byte[] decoded = new byte[0];

            CollectionAssert.AreEqual(data, decoded, "Decoded data should match original");
        }
    }
}
