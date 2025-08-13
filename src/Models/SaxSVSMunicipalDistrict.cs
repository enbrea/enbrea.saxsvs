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

using System.Threading.Tasks;
using System.Xml;

namespace Enbrea.SaxSVS
{
    /// <summary>
    /// SaxSVS municipal district (Gemeindeteil)
    /// </summary>
    public class SaxSVSMunicipalDistrict
	{
        /// <summary>
        /// Key (Gemeindeteilschlüssel)
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Parses a <see cref="SaxSVSMunicipalDistrict"/> from the given XML stream and gives back a new instance
        /// </summary>
        /// <param name="xmlReader">The XML reader</param>
        /// <param name="parentElementName">Name of the current XML parent element</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult
        /// parameter contains a new <see cref="SaxSVSMunicipalDistrict"/> instance.
        /// <returns>
        public static async Task<SaxSVSMunicipalDistrict> FromXmlReader(XmlReader xmlReader, string parentElementName)
        {
            return new SaxSVSMunicipalDistrict
            {
                Key = xmlReader.GetAttribute("gemeindeschluessel"),
                Name = await xmlReader.ReadElementContentAsStringAsync()
            };
        }
    }
}
