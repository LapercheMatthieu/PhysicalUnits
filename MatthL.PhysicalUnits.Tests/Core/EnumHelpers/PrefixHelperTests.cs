using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MatthL.PhysicalUnits.Core.EnumHelpers;
using MatthL.PhysicalUnits.Core.Enums;
using Xunit;

namespace MatthL.PhysicalUnits.Tests.Core.EnumHelpers
{
    public class PrefixHelperTests
    {
        [Theory]
        [InlineData(Prefix.SI, "", "")]
        [InlineData(Prefix.kilo, "k", "kilo")]
        [InlineData(Prefix.mega, "M", "mega")]
        [InlineData(Prefix.micro, "μ", "micro")]
        [InlineData(Prefix.nano, "n", "nano")]
        public void GetSymbol_ReturnsCorrectSymbol(Prefix prefix, string expectedSymbol, string expectedName)
        {
            // Arrange & Act
            var symbol = PrefixHelper.GetSymbol(prefix);
            var name = PrefixHelper.GetName(prefix);

            // Assert
            Assert.Equal(expectedSymbol, symbol);
            Assert.Equal(expectedName, name);
        }

        [Theory]
        [InlineData(Prefix.kilo, 1e3)]
        [InlineData(Prefix.mega, 1e6)]
        [InlineData(Prefix.giga, 1e9)]
        [InlineData(Prefix.milli, 1e-3)]
        [InlineData(Prefix.micro, 1e-6)]
        [InlineData(Prefix.nano, 1e-9)]
        [InlineData(Prefix.SI, 1)]
        public void GetSize_ReturnsCorrectMultiplier(Prefix prefix, double expectedSize)
        {
            // Arrange & Act
            var size = PrefixHelper.GetSize(prefix);

            // Assert
            Assert.Equal((decimal)expectedSize, size);
        }

        [Theory]
        [InlineData(1000, Prefix.SI, Prefix.kilo, 1)]
        [InlineData(1, Prefix.kilo, Prefix.SI, 1000)]
        [InlineData(1000, Prefix.milli, Prefix.SI, 1)]
        [InlineData(1, Prefix.mega, Prefix.kilo, 1000)]
        [InlineData(1000000, Prefix.micro, Prefix.SI, 1)]
        public void Convert_ConvertsCorrectly(decimal value, Prefix from, Prefix to, decimal expected)
        {
            // Arrange & Act
            var result = PrefixHelper.Convert(value, from, to);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("k", Prefix.kilo)]
        [InlineData("M", Prefix.mega)]
        [InlineData("μ", Prefix.micro)]
        [InlineData("", Prefix.SI)]
        public void GetPrefixBySymbol_FindsCorrectPrefix(string symbol, Prefix expected)
        {
            // Arrange & Act
            var result = PrefixHelper.GetPrefixBySymbol(symbol);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expected, result.Value);
        }

        [Theory]
        [InlineData("kilo", Prefix.kilo)]
        [InlineData("mega", Prefix.mega)]
        [InlineData("micro", Prefix.micro)]
        [InlineData("nano", Prefix.nano)]
        public void GetPrefixByName_FindsCorrectPrefix(string name, Prefix expected)
        {
            // Arrange & Act
            var result = PrefixHelper.GetPrefixByName(name);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expected, result.Value);
        }

        [Theory]
        [InlineData(1500, Prefix.kilo)]    // 1.5 km
        [InlineData(0.005, Prefix.milli)]  // 5 mm
        [InlineData(1000000, Prefix.mega)] // 1 M
        [InlineData(0.000001, Prefix.micro)] // 1 μ
        public void FindBestPrefix_SelectsOptimalPrefix(decimal value, Prefix expected)
        {
            // Arrange & Act
            var result = PrefixHelper.FindBestPrefix(value);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void FindBestPrefix_WithZero_ReturnsSI()
        {
            // Arrange & Act
            var result = PrefixHelper.FindBestPrefix(0);

            // Assert
            Assert.Equal(Prefix.SI, result);
        }

        [Fact]
        public void FindBestPrefix_WithNegativeValue_IgnoresSign()
        {
            // Arrange & Act
            var result1 = PrefixHelper.FindBestPrefix(1500);
            var result2 = PrefixHelper.FindBestPrefix(-1500);

            // Assert
            Assert.Equal(result1, result2);
        }
    }
}