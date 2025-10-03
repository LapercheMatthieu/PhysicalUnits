using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Fractions;
using MatthL.PhysicalUnits.Core.Tools;
using Xunit;

namespace MatthL.PhysicalUnits.Tests.Core.Tools
{
    public class EquationToStringHelperTests
    {
        [Theory]
        [InlineData(0, "⁰")]
        [InlineData(1, "")]
        [InlineData(2, "²")]
        [InlineData(3, "³")]
        [InlineData(-1, "⁻¹")]
        [InlineData(-2, "⁻²")]
        [InlineData(10, "¹⁰")]
        public void ToSuperscript_WithInteger_FormatsCorrectly(int number, string expected)
        {
            // Arrange & Act
            var result = EquationToStringHelper.ToSuperscript(number);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToSuperscript_WithFraction_FormatsCorrectly()
        {
            // Arrange
            var fraction = new Fraction(1, 2);

            // Act
            var result = EquationToStringHelper.ToSuperscript(fraction);

            // Assert
            Assert.Equal("¹ᐟ²", result);
        }

        [Fact]
        public void ToSuperscript_WithNegativeFraction_FormatsCorrectly()
        {
            // Arrange
            var fraction = new Fraction(-3, 4);

            // Act
            var result = EquationToStringHelper.ToSuperscript(fraction);

            // Assert
            Assert.Equal("⁻³ᐟ⁴", result);
        }

        [Fact]
        public void ToSuperscript_WithWholeNumberFraction_FormatsAsInteger()
        {
            // Arrange
            var fraction = new Fraction(3, 1);

            // Act
            var result = EquationToStringHelper.ToSuperscript(fraction);

            // Assert
            Assert.Equal("³", result);
        }

        [Theory]
        [InlineData("m", 1, "m")]
        [InlineData("m", 2, "m²")]
        [InlineData("m", -1, "m⁻¹")]
        [InlineData("kg", 3, "kg³")]
        [InlineData("s", 0, "1")]
        public void FormatWithExponent_Integer_FormatsCorrectly(string baseSymbol, int exponent, string expected)
        {
            // Arrange & Act
            var result = EquationToStringHelper.FormatWithExponent(baseSymbol, exponent);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void FormatWithExponent_FractionalExponent_FormatsCorrectly()
        {
            // Arrange
            var fraction = new Fraction(1, 2);

            // Act
            var result = EquationToStringHelper.FormatWithExponent("m", fraction);

            // Assert
            Assert.Equal("m¹ᐟ²", result);
        }

        [Fact]
        public void FormatWithExponent_ComplexFraction_FormatsCorrectly()
        {
            // Arrange
            var fraction = new Fraction(-5, 3);

            // Act
            var result = EquationToStringHelper.FormatWithExponent("kg", fraction);

            // Assert
            Assert.Equal("kg⁻⁵ᐟ³", result);
        }

        [Theory]
        [InlineData(1, 1, "1")]
        [InlineData(5, 1, "5")]
        [InlineData(1, 2, "1/2")]
        [InlineData(3, 4, "3/4")]
        [InlineData(-2, 3, "-2/3")]
        public void FormatFactor_FormatsCorrectly(int numerator, int denominator, string expected)
        {
            // Arrange
            var fraction = new Fraction(numerator, denominator);

            // Act
            var result = EquationToStringHelper.FormatFactor(fraction);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
