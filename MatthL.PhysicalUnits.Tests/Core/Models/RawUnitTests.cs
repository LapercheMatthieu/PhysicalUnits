using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Fractions;
using MatthL.PhysicalUnits.Core.Enums;
using MatthL.PhysicalUnits.Core.Models;
using Xunit;

namespace MatthL.PhysicalUnits.Tests.Core.Models
{
    public class RawUnitTests
    {
        [Fact]
        public void Constructor_WithIntegerExponent_CreatesCorrectly()
        {
            // Arrange & Act
            var rawUnit = new RawUnit(BaseUnitType.Length, 2);

            // Assert
            Assert.Equal(BaseUnitType.Length, rawUnit.UnitType);
            Assert.Equal(new Fraction(2, 1), rawUnit.Exponent);
            Assert.Equal(2, rawUnit.Exponent_Numerator);
            Assert.Equal(1, rawUnit.Exponent_Denominator);
        }

        [Fact]
        public void Constructor_WithFractionExponent_CreatesCorrectly()
        {
            // Arrange
            var fraction = new Fraction(3, 4);

            // Act
            var rawUnit = new RawUnit(BaseUnitType.Mass, fraction);

            // Assert
            Assert.Equal(BaseUnitType.Mass, rawUnit.UnitType);
            Assert.Equal(fraction, rawUnit.Exponent);
            Assert.Equal(3, rawUnit.Exponent_Numerator);
            Assert.Equal(4, rawUnit.Exponent_Denominator);
        }

        [Fact]
        public void Symbol_ReturnsCorrectBaseSymbol()
        {
            // Arrange
            var rawUnit = new RawUnit(BaseUnitType.Length, 1);

            // Act
            var symbol = rawUnit.Symbol;

            // Assert
            Assert.Equal("m", symbol);
        }

        [Fact]
        public void Power_MultipliesExponent()
        {
            // Arrange
            var rawUnit = new RawUnit(BaseUnitType.Time, 2);
            var powerFraction = new Fraction(3, 1);

            // Act
            var result = rawUnit.Power(powerFraction);

            // Assert
            Assert.Equal(new Fraction(6, 1), result.Exponent);
            Assert.Equal(BaseUnitType.Time, result.UnitType);
        }

        [Fact]
        public void Power_WithFractionalExponent_WorksCorrectly()
        {
            // Arrange
            var rawUnit = new RawUnit(BaseUnitType.Length, new Fraction(1, 2));
            var power = new Fraction(2, 1);

            // Act
            var result = rawUnit.Power(power);

            // Assert
            Assert.Equal(new Fraction(1, 1), result.Exponent);
        }

        [Fact]
        public void WithExponent_CreatesNewRawUnitWithDifferentExponent()
        {
            // Arrange
            var original = new RawUnit(BaseUnitType.Mass, 1);

            // Act
            var modified = original.WithExponent(3);

            // Assert
            Assert.Equal(BaseUnitType.Mass, modified.UnitType);
            Assert.Equal(new Fraction(3, 1), modified.Exponent);
            Assert.Equal(new Fraction(1, 1), original.Exponent); // Original unchanged
        }

        [Theory]
        [InlineData(1, "m")]
        [InlineData(2, "m²")]
        [InlineData(-1, "m⁻¹")]
        [InlineData(3, "m³")]
        public void ToString_FormatsCorrectly(int exponent, string expected)
        {
            // Arrange
            var rawUnit = new RawUnit(BaseUnitType.Length, exponent);

            // Act
            var result = rawUnit.ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToString_WithFractionalExponent_FormatsCorrectly()
        {
            // Arrange
            var rawUnit = new RawUnit(BaseUnitType.Length, new Fraction(1, 2));

            // Act
            var result = rawUnit.ToString();

            // Assert
            Assert.Equal("m¹ᐟ²", result);
        }

        [Fact]
        public void Equals_SameTypeAndExponent_ReturnsTrue()
        {
            // Arrange
            var unit1 = new RawUnit(BaseUnitType.Length, 2);
            var unit2 = new RawUnit(BaseUnitType.Length, 2);

            // Act & Assert
            Assert.Equal(unit1, unit2);
        }

        [Fact]
        public void Equals_DifferentExponent_ReturnsFalse()
        {
            // Arrange
            var unit1 = new RawUnit(BaseUnitType.Length, 2);
            var unit2 = new RawUnit(BaseUnitType.Length, 3);

            // Act & Assert
            Assert.NotEqual(unit1, unit2);
        }

        [Fact]
        public void Equals_DifferentType_ReturnsFalse()
        {
            // Arrange
            var unit1 = new RawUnit(BaseUnitType.Length, 2);
            var unit2 = new RawUnit(BaseUnitType.Mass, 2);

            // Act & Assert
            Assert.NotEqual(unit1, unit2);
        }

        [Fact]
        public void GetHashCode_SameTypeAndExponent_ReturnsSameHashCode()
        {
            // Arrange
            var unit1 = new RawUnit(BaseUnitType.Time, 1);
            var unit2 = new RawUnit(BaseUnitType.Time, 1);

            // Act & Assert
            Assert.Equal(unit1.GetHashCode(), unit2.GetHashCode());
        }

        [Fact]
        public void Exponent_SetWithOverflowValue_ThrowsException()
        {
            // Arrange
            var rawUnit = new RawUnit();
            var hugeFraction = new Fraction(long.MaxValue, 1);

            // Act & Assert
            Assert.Throws<OverflowException>(() => rawUnit.Exponent = hugeFraction);
        }

        [Theory]
        [InlineData(BaseUnitType.Length, "m")]
        [InlineData(BaseUnitType.Mass, "kg")]
        [InlineData(BaseUnitType.Time, "s")]
        [InlineData(BaseUnitType.ElectricCurrent, "A")]
        [InlineData(BaseUnitType.Temperature, "K")]
        public void Symbol_ForDifferentBaseTypes_ReturnsCorrectSymbol(BaseUnitType type, string expectedSymbol)
        {
            // Arrange
            var rawUnit = new RawUnit(type, 1);

            // Act
            var symbol = rawUnit.Symbol;

            // Assert
            Assert.Equal(expectedSymbol, symbol);
        }
    }
}
