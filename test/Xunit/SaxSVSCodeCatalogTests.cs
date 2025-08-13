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
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Enbrea.SaxSVS.Tests
{
    /// <summary>
    /// Unit tests for <see cref="SaxSVSCode"/>.
    /// </summary>
    public class SaxSVSCodeCatalogTests
    {
        [Fact]
        public async Task Test_GenderCodes()
        {
            var restClient = RestClientFactory.CreateRestClient();
            using var stream = await restClient.GetStreamAsync(new Uri("https://web1.extranet.sachsen.de/bbsp/public/schnittstellen/Schluessel.xml"), MediaTypeNames.Application.Xml);

            using var strReader = new StreamReader(stream, Encoding.UTF8);

            var saxSVSReader = new SaxSVSReader(strReader);
            var codeCatalog = await saxSVSReader.ReadCodeCatalogAsync();

            Assert.Equal(64, codeCatalog.Count);
            Assert.Equal("männl.", codeCatalog[0].Codes[0].ShortName);
            Assert.Equal("männlich", codeCatalog[0].Codes[0].LongName);
            Assert.Equal("1", codeCatalog[0].Codes[0].Code);
            Assert.Equal(new DateTime(2005, 1, 1, 0, 0, 0), codeCatalog[0].Codes[0].ValidFrom);
            Assert.Null(codeCatalog[0].Codes[0].ValidTo);
            Assert.Equal("weibl.", codeCatalog[0].Codes[1].ShortName);
            Assert.Equal("weiblich", codeCatalog[0].Codes[1].LongName);
            Assert.Equal("2", codeCatalog[0].Codes[1].Code);
            Assert.Equal(new DateTime(2005, 1, 1, 0, 0, 0), codeCatalog[0].Codes[1].ValidFrom);
            Assert.Null(codeCatalog[0].Codes[1].ValidTo);
        }
    }
}
