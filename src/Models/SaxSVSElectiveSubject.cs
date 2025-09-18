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
    /// SaxSVS elective subject (Wahfach)
    /// </summary>
    public class SaxSVSElectiveSubject
    {
        /// <summary>
        /// Academic year (Schuljahr)
        /// </summary>
        public string AcademicYear { get; set; }

        /// <summary>
        /// Field Id (ID 509-XXX: )
        /// </summary>
        public string FieldId { get; set; }

        /// <summary>
        /// Subject (ID 509-XXX: )
        /// </summary>
        public SaxSVSCodeRef Subject { get; set; }

        /// <summary>
        /// Parses a <see cref="SaxSVSElectiveSubject"/> from the given XML stream and gives back a new instance
        /// </summary>
        /// <param name="xmlReader">The XML reader</param>
        /// <param name="parentElementName">Name of the current XML parent element</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult
        /// parameter contains a new <see cref="SaxSVSElectiveSubject"/> instance.
        /// <returns>
        public async static Task<SaxSVSElectiveSubject> FromXmlReader(XmlReader xmlReader, string parentElementName)
        {
            var electiveSubject = new SaxSVSElectiveSubject
            {
                AcademicYear = xmlReader.GetAttribute("id")
            };

            while (!xmlReader.EOF)
            {
                await xmlReader.MoveToContentAsync();

                if (xmlReader.NodeType == XmlNodeType.Element)
                {
                    if (xmlReader.Name == parentElementName)
                    {
                        if (xmlReader.IsEmptyElement)
                        {
                            await xmlReader.ReadAsync();
                            return electiveSubject;
                        }
                        else
                        {
                            await xmlReader.ReadAsync();
                        }
                    }
                    else if (xmlReader.Name == "wert")
                    {
                        var fieldId = xmlReader.GetAttribute("feld") ?? throw new FormatException("XML attribute \"field\" expected.");

                        switch (fieldId)
                        {
                            case "514-010":
                            case "514-011":
                            case "514-012":
                            case "514-013":
                            case "514-101":
                            case "514-102":
                            case "514-103":
                            case "514-104":
                            case "514-200":
                                electiveSubject.FieldId = xmlReader.GetAttribute("feld");
                                electiveSubject.Subject = await SaxSVSCodeRef.FromXmlReader(xmlReader, xmlReader.Name);
                                break;

                            default:
                                await xmlReader.SkipAsync();
                                break;
                        }
                    }
                    else
                    {
                        await xmlReader.ReadAsync();
                    }
                }
                else if (xmlReader.NodeType == XmlNodeType.EndElement && xmlReader.Name == parentElementName)
                {
                    await xmlReader.ReadAsync();
                    return electiveSubject;
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