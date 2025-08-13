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
    /// SaxSVS code (Schlüsselwert)
    /// </summary>
    public class SaxSVSCode
	{
        /// <summary>
        /// Code (extern)
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Long name (lang)
        /// </summary>
        public string LongName { get; set; }

        /// <summary>
        /// Short name (kurz)
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// Sorting name (sort)
        /// </summary>
        public string SortingName { get; set; }

        /// <summary>
        /// Valid from (gueltig von)
        /// </summary>
        public DateTime? ValidFrom { get; set; }

        /// <summary>
        /// Valid school types (Schularten)
        /// </summary>
        public IList<string> ValidSchoolTypes { get; set; } = [];

        /// <summary>
        /// Valid to (gueltig bis)
        /// </summary>
        public DateTime? ValidTo { get; set; }

        /// <summary>
        /// Parses a <see cref="SaxSVSCode"/> from the given XML stream and gives back a new instance
        /// </summary>
        /// <param name="xmlReader">The XML reader</param>
        /// <param name="parentElementName">Name of the current XML parent element</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult
        /// parameter contains a new <see cref="SaxSVSCode"/> instance.
        /// <returns>
        public static async Task<SaxSVSCode> FromXmlReader(XmlReader xmlReader, string parentElementName)
        {
            var code = new SaxSVSCode()
            {
                Code = xmlReader.GetAttribute("extern-id")
            };

            await xmlReader.ReadAsync();

            while (!xmlReader.EOF)
            {
                if (xmlReader.NodeType == XmlNodeType.Element)
                {
                    switch (xmlReader.Name)
                    {
                        case "extern":
                            code.Code = await xmlReader.ReadElementContentAsStringAsync();
                            break;

                        case "kurz":
                            code.ShortName = await xmlReader.ReadElementContentAsStringAsync();
                            break;

                        case "lang":
                            code.LongName = await xmlReader.ReadElementContentAsStringAsync();
                            break;

                        case "sort":
                            code.SortingName = await xmlReader.ReadElementContentAsStringAsync();
                            break;

                        case "gueltig":
                            code.ValidFrom = ParseUtils.ParseDateTimeOrDefault(xmlReader.GetAttribute("von"));
                            code.ValidTo = ParseUtils.ParseDateTimeOrDefault(xmlReader.GetAttribute("bis"));
                            await xmlReader.ReadAsync();
                            break;

                        case "schularten" when !xmlReader.IsEmptyElement:
                            await xmlReader.ReadAsync();

                            while (!xmlReader.EOF)
                            {
                                if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "schulart")
                                {
                                    code.ValidSchoolTypes.Add(await xmlReader.ReadElementContentAsStringAsync());
                                }
                                else if (xmlReader.NodeType == XmlNodeType.EndElement && xmlReader.Name == "schularten")
                                {
                                    await xmlReader.ReadAsync();
                                    break;
                                }
                                else
                                {
                                    await xmlReader.ReadAsync();
                                }
                            }

                            break;

                        default:
                            await xmlReader.ReadAsync();
                            break;
                    }
                }
                else if (xmlReader.NodeType == XmlNodeType.EndElement && xmlReader.Name == parentElementName)
                {
                    return code;
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
