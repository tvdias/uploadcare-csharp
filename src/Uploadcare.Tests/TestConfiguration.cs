namespace Uploadcare.Tests
{
    public class TestConfiguration : Configuration
    {
        public TestConfiguration()
            : base("demopublicKey", "demoprivateKey")
        {
        }
    }
}