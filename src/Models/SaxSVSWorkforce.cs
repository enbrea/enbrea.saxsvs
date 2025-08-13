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
    /// SaxSVS workforce (Personal)
    /// </summary>
    public class SaxSVSWorkforce
    {
        /// <summary>
        /// Academic degree (ID 200-011: Akademischer Grad)
        /// </summary>
        public string AcademicDegree { get; set; }

        /// <summary>
        /// Birth date (ID 200-017: Geburtsdatum)
        /// </summary>
        public DateOnly? BirthDate { get; set; }

        /// <summary>
        /// Birth name (ID 200-015: Geburtsname)
        /// </summary>
        public string BirthName { get; set; }

        /// <summary>
        /// Family name (ID 200-014: Familienname bzw. Nachname)
        /// </summary>
        public string FamilyName { get; set; }

        /// <summary>
        /// Gender (ID 200-010: Geschlecht)
        /// </summary>
        public SaxSVSCodeRef Gender { get; set; }

        /// <summary>
        /// Given name (ID 200-016: Vorname)
        /// </summary>
        public string GivenName { get; set; }

        /// <summary>
        /// Unique ID of the employee
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Name suffix (ID 200-012: Namenszusatz)
        /// </summary>
        public string NameSuffix { get; set; }

        /// <summary>
        /// No lessons (ID 200-070: ohne Unterrichtseinsatz)
        /// </summary>
        public bool? NoLessons { get; set; }

        /// <summary>
        /// Short name (ID 200-009: Kürzel)
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// Status (ID 200-008: Status an der Schule)
        /// </summary>
        public SaxSVSCodeRef Status { get; set; }

        /// <summary>
        /// Parses a <see cref="SaxSVSWorkforce"/> from the given XML stream and gives back a new instance
        /// </summary>
        /// <param name="xmlReader">The XML reader</param>
        /// <param name="parentElementName">Name of the current XML parent element</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult
        /// parameter contains a new <see cref="SaxSVSWorkforce"/> instance.
        /// <returns>
        public static async Task<SaxSVSWorkforce> FromXmlReader(XmlReader xmlReader, string parentElementName)
        {
            var employee = new SaxSVSWorkforce()
            {
                Id = Guid.Parse(xmlReader.GetAttribute("id"))
            };

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
                            case "200-008":
                                employee.Status = await SaxSVSCodeRef.FromXmlReader(xmlReader, xmlReader.Name);
                                break;

                            case "200-009":
                                employee.ShortName = await xmlReader.ReadElementContentAsStringAsync();
                                break;

                            case "200-010":
                                employee.Gender = await SaxSVSCodeRef.FromXmlReader(xmlReader, xmlReader.Name);
                                break;

                            case "200-011":
                                employee.AcademicDegree = await xmlReader.ReadElementContentAsStringAsync();
                                break;

                            case "200-012":
                                employee.NameSuffix = await xmlReader.ReadElementContentAsStringAsync();
                                break;

                            case "200-014":
                                employee.FamilyName = await xmlReader.ReadElementContentAsStringAsync();
                                break;

                            case "200-015":
                                employee.BirthName = await xmlReader.ReadElementContentAsStringAsync();
                                break;

                            case "200-016":
                                employee.GivenName = await xmlReader.ReadElementContentAsStringAsync();
                                break;

                            case "200-017":
                                employee.BirthDate = ParseUtils.ParseDateOnlyOrDefault(await xmlReader.ReadElementContentAsStringAsync());
                                break;

                            default:
                                await xmlReader.ReadAsync();
                                break;
                        }
                    }
                    else if (xmlReader.Name == "zusammenstellung")
                    {
                        var name = xmlReader.GetAttribute("name") ?? throw new FormatException("XML attribute \"name\" expected.");

                        switch (name)
                        {
                            // not yet implemented
                            case "Beschäftigung / Beurlaubung laut LPDK":
                            case "Auswahl Einsatzfächer":
                            case "Anrechnungen, Ermäßigungen, Freistellungen, Minderungen und Funktionen":
                            case "Abordnungen":
                                await xmlReader.ReadAsync();
                                break;

                            default:
                                await xmlReader.ReadAsync();
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
                    return employee;
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
