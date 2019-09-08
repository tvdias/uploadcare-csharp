namespace Uploadcare.Tests
{
    internal class TestClient : ClientBase
    {
        public TestClient()
            : base(new TestConfiguration())
        {
        }
    }
}