namespace Uploadcare.Tests
{
    public abstract class BaseFileClientTests
    {
        protected readonly FileClient target;

        protected BaseFileClientTests()
        {
            this.target = new FileClient(new TestConfiguration());
        }
    }
}