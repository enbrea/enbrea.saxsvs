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
    /// SaxSVS promotion (Versetzung)
    /// </summary>
    public class SaxSVSPromotion
    {
        /// <summary>
        /// Academic year (ID 514-009: Schuljahr)
        /// </summary>
        public string AcademicYear { get; set; }

        /// <summary>
        /// Class name (ID 514-010: Name der Klasse / Kürzel)
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// Class name (ID 514-011: Schulart / Bildungsgang)
        /// </summary>
        public SaxSVSCodeRef SchoolType { get; set; }

        /// <summary>
        /// Class name (ID 514-013: Wertung des Jahres)
        /// </summary>
        public bool? Rating { get; set; }

        /// <summary>
        /// Class name (ID 514-015: Versetzungsvermerk 2 HJ)
        /// </summary>
        public SaxSVSCodeRef Flag { get; set; }

        /// <summary>
        /// Parses a <see cref="SaxSVSPromotion"/> from the given XML stream and gives back a new instance
        /// </summary>
        /// <param name="xmlReader">The XML reader</param>
        /// <param name="parentElementName">Name of the current XML parent element</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult
        /// parameter contains a new <see cref="SaxSVSPromotion"/> instance.
        /// <returns>
        public static async Task<SaxSVSPromotion> FromXmlReader(XmlReader xmlReader, string parentElementName)
        {
            var promotion = new SaxSVSPromotion();

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
                            return promotion;
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
                            case "514-009":
                                promotion.AcademicYear = await xmlReader.ReadElementContentAsStringAsync();
                                break;

                            case "514-010":
                                promotion.ClassName = await xmlReader.ReadElementContentAsStringAsync();
                                break;

                            case "514-011":
                                promotion.SchoolType = await SaxSVSCodeRef.FromXmlReader(xmlReader, xmlReader.Name);
                                break;

                            case "514-013":
                                promotion.Rating = ParseUtils.ParseBooleanOrDefault(await xmlReader.ReadElementContentAsStringAsync());
                                break;

                            case "514-015":
                                promotion.Flag = await SaxSVSCodeRef.FromXmlReader(xmlReader, xmlReader.Name);
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
                    return promotion;
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
