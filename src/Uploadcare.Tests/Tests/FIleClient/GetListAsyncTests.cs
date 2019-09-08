using System;
using System.Threading.Tasks;
using Xunit;

namespace Uploadcare.Tests
{
    public class GetListAsyncTests : BaseFileClientTests
    {
        [Fact]
        public async Task DefaultFilter_NotEmptyList()
        {
            var filter = new DTO.GetFilesFilter();

            var result = await this.target
                .GetListAsync(filter)
                .ConfigureAwait(false);

            Assert.True(result.Total > 0);
            Assert.NotEmpty(result.Results);
        }

        [Fact]
        public async Task FromDatetimeUploadedNow_EmptyList()
        {
            var filter = new DTO.GetFilesFilter()
            {
                FromDatetimeUploaded = DateTime.UtcNow
            };

            var result = await this.target
                .GetListAsync(filter)
                .ConfigureAwait(false);

            Assert.Empty(result.Results);
        }
    }
}