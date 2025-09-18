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
    /// SaxSVS class (Klasse)
    /// </summary>
    public class SaxSVSClass
    {
        /// <summary>
        /// Academic year (ID 400-003: Schuljahr)
        /// </summary>
        public string AcademicYear { get; set; }

        /// <summary>
        /// Class level (ID 400-013: Klassenstufe)
        /// </summary>
        public SaxSVSCodeRef ClassLevel { get; set; }

        /// <summary>
        /// Class type (ID 400-011: Klassentyp)
        /// </summary>
        public SaxSVSCodeRef ClassType { get; set; }

        /// <summary>
        /// Deputy form teacher (ID 400-016: Stellvertreter Klassenlehrer / Klassenlehrer 2)
        /// </summary>
        public Guid? DeputyFormTeacherId { get; set; }

        /// <summary>
        /// Form teacher (ID 400-015: Klassenlehrer)
        /// </summary>
        public Guid? FormTeacherId { get; set; }

        /// <summary>
        /// Unique ID of the class
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Name (ID 400-012: Klassenbezeichung / Kürzel)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Student attendances (Schuelerzuordnungen)
        /// </summary>
        public IList<SaxSVSStudentAttendance> Students { get; set; } = [];

        /// <summary>
        /// Parses a <see cref="SaxSVSClass"/> from the given XML stream and gives back a new instance
        /// </summary>
        /// <param name="xmlReader">The XML reader</param>
        /// <param name="parentElementName">Name of the current XML parent element</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult
        /// parameter contains a new <see cref="SaxSVSClass"/> instance.
        /// <returns>
        public static async Task<SaxSVSClass> FromXmlReader(XmlReader xmlReader, string parentElementName)
        {
            var schoolClass = new SaxSVSClass
            {
                Id = Guid.Parse(xmlReader.GetAttribute("id"))
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
                            return schoolClass;
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
                            case "400-003":
                                schoolClass.AcademicYear = await xmlReader.ReadElementContentAsStringAsync();
                                break;

                            case "400-011":
                                schoolClass.ClassType = await SaxSVSCodeRef.FromXmlReader(xmlReader, xmlReader.Name);
                                break;

                            case "400-012":
                                schoolClass.Name = await xmlReader.ReadElementContentAsStringAsync();
                                break;

                            case "400-013":
                                schoolClass.ClassLevel = await SaxSVSCodeRef.FromXmlReader(xmlReader, xmlReader.Name);
                                break;

                            case "400-015":
                                schoolClass.FormTeacherId = Guid.Parse(xmlReader.GetAttribute("person-id"));
                                await xmlReader.ReadAsync();
                                break;

                            case "400-016":
                                schoolClass.DeputyFormTeacherId = Guid.Parse(xmlReader.GetAttribute("person-id"));
                                await xmlReader.ReadAsync();
                                break;

                            // not yet implemented
                            case "108-006":
                            case "108-018":
                            case "400-002":
                                await xmlReader.SkipAsync();
                                break;

                            default:
                                await xmlReader.SkipAsync();
                                break;
                        }
                    }
                    else if (xmlReader.Name == "schuelerzuordnungen")
                    {
                        while (!xmlReader.EOF)
                        {
                            await xmlReader.MoveToContentAsync();

                            if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "schuelerzuordnung")
                            {
                                schoolClass.Students.Add(await SaxSVSStudentAttendance.FromXmlReader(xmlReader, xmlReader.Name));
                            }
                            else if (xmlReader.NodeType == XmlNodeType.EndElement && xmlReader.Name == "schuelerzuordnungen")
                            {
                                await xmlReader.ReadAsync();
                                break;
                            }
                            else
                            {
                                await xmlReader.ReadAsync();
                            }
                        }
                    }
                    else
                    {
                        await xmlReader.SkipAsync();
                    }
                }
                else if (xmlReader.NodeType == XmlNodeType.EndElement && xmlReader.Name == parentElementName)
                {
                    await xmlReader.ReadAsync();
                    return schoolClass;
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
