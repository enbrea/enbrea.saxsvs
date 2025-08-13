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
using System.Globalization;

namespace Enbrea.SaxSVS
{
    /// <summary>
    /// Utils for parsing dates, numbers etc. from XML values
    /// </summary>
    internal static class ParseUtils
    {
        private static readonly CultureInfo culture = new("de-DE");

        public static bool? ParseBooleanOrDefault(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                return value.ToBoolean(["1"], ["0"]);
            }
            return default;
        }

        public static DateOnly? ParseDateOnlyOrDefault(string value)
        {
            if (!string.IsNullOrWhiteSpace(value) && !value.Equals("unbegrenzt", StringComparison.OrdinalIgnoreCase))
            {
                if (DateOnly.TryParseExact(
                        value,
                        ["dd.MM.yyyy", "yyyy-MM-dd"],
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out var dto))
                {
                    return dto;
                }
            }
            return default;
        }

        public static DateTime? ParseDateTimeOrDefault(string value)
        {
            if (!string.IsNullOrWhiteSpace(value) && !value.Equals("unbegrenzt", StringComparison.OrdinalIgnoreCase))
            {
                if (DateTime.TryParseExact(
                        value,
                        ["dd.MM.yyyy HH:mm:ss", "yyyy.MM.dd HH:mm:ss", "yyyy-MM-ddTHH:mm:ss", "yyyy-MM-dd"],
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out var dto))
                {
                    return dto;
                }
            }
            return default;
        }

        public static double? ParseDoubleOrDefault(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                if (double.TryParse(
                        value, 
                        NumberStyles.Number,
                        culture,
                        out var dto))
                {
                    return dto;
                }
            }
            return default;
        }
    }
}