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
using System.Linq;

namespace Enbrea.SaxSVS
{
    /// <summary>
    /// Extensions for <see cref="string"/>
    /// </summary>
    internal static class StringExtensions
    {
        /// <summary>
        /// Returns a value indicating whether the <see cref="string"/> instance is part of a given list of strings.
        /// </summary>
        /// <param name="value">The <see cref="string"/> instance</param>
        /// <param name="strings">List of strings</param>
        /// <returns>true if list of strings contains the <see cref="string"/> instance; otherwise, false.</returns>
        public static bool AnyOf(this string value, IEnumerable<string> strings)
        {
            return strings.Contains(value);
        }

        /// <summary>
        /// Returns a value indicating whether the <see cref="string"/> instance is part of a given list of strings.
        /// </summary>
        /// <param name="value">The <see cref="string"/> instance</param>
        /// <param name="comparer">An equality comparer to compare strings</param>
        /// <param name="strings">List of strings</param>
        /// <returns>true if list of strings contains the <see cref="string"/> instance; otherwise, false.</returns>
        public static bool AnyOf(this string value, IEqualityComparer<string> comparer, IEnumerable<string> strings)
        {
            return strings.Contains(value, comparer);
        }

        /// <summary>
        /// Returns a value indicating whether the <see cref="string"/> instance is part of a given list of strings.
        /// </summary>
        /// <param name="value">The <see cref="string"/> instance</param>
        /// <param name="strings">List of strings</param>
        /// <returns>true if list of strings contains the <see cref="string"/> instance; otherwise, false.</returns>
        public static bool AnyOf(this string value, params string[] strings)
        {
            return value.AnyOf((IEnumerable<string>)strings);
        }

        /// <summary>
        /// Returns a value indicating whether the <see cref="string"/> instance is part of a given list of strings.
        /// </summary>
        /// <param name="value">The <see cref="string"/> instance</param>
        /// <param name="comparer">An equality comparer to compare strings</param>
        /// <param name="strings">List of strings</param>
        /// <returns>true if list of strings contains the <see cref="string"/> instance; otherwise, false.</returns>
        public static bool AnyOf(this string value, IEqualityComparer<string> comparer, params string[] strings)
        {
            return value.AnyOf(comparer, (IEnumerable<string>)strings);
        }

        /// <summary>
        /// Converts this string instance to a bool value. The mapping is defined by params.
        /// </summary>
        /// <param name="value">The <see cref="string"/> instance</param>
        /// <param name="trueStrings">List of string representations for true</param>
        /// <param name="falseStrings">List of string representations for false</param>
        /// <returns>A bool value</returns>
        public static bool ToBoolean(this string value, IEnumerable<string> trueStrings, IEnumerable<string> falseStrings)
        {
            if (value.AnyOf(trueStrings))
            {
                return true;
            }
            if (value.AnyOf(falseStrings))
            {
                return false;
            }
            throw new StringToBooleanException($"Value \"{value}\" could not be mapped.");
        }

        /// <summary>
        /// Converts this string instance to a bool value. The mapping is defined by params.
        /// </summary>
        /// <param name="value">The <see cref="string"/> instance</param>
        /// <param name="comparer">An equality comparer to compare strings</param>
        /// <param name="trueStrings">List of string representations for true</param>
        /// <param name="falseStrings">List of string representations for false</param>
        /// <returns>A bool value</returns>
        public static bool ToBoolean(this string value, IEqualityComparer<string> comparer, IEnumerable<string> trueStrings, IEnumerable<string> falseStrings)
        {
            if (value.AnyOf(comparer, trueStrings))
            {
                return true;
            }
            if (value.AnyOf(comparer, falseStrings))
            {
                return false;
            }
            throw new StringToBooleanException($"Value \"{value}\" could not be mapped.");
        }

        /// <summary>
        /// Converts this string instance to a bool value. The mapping is defined by params.
        /// </summary>
        /// <param name="value">The <see cref="string"/> instance</param>
        /// <param name="trueStrings">List of string representations for true</param>
        /// <param name="falseStrings">List of string representations for false</param>
        /// <returns>A bool value or null if string value is null or empty</returns>
        public static bool? ToBooleanOrDefault(this string value, IEnumerable<string> trueStrings, IEnumerable<string> falseStrings)
        {
            if (string.IsNullOrEmpty(value))
            {
                return default;
            }
            else
            {
                return ToBoolean(value, trueStrings, falseStrings);
            }
        }

        /// <summary>
        /// Converts this string instance to a bool value. The mapping is defined by params.
        /// </summary>
        /// <param name="value">The <see cref="string"/> instance</param>
        /// <param name="comparer">An equality comparer to compare strings</param>
        /// <param name="trueStrings">List of string representations for true</param>
        /// <param name="falseStrings">List of string representations for false</param>
        /// <returns>A bool value or null if string value is null or empty</returns>
        public static bool ToBooleanOrDefault(this string value, IEqualityComparer<string> comparer, IEnumerable<string> trueStrings, IEnumerable<string> falseStrings)
        {
            if (string.IsNullOrEmpty(value))
            {
                return default;
            }
            else
            {
                return ToBoolean(value, comparer, trueStrings, falseStrings);
            }
        }
    }
}