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
using System.Threading.Tasks;
using System.Xml;

namespace Enbrea.SaxSVS
{
    /// <summary>
    /// SaxSVS student attendacne (Schüler:innenteilnahme für Klassen oder Unterricht)
    /// </summary>
    public class SaxSVSStudentAttendance
    {
        /// <summary>
        /// Student ID
        /// </summary>
        public Guid StudentId { get; set; }

        /// <summary>
        /// Group type (ID 400-011: Klassentyp)
        /// </summary>
        public DateTime? ValidFrom { get; set; }


        /// <summary>
        /// Group type (ID 400-011: Klassentyp)
        /// </summary>
        public DateTime? ValidTo { get; set; }

        /// <summary>
        /// Parses a <see cref="SaxSVSStudentAttendance"/> from the given XML stream and gives back a new instance
        /// </summary>
        /// <param name="xmlReader">The XML reader</param>
        /// <param name="parentElementName">Name of the current XML parent element</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult
        /// parameter contains a new <see cref="SaxSVSStudentAttendance"/> instance.
        /// <returns>
        public async static Task<SaxSVSStudentAttendance> FromXmlReader(XmlReader xmlReader, string parentElementName)
        {
            var attendance = new SaxSVSStudentAttendance
            {
                StudentId = Guid.Parse(xmlReader.GetAttribute("id")),
                ValidFrom = ParseUtils.ParseDateTimeOrDefault(xmlReader.GetAttribute("von")),
                ValidTo = ParseUtils.ParseDateTimeOrDefault(xmlReader.GetAttribute("bis"))
            };
            
            await xmlReader.ReadAsync();
            
            return attendance;
        }
    }
}