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
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace Enbrea.SaxSVS
{
    /// <summary>
    /// SaxSVS export document 
    /// </summary>
    public class SaxSVSDocument
    {
        /// <summary>
        /// Academic year (Schuljahr)
        /// </summary>
        public string AcademicYear { get; set; }

        /// <summary>
        /// List of classes
        /// </summary>
        public IList<SaxSVSClass> Classes { get; set; } = [];

        /// <summary>
        /// List of courses
        /// </summary>
        public IList<SaxSVSCourse> Courses { get; set; } = [];

        /// <summary>
        /// Facility (Dienststelle)
        /// </summary>
        public SaxSVSFacility Facility { get; set; }

        /// <summary>
        /// Facility key (Dienststellenschlüssel)
        /// </summary>
        public string FacilityKey { get; set; }

        /// <summary>
        /// List of lessons
        /// </summary>
        public IList<SaxSVSLesson> Lessons { get; set; } = [];

        /// <summary>
        /// List of students
        /// </summary>
        public IList<SaxSVSStudent> Students { get; set; } = [];

        /// <summary>
        /// TimeStamp
        /// </summary>
        public DateTime? TimeStamp { get; set; }

        /// <summary>
        /// List of workforces (e.g. teachers)
        /// </summary>
        public IList<SaxSVSWorkforce> Workforces { get; set; } = [];

        /// <summary>
        /// Parses a <see cref="SaxSVSDocument"/> from the given XML stream
        /// </summary>
        /// <param name="xmlReader">The XML reader</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult
        /// parameter contains a new <see cref="SaxSVSDocument"/> instance.
        /// <returns>
        public static async Task<SaxSVSDocument> FromXmlReader(XmlReader xmlReader, CancellationToken cancellationToken = default)
        {
            var document = new SaxSVSDocument();

            await xmlReader.ReadAsync();

            while (!xmlReader.EOF)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (xmlReader.NodeType == XmlNodeType.Element)
                {
                    if (xmlReader.Name == "saxsvs")
                    {
                        document.TimeStamp = ParseUtils.ParseDateTimeOrDefault(xmlReader.GetAttribute("datum"));
                        document.FacilityKey = xmlReader.GetAttribute("dienststelle");
                        document.AcademicYear = xmlReader.GetAttribute("schuljahr");
                        
                        await xmlReader.ReadAsync();
                    }
                    else if (xmlReader.Name == "dienststelle")
                    {
                        document.Facility = await SaxSVSFacility.FromXmlReader(xmlReader, xmlReader.Name);
                    }
                    else if (xmlReader.Name == "personal")
                    {
                        await xmlReader.ReadAsync();

                        while (!xmlReader.EOF)
                        {
                            cancellationToken.ThrowIfCancellationRequested();
                            
                            if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "person")
                            {
                                document.Workforces.Add(await SaxSVSWorkforce.FromXmlReader(xmlReader, xmlReader.Name));
                            }
                            else if (xmlReader.NodeType == XmlNodeType.EndElement && xmlReader.Name == "personal")
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
                    else if (xmlReader.Name == "schuelerindex")
                    {
                        await xmlReader.ReadAsync();

                        while (!xmlReader.EOF)
                        {
                            cancellationToken.ThrowIfCancellationRequested();

                            if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "schueler")
                            {
                                document.Students.Add(await SaxSVSStudent.FromXmlReader(xmlReader, xmlReader.Name));
                            }
                            else if (xmlReader.NodeType == XmlNodeType.EndElement && xmlReader.Name == "schuelerindex")
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
                    else if (xmlReader.Name == "klassen")
                    {
                        await xmlReader.ReadAsync();

                        while (!xmlReader.EOF)
                        {
                            cancellationToken.ThrowIfCancellationRequested();

                            if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "klasse")
                            {
                                document.Classes.Add(await SaxSVSClass.FromXmlReader(xmlReader, xmlReader.Name));
                            }
                            else if (xmlReader.NodeType == XmlNodeType.EndElement && xmlReader.Name == "klassen")
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
                    else if (xmlReader.Name == "unterrichtungen")
                    {
                        await xmlReader.ReadAsync();

                        while (!xmlReader.EOF)
                        {
                            cancellationToken.ThrowIfCancellationRequested();

                            if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "unterricht")
                            {
                                document.Lessons.Add(await SaxSVSLesson.FromXmlReader(xmlReader, xmlReader.Name));
                            }
                            else if (xmlReader.NodeType == XmlNodeType.EndElement && xmlReader.Name == "unterrichtungen")
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
                    else if (xmlReader.Name == "kurse")
                    {
                        await xmlReader.ReadAsync();

                        while (!xmlReader.EOF)
                        {
                            cancellationToken.ThrowIfCancellationRequested();

                            if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "kurs")
                            {
                                document.Courses.Add(await SaxSVSCourse.FromXmlReader(xmlReader, xmlReader.Name));
                            }
                            else if (xmlReader.NodeType == XmlNodeType.EndElement && xmlReader.Name == "kurse")
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
                else if (xmlReader.NodeType == XmlNodeType.EndElement && xmlReader.Name == "saxsvs")
                {
                    return document;
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