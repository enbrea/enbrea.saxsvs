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
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;

namespace Enbrea.SaxSVS
{
    /// <summary>
    /// SaxSVS facility (Dienststelle)
    /// </summary>
    public class SaxSVSFacility
    {
        /// <summary>
        /// Branches (Außenstellen)
        /// </summary>
        public IList<SaxSVSFacility> Branches { get; set; } = [];

        /// <summary>
        /// Cooperations (Kooperierende Dienststellen)
        /// </summary>
        public IList<SaxSVSFacility> Cooperations { get; set; } = [];

        /// <summary>
        /// Key (ID 108-006: Dienststellenschlüssel)
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Name (ID 108-018: Name der Einrichtung)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Sorting name (ID 108-011: Name der Einrichtung - Sortname)
        /// </summary>
        public string SortingName { get; set; }

        /// <summary>
        /// Parses a <see cref="SaxSVSFacility"/> from the given XML stream and gives back a new instance
        /// </summary>
        /// <param name="xmlReader">The XML reader</param>
        /// <param name="parentElementName">Name of the current XML parent element</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult
        /// parameter contains a new <see cref="SaxSVSFacility"/> instance.
        /// <returns>
        public static async Task<SaxSVSFacility> FromXmlReader(XmlReader xmlReader, string parentElementName)
        {
            var facility = new SaxSVSFacility();

            await xmlReader.ReadAsync();

            while (!xmlReader.EOF)
            {
                if (xmlReader.NodeType == XmlNodeType.Element)
                {
                    if (xmlReader.Name == "wert")
                    {
                        var fieldId = xmlReader.GetAttribute("feld") ?? throw new FormatException("XML attribute \"field\" expected.");

                        switch (fieldId)
                        {
                            case "108-006":
                                facility.Key = await xmlReader.ReadElementContentAsStringAsync();
                                break;

                            case "108-018":
                                facility.Name = await xmlReader.ReadElementContentAsStringAsync();
                                break;

                            case "108-011":
                                facility.SortingName = await xmlReader.ReadElementContentAsStringAsync();
                                break;

                            default:
                                await xmlReader.ReadAsync();
                                break;
                        }
                    }
                    else if (xmlReader.Name == "kooperierend")
                    {
                        facility.Cooperations.Add(await FromXmlReader(xmlReader, xmlReader.Name));
                    }
                    else if (xmlReader.Name == "aussenstelle")
                    {
                        facility.Branches.Add(await FromXmlReader(xmlReader, xmlReader.Name));
                    }
                    else
                    {
                        await xmlReader.ReadAsync();
                    }
                }
                else if (xmlReader.NodeType == XmlNodeType.EndElement && xmlReader.Name == parentElementName)
                {
                    return facility;
                }
                else 
                {
                    await xmlReader.ReadAsync();
                }
            }

            throw new FormatException("Unexpected end of XML");
        }
    }
}
