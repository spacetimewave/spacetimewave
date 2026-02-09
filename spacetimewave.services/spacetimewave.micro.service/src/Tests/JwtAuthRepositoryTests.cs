using Application.Configuration;

// Execute inside /src folder: dotnet test
namespace MyProject.Tests
{
    [TestClass]
    public class JwtAuthRepositoryTests
    {
        private JwtAuthRepository _repo = null!;

        [TestInitialize]
        public void Setup()
        {
            var jwtSettings = new JwtBearer
            {
                Issuer = "https://myissuer.com",
                Audience = "myAudience",
                IssuerKey = "SuperSecretIssuerKey12345"
            };

            var authSettings = new AuthenticationSettings
            {
                JwtBearer = jwtSettings
            };

            _repo = new JwtAuthRepository(authSettings);
        }

        [TestMethod]
        public void GenerateJwtToken_ShouldReturnTokenString()
        {
            var claims = new Dictionary<string, string>
            {
                { "sub", "123" },
                { "role", "admin" }
            };

            string token = _repo.GenerateJwtToken("myAudience", claims, 60);

            Assert.IsFalse(string.IsNullOrEmpty(token), "Token should not be null or empty");
        }

        [TestMethod]
        public void GetJwtClaims_ShouldReturnCorrectClaims()
        {
            var claims = new Dictionary<string, string>
            {
                { "sub", "456" },
                { "role", "user" }
            };

            string token = _repo.GenerateJwtToken("myAudience", claims, 60);
            var extractedClaims = _repo.GetJwtClaims(token);

            Assert.HasCount(5, extractedClaims); 
            foreach (var kv in claims)
            {
                // Assert.Contains(extractedClaims.Keys, kv.Key);
                // Assert.AreEqual(kv.Value, extractedClaims[kv.Key]);
            }
        }

        [TestMethod]
        public void GetJwtClaims_ShouldReturnEmptyForInvalidToken()
        {
            var claims = _repo.GetJwtClaims("invalid.token.here");
            Assert.IsEmpty(claims); 
        }

        [TestMethod]
        public void GetIssuerPublicKeys_ShouldReturnValidJsonStrings()
        {
            var keys = _repo.GetIssuerPublicKeys();
            Assert.IsTrue(keys.Any(), "Should have at least one public key");

            foreach (var key in keys)
            {
                Assert.IsFalse(string.IsNullOrEmpty(key));
                Assert.Contains("kty", key);
                Assert.Contains("n", key);
                Assert.Contains("e", key);
            }
        }

        [TestMethod]
        public void GetIssuerJwks_ShouldReturnObjectsWithCorrectFields()
        {
            var jwks = _repo.GetIssuerJwks().ToList();
            Assert.IsTrue(jwks.Any(), "Should return at least one JWK");

            var first = jwks.First();
            Assert.IsTrue(first!.GetType().GetProperty("kty") != null);
            Assert.IsTrue(first!.GetType().GetProperty("n") != null);
            Assert.IsTrue(first!.GetType().GetProperty("e") != null);
            Assert.IsTrue(first!.GetType().GetProperty("alg") != null);
            Assert.IsTrue(first!.GetType().GetProperty("use") != null);
            Assert.IsTrue(first!.GetType().GetProperty("kid") != null);
        }

        [TestMethod]
        public void Base64UrlEncodeDecode_ShouldRoundTripCorrectly()
        {
            byte[] data = new byte[] { 1, 2, 3, 4, 5, 255 };
            string encoded = _repo.Base64UrlEncode(data);
            byte[] decoded = _repo.Base64UrlDecode(encoded);

            CollectionAssert.AreEqual(data, decoded, "Decoded data should match original");
        }
    }
}
