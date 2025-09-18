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
    public class SaxSVSCodeListTests
    {
        [Fact]
        public async Task Test_GenderCodes()
        {
            var restClient = RestClientFactory.CreateRestClient();
            using var stream = await restClient.GetStreamAsync(new Uri("https://web1.extranet.sachsen.de/svsp/public/schnittstellen/K101.xml"), MediaTypeNames.Application.Xml);

            using var strReader = new StreamReader(stream, Encoding.GetEncoding("ISO-8859-1"));

            var saxSVSReader = new SaxSVSReader(strReader);
            var codeList = await saxSVSReader.ReadCodeListAsync();

            Assert.Equal(4, codeList.Count);
            Assert.Equal("männl.", codeList[0].ShortName);
            Assert.Equal("männlich", codeList[0].LongName);
            Assert.Equal("1", codeList[0].Code);
            Assert.Equal(new DateTime(2005, 4, 27, 14, 34, 46), codeList[0].ValidFrom);
            Assert.Null(codeList[0].ValidTo);
            Assert.Equal(21, codeList[0].ValidSchoolTypes.Count);
            Assert.Equal("Gymnasium", codeList[0].ValidSchoolTypes[2]);
            Assert.Equal("weibl.", codeList[2].ShortName);
            Assert.Equal("weiblich", codeList[2].LongName);
            Assert.Equal("2", codeList[2].Code);
            Assert.Equal(new DateTime(2005, 4, 27, 14, 34, 46), codeList[2].ValidFrom);
            Assert.Null(codeList[2].ValidTo);
            Assert.Equal(21, codeList[0].ValidSchoolTypes.Count);
            Assert.Equal("Berufliches Gymnasium", codeList[0].ValidSchoolTypes[20]);
        }
    }
}
