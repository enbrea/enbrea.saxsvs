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
    /// SaxSVS lesson (Unterricht)
	/// </summary>
	public class SaxSVSLesson
    {
        /// <summary>
        /// Area of curriculum (ID 750-030: Bereich_1)
        /// </summary>
        public SaxSVSCodeRef AreaOfCurriculum { get; set; }

        /// <summary>
        /// Classes (Liste der Klassen)
        /// </summary>
        public IList<SaxSVSClassRelation> Classes { get; set; } = [];

        /// <summary>
        /// Complete group (ID 750-024: Gesamte Klasse)
        /// </summary>
        public bool? CompleteGroup { get; set; }

        /// <summary>
        /// Group name (ID 750-060: Gruppenname)
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Unique ID of the lesson
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Internal long name (ID 750-092: intern lang)
        /// </summary>
        public string InternalLongName { get; set; }

        /// <summary>
        /// Internal short name (ID 750-090: intern)
        /// </summary>
        public string InternalShortName { get; set; }

        /// <summary>
        /// Multi level (ID 750-026: Mehrstufengruppe)
        /// </summary>
        public bool? MultiLevel { get; set; }

        /// <summary>
        /// Number of hours (ID 750-032: Stundenzahl_1)
        /// </summary>
        public double? NumberOfHours { get; set; }

        /// <summary>
        /// Official subject (ID 750-040: amtliches Bildungsangebot)
        /// </summary>
        public SaxSVSCodeRef OfficialSubject { get; set; }

        /// <summary>
        /// Remark (ID 750-093: Bemerkung)
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// Academic year (ID 700-003: Schuljahr)
        /// </summary>
        public string AcademicYear { get; set; }

        /// <summary>
        /// Students (Liste der Schüler)
        /// </summary>
        public IList<SaxSVSStudentAttendance> Students { get; set; } = [];

        /// <summary>
        /// School subject (ID 750-042: schulisches Bildungsangebot)
        /// </summary>
        public SaxSVSCodeRef Subject { get; set; }

        /// <summary>
        /// Supplementary area of curriculum (ID 750-034: Bereich_2 (Verstärkungslehrer))
        /// </summary>
        public SaxSVSCodeRef SupplementaryAreaOfCurriculum { get; set; }

        /// <summary>
        /// Supplementary number of hours (ID 750-036: Stundenzahl_2 (Verstärkungslehrer))
        /// </summary>
        public double? SupplementaryNumberOfHours { get; set; }

        /// <summary>
        /// Supplementary teacher (ID 700-022: (ID_Lehrer) - (2 - Verstärkungslehrer))
        /// </summary>
        public Guid? SupplementaryTeacherId { get; set; }

        /// <summary>
        /// Teacher (ID 700-020: (ID_Lehrer) - 1)
        /// </summary>
        public Guid? TeacherId { get; set; }

        /// <summary>
        /// Parses a <see cref="SaxSVSLesson"/> from the given XML stream and gives back a new instance
        /// </summary>
        /// <param name="xmlReader">The XML reader</param>
        /// <param name="parentElementName">Name of the current XML parent element</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult
        /// parameter contains a new <see cref="SaxSVSLesson"/> instance.
        /// <returns>
        public static async Task<SaxSVSLesson> FromXmlReader(XmlReader xmlReader, string parentElementName)
        {
            var saxSVSLesson = new SaxSVSLesson
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
                            return saxSVSLesson;
                        }
                        else
                        {
                            await xmlReader.ReadAsync();
                        }
                    }
                    else if (xmlReader.Name == "wert")
                    {
                        var fieldId = xmlReader.GetAttribute("feld") ?? throw new FormatException("XML attribute \"feld\" expected.");
                        
                        switch (fieldId)
                        {
                            case "700-020":
                                saxSVSLesson.TeacherId = Guid.Parse(xmlReader.GetAttribute("person-id"));
                                await xmlReader.ReadAsync();
                                break;

                            case "700-022":
                                saxSVSLesson.SupplementaryTeacherId = Guid.Parse(xmlReader.GetAttribute("person-id"));
                                await xmlReader.ReadAsync();
                                break;

                            case "700-050":
                                saxSVSLesson.Remark = await xmlReader.ReadElementContentAsStringAsync();
                                break;

                            case "750-003":
                                saxSVSLesson.AcademicYear = await xmlReader.ReadElementContentAsStringAsync();
                                break;

                            case "750-024":
                                saxSVSLesson.CompleteGroup = ParseUtils.ParseBooleanOrDefault(await xmlReader.ReadElementContentAsStringAsync());
                                break;

                            case "750-026":
                                saxSVSLesson.MultiLevel = ParseUtils.ParseBooleanOrDefault(await xmlReader.ReadElementContentAsStringAsync());
                                break;

                            case "750-030":
                                saxSVSLesson.AreaOfCurriculum = await SaxSVSCodeRef.FromXmlReader(xmlReader, xmlReader.Name);
                                break;

                            case "750-032":
                                saxSVSLesson.NumberOfHours = ParseUtils.ParseDoubleOrDefault(await xmlReader.ReadElementContentAsStringAsync());
                                break;

                            case "750-034":
                                saxSVSLesson.SupplementaryAreaOfCurriculum = await SaxSVSCodeRef.FromXmlReader(xmlReader, xmlReader.Name);
                                break;

                            case "750-036":
                                saxSVSLesson.SupplementaryNumberOfHours = ParseUtils.ParseDoubleOrDefault(await xmlReader.ReadElementContentAsStringAsync());
                                break;

                            case "750-040":
                                saxSVSLesson.OfficialSubject = await SaxSVSCodeRef.FromXmlReader(xmlReader, xmlReader.Name);
                                break;

                            case "750-042":
                                saxSVSLesson.Subject = await SaxSVSCodeRef.FromXmlReader(xmlReader, xmlReader.Name);
                                break;

                            case "750-060":
                                saxSVSLesson.GroupName = await xmlReader.ReadElementContentAsStringAsync();
                                break;

                            case "750-090":
                                saxSVSLesson.InternalShortName = await xmlReader.ReadElementContentAsStringAsync();
                                break;

                            case "750-092":
                                saxSVSLesson.InternalLongName = await xmlReader.ReadElementContentAsStringAsync();
                                break;

                            case "750-093":
                                saxSVSLesson.Remark = await xmlReader.ReadElementContentAsStringAsync();
                                break;

                            // not yet implemented
                            case "108-006":
                            case "108-018":
                            case "410-016":
                            case "750-020":
                            case "750-022":
                                await xmlReader.SkipAsync();
                                break;

                            default:
                                await xmlReader.SkipAsync();
                                break;
                        }
                    }
                    else if (xmlReader.Name == "klassenzuordnungen")
                    {
                        while (!xmlReader.EOF)
                        {
                            await xmlReader.MoveToContentAsync();

                            if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "klassenzuordnung")
                            {
                                saxSVSLesson.Classes.Add(await SaxSVSClassRelation.FromXmlReader(xmlReader, xmlReader.Name));
                            }
                            else if (xmlReader.NodeType == XmlNodeType.EndElement && xmlReader.Name == "klassenzuordnungen")
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
                    else if (xmlReader.Name == "schuelerzuordnungen")
                    {
                        await xmlReader.ReadAsync();

                        while (!xmlReader.EOF)
                        {
                            await xmlReader.MoveToContentAsync();

                            if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "schuelerzuordnung")
                            {
                                saxSVSLesson.Students.Add(await SaxSVSStudentAttendance.FromXmlReader(xmlReader, xmlReader.Name));
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
                    return saxSVSLesson;
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
