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
    /// SaxSVS custodian (Sorgeberechtigte:r)
    /// </summary>
    public class SaxSVSCustodian
    {
        /// <summary>
        /// Active contact (ID 520-010: aktuell)
        /// </summary>
        public bool? ActiveContact { get; set; }

        /// <summary>
        /// Country (ID 521-026: Staat)
        /// </summary>
        public SaxSVSCodeRef Country { get; set; }

        /// <summary>
        /// E-mail (ID 522-030: E-Mail)
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Family name (ID 520-014: Nachname)
        /// </summary>
        public string FamilyName { get; set; }

        /// <summary>
        /// Fax (ID 522-029: Fax)
        /// </summary>
        public string Fax { get; set; }

        /// <summary>
        /// Federal state (ID 521-031: Bundesland)
        /// </summary>
        public string FederalState { get; set; }

        /// <summary>
        /// Given name (ID 520-013: Vorname)
        /// </summary>
        public string GivenName { get; set; }

        /// <summary>
        /// House number (ID 521-022: Hausnummer)
        /// </summary>
        public string HouseNumber { get; set; }

        /// <summary>
        /// Location (ID 521-024: Ort)
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Mobile (ID 522-028: Handy)
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// Municipal district (ID 521-025: Ortsteil)
        /// </summary>
        public SaxSVSMunicipalDistrict MunicipalDistrict { get; set; }

        /// <summary>
        /// Municipality (ID 521-024: Ort bzw. Gemeinde)
        /// </summary>
        public SaxSVSMunicipality Municipality { get; set; }

        /// <summary>
        /// Must be contacted (ID 520-009: anschreiben / personensorgeberechtigt)
        /// </summary>
        public bool? MustBeContacted { get; set; }
        
        /// <summary>
        /// Phone (ID 522-027: Telefon)
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Postal code (ID 521-023: Postleitzahl)
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// Relationship (ID 520-011: Beziehung)
        /// </summary>
        public SaxSVSCodeRef Relationship { get; set; }

        /// <summary>
        /// Salutation (ID 520-012: Anrede)
        /// </summary>
        public SaxSVSCodeRef Salutation { get; set; }

        /// <summary>
        /// Street (ID 521-021: Straße)
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// Suburb (ID 521-025: Ortsteil)
        /// </summary>
        public string Suburb { get; set; }

        /// <summary>
        /// Work e-mail (ID 522-042: E-Mail dienstlich)
        /// </summary>
        public string WorkEmail { get; set; }

        /// <summary>
        /// Work mobile (ID 522-041: Handy dienstlich)
        /// </summary>
        public string WorkMobile { get; set; }

        /// <summary>
        /// Work phone (ID 522-040: Telefon dienstlich)
        /// </summary>
        public string WorkPhone { get; set; }

        /// <summary>
        /// Parses a <see cref="SaxSVSCustodian"/> from the given XML stream and gives back a new instance
        /// </summary>
        /// <param name="xmlReader">The XML reader</param>
        /// <param name="parentElementName">Name of the current XML parent element</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult
        /// parameter contains a new <see cref="SaxSVSCustodian"/> instance.
        /// <returns>
        public static async Task<SaxSVSCustodian> FromXmlReader(XmlReader xmlReader, string parentElementName)
        {
            var custodian = new SaxSVSCustodian();

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
                            case "520-009":
                                custodian.MustBeContacted = ParseUtils.ParseBooleanOrDefault(await xmlReader.ReadElementContentAsStringAsync());
                                break;

                            case "520-010":
                                custodian.ActiveContact = ParseUtils.ParseBooleanOrDefault(await xmlReader.ReadElementContentAsStringAsync());
                                break;

                            case "520-011":
                                custodian.Relationship = await SaxSVSCodeRef.FromXmlReader(xmlReader, xmlReader.Name);
                                break;

                            case "520-012":
                                custodian.Salutation = await SaxSVSCodeRef.FromXmlReader(xmlReader, xmlReader.Name);
                                break;

                            case "520-013":
                                custodian.GivenName = await xmlReader.ReadElementContentAsStringAsync();
                                break;

                            case "520-014":
                                custodian.FamilyName = await xmlReader.ReadElementContentAsStringAsync();
                                break;

                            case "521-020":
                                custodian.MunicipalDistrict = await SaxSVSMunicipalDistrict.FromXmlReader(xmlReader, xmlReader.Name);
                                break;

                            case "521-021":
                                custodian.Street = await xmlReader.ReadElementContentAsStringAsync();
                                break;

                            case "521-022":
                                custodian.HouseNumber = await xmlReader.ReadElementContentAsStringAsync();
                                break;

                            case "521-023":
                                custodian.PostalCode = await xmlReader.ReadElementContentAsStringAsync();
                                break;

                            case "521-024":
                                custodian.Municipality = await SaxSVSMunicipality.FromXmlReader(xmlReader, xmlReader.Name);
                                break;

                            case "521-026":
                                custodian.Country = await SaxSVSCodeRef.FromXmlReader(xmlReader, xmlReader.Name);
                                break;

                            case "522-027":
                                custodian.Phone = await xmlReader.ReadElementContentAsStringAsync();
                                break;

                            case "522-028":
                                custodian.Mobile = await xmlReader.ReadElementContentAsStringAsync();
                                break;

                            case "522-029":
                                custodian.Fax = await xmlReader.ReadElementContentAsStringAsync();
                                break;

                            case "522-030":
                                custodian.Email = await xmlReader.ReadElementContentAsStringAsync();
                                break;

                            case "522-040":
                                custodian.WorkPhone = await xmlReader.ReadElementContentAsStringAsync();
                                break;

                            case "522-041":
                                custodian.WorkMobile = await xmlReader.ReadElementContentAsStringAsync();
                                break;

                            case "522-042":
                                custodian.WorkEmail = await xmlReader.ReadElementContentAsStringAsync();
                                break;

                            // not yet implemented
                            case "520-015":
                            case "521-025":
                            case "530-010":
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
                    return custodian;
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
