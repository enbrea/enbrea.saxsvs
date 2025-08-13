#region Enbrea - Copyright (c) STÜBER SYSTEMS GmbH
/*    
 *    Enbrea
 *    
 *    Copyright (c) STÜBER SYSTEMS GmbH
 *
 *    This program is free software: you can redistribute it and/or modify
 *    it under the terms of the GNU Affero General Public License, version 3,
 *    as published by the Free Software Foundation.
 *
 *    This program is distributed in the hope that it will be useful,
 *    but WITHOUT ANY WARRANTY; without even the implied warranty of
 *    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 *    GNU Affero General Public License for more details.
 *
 *    You should have received a copy of the GNU Affero General Public License
 *    along with this program. If not, see <http://www.gnu.org/licenses/>.
 *
 */
#endregion

using Microsoft.Extensions.DependencyInjection;
using Polly;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;

namespace Enbrea.SaxSVS.Tests
{
    /// <summary>
    /// A factory for <see cref="IRestClient"/> instances
    /// </summary>
    public static class RestClientFactory
    {
        /// <summary>
        /// Creates a new <see cref="IRestClient"/> instance
        /// </summary>
        /// <returns>A new <see cref="IRestClient"/> instance</returns>
        public static IRestClient CreateRestClient()
        {
            return CreateRestClientWithAcceptRequestHeader(MediaTypeNames.Application.Xml);
        }

        /// <summary>
        /// Creates a new <see cref="IRestClient"/> instance
        /// </summary>
        /// <param name="mediaType"></param>
        /// <returns>A new <see cref="IRestClient"/> instance</returns>
        public static IRestClient CreateRestClientWithAcceptRequestHeader(string mediaType)
        {
            // Create dependency injection container
            var serviceCollection = new ServiceCollection();

            // Register Http Client
            serviceCollection
                .AddHttpClient<IRestClient, RestClient>(client =>
                {
                    client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(AssemblyInfo.GetAgentName(), AssemblyInfo.GetVersion()));
                })
                .AddPolicyHandler(Policy<HttpResponseMessage>.Handle<HttpRequestException>()
                    .OrResult(msg => msg.StatusCode == HttpStatusCode.RequestTimeout)
                    .OrResult(msg => msg.StatusCode == HttpStatusCode.ServiceUnavailable)
                    .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));

            // Create IRestHttpClient implementation
            var services = serviceCollection.BuildServiceProvider();

            // Return back IRestHttpClient implementation
            return services.GetRequiredService<IRestClient>();
        }
    }
}
