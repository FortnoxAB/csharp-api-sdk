using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Fortnox.SDK.Connectors.Base;
using Fortnox.SDK.Entities;
using Fortnox.SDK.Interfaces;
using Fortnox.SDK.Requests;
using Fortnox.SDK.Utility;

// ReSharper disable UnusedMember.Global

namespace Fortnox.SDK.Connectors
{
    /// <remarks/>
    internal class AuthorizationConnector : BaseConnector, IAuthorizationConnector
    {
        public AuthorizationConnector()
        {
            Resource = "";
            UseAuthHeaders = false;
        }

        public string GetAccessToken(string authorizationCode, string clientSecret)
        {
            return GetAccessTokenAsync(authorizationCode, clientSecret).GetResult();
        }

        public async Task<string> GetAccessTokenAsync(string authorizationCode, string clientSecret)
        {
            var fortnoxRequest = new BaseRequest()
            {
                Method = HttpMethod.Get,
                Resource = Resource,
                Headers = new Dictionary<string, string>()
                {
                    { "Authorization-Code", authorizationCode },
                    { "Client-Secret", clientSecret },
                    { "Accept", "application/json" }
                }
            };

            var responseData = await SendAsync(fortnoxRequest).ConfigureAwait(false);
            var responseJson = Encoding.UTF8.GetString(responseData);
            var result = Serializer.Deserialize<EntityWrapper<AuthorizationData>>(responseJson);

            var accessToken = result?.Entity.AccessToken;
            return accessToken;
        }
    }
}
