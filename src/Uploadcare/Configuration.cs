namespace Uploadcare
{
    public class Configuration
    {
        public string ApiVersion = "application/vnd.uploadcare-v0.5+json";

        public string PublicKey { get; set; }

        public string PrivateKey { get; set; }

        public Configuration(string publicKey, string privateKey)
        {
            if (string.IsNullOrEmpty(publicKey))
            {
                throw new System.ArgumentException(nameof(publicKey));
            }

            if (string.IsNullOrEmpty(privateKey))
            {
                throw new System.ArgumentException(nameof(privateKey));
            }

            this.PublicKey = publicKey;
            this.PrivateKey = privateKey;
        }
    }
}