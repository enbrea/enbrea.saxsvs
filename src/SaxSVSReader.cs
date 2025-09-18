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

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace Enbrea.SaxSVS
{
    /// <summary>
    /// An object representing the content of an official import/export file of the SaxSVS BBS interface
    /// </summary>xs
    public class SaxSVSReader
    {
        private readonly TextReader _textReader;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaxSVSReader"/> class.
        /// </summary>
        public SaxSVSReader(TextReader textReader)
        {
            _textReader = textReader;
        }

        /// <summary>
        /// Parses <see cref="SaxSVSCodeList"/> instances from the given XML stream
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult
        /// parameter contains a new list of <see cref="SaxSVSCodeList"/> instance.
        /// <returns>
        /// <remark>
        /// This parser is compatible with the catalog published under:
        /// https://web1.extranet.sachsen.de/bbsp/public/schnittstellen/Schluessel.xml
        /// </remark>
        public async Task<IList<SaxSVSCodeList>> ReadCodeCatalogAsync(CancellationToken cancellationToken = default)
        {
            var codeCatalog = new List<SaxSVSCodeList>();

            using (var xmlReader = XmlReader.Create(_textReader, new XmlReaderSettings { IgnoreWhitespace = true, Async = true }))
            {
                while (!xmlReader.EOF)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    if (xmlReader.NodeType == XmlNodeType.Element)
                    {
                        if (xmlReader.Name == "schluessel")
                        {
                            codeCatalog.Add(await SaxSVSCodeList.FromXmlReader(xmlReader, xmlReader.Name));
                        }
                        else 
                        {
                            await xmlReader.ReadAsync();
                        }
                    }
                    else
                    {
                        await xmlReader.ReadAsync();
                    }
                }
            }

            return codeCatalog;
        }

        /// <summary>
        /// Parses <see cref="SaxSVSCode"/> instances from the given XML stream
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult
        /// parameter contains a new list of <see cref="SaxSVSCode"/> instance.
        /// <returns>
        /// <remark>
        /// This parser is compatible with all standard code lists published under:
        /// https://web1.extranet.sachsen.de/svsp/public/schnittstellen/index.xml
        /// </remark>
        public async Task<IList<SaxSVSCode>> ReadCodeListAsync(CancellationToken cancellationToken = default)
        {
            var codeList = new List<SaxSVSCode>();

            using (var xmlReader = XmlReader.Create(_textReader, new XmlReaderSettings { IgnoreWhitespace = true, Async = true }))
            {
                while (!xmlReader.EOF)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    if (xmlReader.NodeType == XmlNodeType.Element)
                    {
                        if (xmlReader.Name == "element")
                        {
                            codeList.Add(await SaxSVSCode.FromXmlReader(xmlReader, xmlReader.Name));
                        }
                        else 
                        {
                            await xmlReader.ReadAsync();
                        }
                    }
                    else
                    {
                        await xmlReader.ReadAsync();
                    }
                }
            }

            return codeList;
        }

        /// <summary>
        /// Parses a <see cref="SaxSVSDocument"/> instance from the given XML stream
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult
        /// parameter contains a new  <see cref="SaxSVSDocument"/> instance.
        /// <returns>
        public async Task<SaxSVSDocument> ReadDocumentAsync(CancellationToken cancellationToken = default)
        {
            using var xmlReader = XmlReader.Create(_textReader, new XmlReaderSettings { IgnoreWhitespace = true, Async = true });
            
            return await SaxSVSDocument.FromXmlReader(xmlReader, cancellationToken);
        }
    }
}
