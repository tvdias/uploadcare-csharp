using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Uploadcare.DTO;
using Uploadcare.Exceptions;

namespace Uploadcare
{
    public abstract class ClientBase : IDisposable
    {
        protected readonly Configuration _configuration;
        protected readonly HttpClient _httpClient;

        private static readonly JsonSerializer _serializer = JsonSerializer.Create(
            new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            });

        private readonly bool _disposeClient;

        protected ClientBase(Configuration configuration, HttpClient httpClient = null)
        {
            this._configuration = configuration;

            this._disposeClient = httpClient == null;

            this._httpClient = httpClient ?? new HttpClient();

            this.EnsureHeaders();
        }

        public static async Task<T> ExecuteRequestAsync<T>(HttpClient httpClient, HttpRequestMessage request, CancellationToken cancellationToken = default)
        {
            using (var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false))
            {
                using (var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        throw ProcessError(response, responseStream);
                    }

                    return DeserializeJsonFromStream<T>(responseStream);
                }
            }
        }

        public static async Task ExecuteRequestAsync(HttpClient httpClient, HttpRequestMessage request, CancellationToken cancellationToken = default)
        {
            using (var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false))
            {
                if (!response.IsSuccessStatusCode)
                {
                    var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

                    try
                    {
                        throw ProcessError(response, stream);
                    }
                    finally
                    {
                        stream.Dispose();
                    }
                }
            }
        }

        public void Dispose()
        {
            if (this._disposeClient)
            {
                this._httpClient.Dispose();
            }
        }

        private static T DeserializeJsonFromStream<T>(Stream stream)
        {
            if (stream?.CanRead != true)
            {
                return default;
            }

            using (var streamReader = new StreamReader(stream))
            using (var textReader = new JsonTextReader(streamReader))
            {
                return _serializer.Deserialize<T>(textReader);
            }
        }

        private static UploadcareException ProcessError(HttpResponseMessage response, Stream stream)
        {
            var exceptionMessage = DeserializeJsonFromStream<Error>(stream);

            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    return new InvalidRequestException(exceptionMessage.Detail);

                case HttpStatusCode.Unauthorized:
                    return new UnauthorizedException(exceptionMessage.Detail);

                case HttpStatusCode.Forbidden:
                    return new ForbiddenException(exceptionMessage.Detail);

                case HttpStatusCode.NotFound:
                    return new NotFoundException(exceptionMessage.Detail);

                case HttpStatusCode.MethodNotAllowed:
                    return new InvalidRequestException(exceptionMessage.Detail);
            }

            return new UnknownException(exceptionMessage.Detail);
        }

        private void EnsureHeaders()
        {
            if (!this._httpClient.DefaultRequestHeaders.Contains("Accept"))
            {
                this._httpClient.DefaultRequestHeaders.Add("Accept", this._configuration.ApiVersion);
            }

            if (!this._httpClient.DefaultRequestHeaders.Contains("Authorization"))
            {
                this._httpClient.DefaultRequestHeaders.Add(
                        "Authorization",
                        $"Uploadcare.Simple {this._configuration.PublicKey}:{this._configuration.PrivateKey}");
            }
        }
    }
}