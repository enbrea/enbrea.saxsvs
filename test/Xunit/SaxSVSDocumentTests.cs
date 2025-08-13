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
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Enbrea.SaxSVS.Tests
{
    /// <summary>
    /// Unit tests for <see cref="SaxSVSDocument"/>.
    /// </summary>
    public class SaxSVSDocumentTests : IClassFixture<SaxSVSFixture>
    {
        private readonly SaxSVSFixture _saxSVSFixture;

        public SaxSVSDocumentTests(SaxSVSFixture saxSVSFixture)
        {
            _saxSVSFixture = saxSVSFixture;
        }

        [Fact]
        public async Task Test_Classes()
        {
            using var strReader = File.OpenText(_saxSVSFixture.GetTestFilePath());

            var saxSVSReader = new SaxSVSReader(strReader);
            var saxSVSDocument = await saxSVSReader.ReadDocumentAsync();

            Assert.Single(saxSVSDocument.Classes);
            Assert.Equal("9a", saxSVSDocument.Classes[0].Name);
            Assert.Equal("Klassenstufe 9 Oberschule", saxSVSDocument.Classes[0].ClassType.Name);
            Assert.Equal("20100", saxSVSDocument.Classes[0].ClassType.Code);
            Assert.Null(saxSVSDocument.Classes[0].ClassType.CodeList);
            Assert.Equal("KL9", saxSVSDocument.Classes[0].ClassLevel.Name);
            Assert.Equal("09", saxSVSDocument.Classes[0].ClassLevel.Code);
            Assert.Null(saxSVSDocument.Classes[0].ClassLevel.CodeList);
        }

        [Fact]
        public async Task Test_Lessons()
        {
            using var strReader = File.OpenText(_saxSVSFixture.GetTestFilePath());

            var saxSVSReader = new SaxSVSReader(strReader);
            var saxSVSDocument = await saxSVSReader.ReadDocumentAsync();

            Assert.Equal(2, saxSVSDocument.Lessons.Count);
            Assert.Equal(new Guid("7a3717d3-a98b-4f33-8cb6-35b7af2d3b4a"), saxSVSDocument.Lessons[0].Id);
            Assert.Equal("S0520", saxSVSDocument.Lessons[0].Subject.CodeList);
            Assert.Equal("60040", saxSVSDocument.Lessons[0].Subject.Code);
            Assert.Equal("INF", saxSVSDocument.Lessons[0].Subject.Name);
            Assert.Single(saxSVSDocument.Lessons[0].Classes);
            Assert.Equal(new Guid("2680385e-105e-4be8-85d5-250d09e0d8b4"), saxSVSDocument.Lessons[0].Classes[0].ClassId);
            Assert.Equal(new Guid("1458d0b3-e964-40b6-bd63-b3f9d4a79727"), saxSVSDocument.Lessons[1].Id);
            Assert.Equal("S0520", saxSVSDocument.Lessons[1].Subject.CodeList);
            Assert.Equal("71010", saxSVSDocument.Lessons[1].Subject.Code);
            Assert.Equal("DAZ", saxSVSDocument.Lessons[1].Subject.Name);
            Assert.Single(saxSVSDocument.Lessons[1].Classes);
            Assert.Equal(new Guid("2680385e-105e-4be8-85d5-250d09e0d8b4"), saxSVSDocument.Lessons[1].Classes[0].ClassId);
            Assert.Single(saxSVSDocument.Lessons[1].Students);
            Assert.Equal(new Guid("9ad6bf45-c3c4-4f3f-83ff-e42433b3e188"), saxSVSDocument.Lessons[1].Students[0].StudentId);
        }

        [Fact]
        public async Task Test_Metadata()
        {
            using var strReader = File.OpenText(_saxSVSFixture.GetTestFilePath());

            var saxSVSReader = new SaxSVSReader(strReader);
            var saxSVSDocument = await saxSVSReader.ReadDocumentAsync();

            Assert.Equal("2024/2025", saxSVSDocument.AcademicYear);
            Assert.Equal("1234567", saxSVSDocument.FacilityKey);
            Assert.Equal("Testschule", saxSVSDocument.Facility.Name);
            Assert.Equal("1234567", saxSVSDocument.Facility.Key);
            Assert.Equal(2, saxSVSDocument.Facility.Cooperations.Count);
            Assert.Equal("Grundschule Test1", saxSVSDocument.Facility.Cooperations[0].Name);
            Assert.Equal("0123456", saxSVSDocument.Facility.Cooperations[0].Key);
            Assert.Equal("Grundschule Test2", saxSVSDocument.Facility.Cooperations[1].Name);
            Assert.Equal("1123456", saxSVSDocument.Facility.Cooperations[1].Key);
            Assert.Null(saxSVSDocument.Classes[0].ClassLevel.CodeList);
        }

        [Fact]
        public async Task Test_Students()
        {
            using var strReader = File.OpenText(_saxSVSFixture.GetTestFilePath());

            var saxSVSReader = new SaxSVSReader(strReader);
            var saxSVSDocument = await saxSVSReader.ReadDocumentAsync();

            Assert.Equal(2, saxSVSDocument.Students.Count);
            Assert.Equal("Schumacher", saxSVSDocument.Students[0].FamilyName);
            Assert.Equal("Beate", saxSVSDocument.Students[0].GivenName);
            Assert.Equal(new DateOnly(2013, 1, 1), saxSVSDocument.Students[0].BirthDate);
            Assert.Equal(2, saxSVSDocument.Students[0].Custodians.Count);
            Assert.Equal("Gagarina", saxSVSDocument.Students[1].FamilyName);
            Assert.Equal("Nastja", saxSVSDocument.Students[1].GivenName);
            Assert.Equal(new DateOnly(2012, 1, 1), saxSVSDocument.Students[1].BirthDate);
            Assert.Equal(3, saxSVSDocument.Students[1].Custodians.Count);
            Assert.Equal("Gagarina", saxSVSDocument.Students[1].Custodians[0].FamilyName);
            Assert.Equal(2, saxSVSDocument.Students[0].Promotions.Count);
            Assert.Equal("2024/2025", saxSVSDocument.Students[0].Promotions[1].AcademicYear);
        }

        [Fact]
        public async Task Test_Teachers()
        {
            using var strReader = File.OpenText(_saxSVSFixture.GetTestFilePath());

            var saxSVSReader = new SaxSVSReader(strReader);
            var saxSVSDocument = await saxSVSReader.ReadDocumentAsync();

            Assert.Equal(2, saxSVSDocument.Workforces.Count);
            Assert.Equal("HM", saxSVSDocument.Workforces[0].ShortName);
            Assert.Equal("Müller", saxSVSDocument.Workforces[0].FamilyName);
            Assert.Equal("Heidi", saxSVSDocument.Workforces[0].GivenName);
            Assert.Equal("2", saxSVSDocument.Workforces[0].Gender.Code);
            Assert.Equal("10", saxSVSDocument.Workforces[0].Status.Code);
            Assert.Equal("LK", saxSVSDocument.Workforces[0].Status.Name);
            Assert.Equal(new DateOnly(1984, 6, 6), saxSVSDocument.Workforces[0].BirthDate);
            Assert.Equal("WS", saxSVSDocument.Workforces[1].ShortName);
            Assert.Equal("Schneider", saxSVSDocument.Workforces[1].FamilyName);
            Assert.Equal("Walter", saxSVSDocument.Workforces[1].GivenName);
            Assert.Equal(new DateOnly(1992, 6, 6), saxSVSDocument.Workforces[1].BirthDate);
            Assert.Equal("1", saxSVSDocument.Workforces[1].Gender.Code);
            Assert.Equal("10", saxSVSDocument.Workforces[1].Status.Code);
            Assert.Equal("LK", saxSVSDocument.Workforces[1].Status.Name);
        }
    }
}
