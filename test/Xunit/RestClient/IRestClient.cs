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

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Enbrea.SaxSVS.Tests
{
    /// <summary>
    /// A typed HTTP client interface
    /// </summary>
    public interface IRestClient
    {
        /// <summary>
        /// Request an API endpoint and return back the raw response stream
        /// </summary>
        /// <param name="requestUrl">The request url</param>
        /// <param name="mediaType">The request media type</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult parameter 
        /// contains the stream.</returns>
        Task<Stream> GetStreamAsync(Uri requestUrl, string mediaType, CancellationToken cancellationToken = default);
    }
}
