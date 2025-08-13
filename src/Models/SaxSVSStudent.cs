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
    /// SaxSVS student (Schüler:in)
    /// </summary>
    public class SaxSVSStudent
    {
        /// <summary>
        /// Birth date (ID 500-017: Geburtsdatum)
        /// </summary>
        public DateOnly? BirthDate { get; set; }

        /// <summary>
        /// Birth name (ID 500-013: Geburtsname)
        /// </summary>
        public string BirthName { get; set; }

        /// <summary>
        /// Custodians (Liste der Sorgeberechtigten)
        /// </summary>
        public IList<SaxSVSCustodian> Custodians { get; set; } = [];

        /// <summary>
        /// Promotions (Liste der Versetzungen)
        /// </summary>
        public IList<SaxSVSPromotion> Promotions { get; set; } = [];

        /// <summary>
        /// Denomination (ID 500-021: Konfession/Religionszugehörigkeit)
        /// </summary>
        public string Denomination { get; set; }

        /// <summary>
        /// Family name (ID 500-009: Familienname bzw. Nachname)
        /// </summary>
        public string FamilyName { get; set; }

        /// <summary>
        /// Family name in native language (ID 500-011: Nachname in Muttersprache)
        /// </summary>
        public string FamilyNameInNativeLanguage { get; set; }

        /// <summary>
        /// Gender (ID 500-015: Geschlecht)
        /// </summary>
        public SaxSVSCodeRef Gender { get; set; }

        /// <summary>
        /// Given name (ID 500-003: Vorname)
        /// </summary>
        public string GivenName { get; set; }

        /// <summary>
        /// Given name in native language (ID 500-005: Vorname in Muttersprache)
        /// </summary>
        public string GivenNameInNativeLanguage { get; set; }

        /// <summary>
        /// Unique ID of the student
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Middle names (ID 500-006: weitere Vornamen)
        /// </summary>
        public string MiddleNames { get; set; }

        /// <summary>
        /// Name of the main school (ID 543-011: Name der Stammschule)
        /// </summary>
        public string NameOfMainSchool { get; set; }

        /// <summary>
        /// Name prefix (ID 500-007: Namensvorsatz)
        /// </summary>
        public string NamePrefix { get; set; }

        /// <summary>
        /// Nationality (ID 500-023: Staatsangehörigkeit)
        /// </summary>
        public SaxSVSCodeRef Nationality { get; set; }

        /// <summary>
        /// Native language (ID 500-027: Muttersprache)
        /// </summary>
        public SaxSVSCodeRef NativeLanguage { get; set; }

        /// <summary>
        /// Place of birth (ID 500-019: Geburtsort)
        /// </summary>
        public string PlaceOfBirth { get; set; }

        /// <summary>
        /// Present (ID 516-002: aktuell anwesend)
        /// </summary>
        public bool? Present { get; set; }

        /// <summary>
        /// Public transportation (ID 500-033: Fahrschüler)
        /// </summary>
        public bool? PublicTransportation { get; set; }

        /// <summary>
        /// Sorbian (ID 500-032: sorbisch)
        /// </summary>
        public bool? Sorbian { get; set; }

        /// <summary>
        /// Parses a <see cref="SaxSVSStudent"/> from the given XML stream and gives back a new instance
        /// </summary>
        /// <param name="xmlReader">The XML reader</param>
        /// <param name="parentElementName">Name of the current XML parent element</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult
        /// parameter contains a new <see cref="SaxSVSStudent"/> instance.
        /// <returns>
        public static async Task<SaxSVSStudent> FromXmlReader(XmlReader xmlReader, string parentElementName)
        {
            var student = new SaxSVSStudent
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
                            case "500-003":
                                student.GivenName = await xmlReader.ReadElementContentAsStringAsync();
                                break;

                            case "500-005":
                                student.GivenNameInNativeLanguage = await xmlReader.ReadElementContentAsStringAsync();
                                break;

                            case "500-006":
                                student.MiddleNames = await xmlReader.ReadElementContentAsStringAsync();
                                break;

                            case "500-007":
                                student.NamePrefix = await xmlReader.ReadElementContentAsStringAsync();
                                break;

                            case "500-009":
                                student.FamilyName = await xmlReader.ReadElementContentAsStringAsync();
                                break;

                            case "500-011":
                                student.FamilyNameInNativeLanguage = await xmlReader.ReadElementContentAsStringAsync();
                                break;

                            case "500-013":
                                student.BirthName = await xmlReader.ReadElementContentAsStringAsync();
                                break;

                            case "500-015":
                                student.Gender = await SaxSVSCodeRef.FromXmlReader(xmlReader, xmlReader.Name);
                                break;

                            case "500-017":
                                student.BirthDate = ParseUtils.ParseDateOnlyOrDefault(await xmlReader.ReadElementContentAsStringAsync());
                                break;

                            case "500-019":
                                student.PlaceOfBirth = await xmlReader.ReadElementContentAsStringAsync();
                                break;

                            case "500-021":
                                student.Denomination = await xmlReader.ReadElementContentAsStringAsync();
                                break;

                            case "500-023":
                                student.Nationality = await SaxSVSCodeRef.FromXmlReader(xmlReader, xmlReader.Name);
                                break;

                            case "500-027":
                                student.NativeLanguage = await SaxSVSCodeRef.FromXmlReader(xmlReader, xmlReader.Name);
                                break;

                            case "500-032":
                                student.Sorbian = ParseUtils.ParseBooleanOrDefault(await xmlReader.ReadElementContentAsStringAsync());
                                break;

                            case "500-033":
                                student.PublicTransportation = ParseUtils.ParseBooleanOrDefault(await xmlReader.ReadElementContentAsStringAsync());
                                break;

                            case "516-002":
                                student.Present = ParseUtils.ParseBooleanOrDefault(await xmlReader.ReadElementContentAsStringAsync());
                                break;

                            case "543-011":
                                student.NameOfMainSchool = await xmlReader.ReadElementContentAsStringAsync();
                                break;

                            // not yet implemented
                            case "500-060":
                            case "516-003":
                            case "516-004":
                                await xmlReader.ReadAsync();
                                break;

                            default:
                                await xmlReader.ReadAsync();
                                break;
                        }
                    }
                    else if (xmlReader.Name == "sorgeberechtigte")
                    {
                        await xmlReader.ReadAsync();

                        while (!xmlReader.EOF)
                        {
                            if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "sorgeberechtigter")
                            {
                                student.Custodians.Add(await SaxSVSCustodian.FromXmlReader(xmlReader, xmlReader.Name));
                            }
                            else if (xmlReader.NodeType == XmlNodeType.EndElement && xmlReader.Name == "sorgeberechtigte")
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
                    else if (xmlReader.Name == "zusammenstellung")
                    {
                        var name = xmlReader.GetAttribute("name") ?? throw new FormatException("XML attribute \"name\" expected.");

                        switch (name)
                        {
                            case "Versetzung":
                                
                                await xmlReader.ReadAsync();

                                while (!xmlReader.EOF)
                                {
                                    if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "gruppierung")
                                    {
                                        student.Promotions.Add(await SaxSVSPromotion.FromXmlReader(xmlReader, xmlReader.Name));
                                    }
                                    else if (xmlReader.NodeType == XmlNodeType.EndElement && xmlReader.Name == "zusammenstellung")
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

                            // not yet implemented
                            case "Laufbahn":
                            case "Fremdsprachen":
                            case "Schülerfunktionen":
                            case "Sportbefreiungen":
                            case "Prüfungsfächer":
                            case "Integration":
                            case "Wahlfächer":
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
                    return student;
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
