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
    /// SaxSVS course (Kurs)
    /// </summary>
    public class SaxSVSCourse
    {
        /// <summary>
        /// Unique ID of the course
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Remark (ID 420-050: Bemerkung)
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// Short name (ID 420-010: Kursbezeichung/Kürzel)
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// Students (Liste der Schüler)
        /// </summary>
        public IList<SaxSVSStudentAttendance> Students { get; set; } = [];

        /// <summary>
        /// Teacher (ID 420-025: (ID-Lehrkraft))
        /// </summary>
        public Guid? TeacherId { get; set; }

        /// <summary>
        /// Parses a <see cref="SaxSVSCourse"/> from the given XML stream and gives back a new instance
        /// </summary>
        /// <param name="xmlReader">The XML reader</param>
        /// <param name="parentElementName">Name of the current XML parent element</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult
        /// parameter contains a new <see cref="SaxSVSCourse"/> instance.
        /// <returns>
        public static async Task<SaxSVSCourse> FromXmlReader(XmlReader xmlReader, string parentElementName)
        {
            var course = new SaxSVSCourse
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
                            return course;
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
                            case "420-010":
                                course.ShortName = await xmlReader.ReadElementContentAsStringAsync();
                                break;

                            case "420-025":
                                course.TeacherId = Guid.Parse(xmlReader.GetAttribute("person-id"));
                                break;

                            case "420-050":
                                course.Remark = await xmlReader.ReadElementContentAsStringAsync();
                                break;

                            // not yet implemented
                            case "108-006":
                            case "108-018":
                            case "420-003":
                            case "420-004":
                            case "420-014":
                            case "420-020":
                            case "420-030":
                            case "503-010":
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
                                course.Students.Add(await SaxSVSStudentAttendance.FromXmlReader(xmlReader, xmlReader.Name));
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
                        await xmlReader.ReadAsync();
                    }
                }
                else if (xmlReader.NodeType == XmlNodeType.EndElement && xmlReader.Name == parentElementName)
                {
                    await xmlReader.ReadAsync();
                    return course;
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
