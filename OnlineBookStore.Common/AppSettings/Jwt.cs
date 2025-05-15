using System.Text.Json.Serialization;

namespace OnlineBookStore.Common.AppSettings
{
    public class Jwt
    {
        [JsonPropertyName("key")]
        public string Key { get; init; } = string.Empty;

        [JsonPropertyName("audience")]
        public string Audience { get; init; } = string.Empty;

        [JsonPropertyName("issuer")]
        public string Issuer { get; init; } = string.Empty;

        [JsonPropertyName("accessTokenExpiryMinutes")]
        public int AccessTokenExpiryMinutes { get; init; } = 60;

        [JsonPropertyName("refreshTokenExpiryMinutes")]
        public int RefreshTokenExpiryMinutes { get; init; } = 10;
        
        [JsonPropertyName("dIWebAppUrl")]
        public string DIWebAppUrl { get; init; } = string.Empty;

        [JsonPropertyName("secret")]
        public string Secret { get; init; } = string.Empty;

    }
}
