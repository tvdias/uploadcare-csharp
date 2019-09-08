using System;
using System.Linq;
using System.Threading.Tasks;
using Uploadcare.Exceptions;
using Xunit;

namespace Uploadcare.Tests
{
    public class CopyAsyncTests : BaseFileClientTests
    {
        [Fact]
        public async Task SourceFileIdRandom_ThrowNotFound()
        {
            await Assert.ThrowsAsync<NotFoundException>(() => this.target.CopyAsync(Guid.NewGuid())).ConfigureAwait(false);
        }

        [Fact]
        public async Task SourceRandomCDN_ThrowNotFound()
        {
            var source = new Uri("https://ucarecdn.com/" + Guid.NewGuid());
            await Assert.ThrowsAsync<NotFoundException>(() => this.target.CopyAsync(source, "target")).ConfigureAwait(false);
        }

        [Fact]
        public async Task SourceAnyURI_ThrowInvalidRequest()
        {
            await Assert.ThrowsAsync<InvalidRequestException>(
                () => this.target
                    .CopyAsync(
                        new Uri("https://www.google.com"),
                        "target"))
                    .ConfigureAwait(false);
        }

        /// <summary>
        /// Just to be used as boilerplace since custom storage isn't allowed with demo key
        /// </summary>
        /// <remarks>Copy to a custom storage isn't allowed with demo key</remarks>
        [Fact]
        public async Task SourceIsValid_ThrowInvalidRequest()
        {
            var filter = new DTO.GetFilesFilter();

            var files = await this.target
                .GetListAsync(filter)
                .ConfigureAwait(false);

            await Assert.ThrowsAsync<InvalidRequestException>(
                () => this.target
                    .CopyAsync(
                        new Uri(files.Results.First().OriginalFileUrl),
                        "target"))
                    .ConfigureAwait(false);
        }
    }
}