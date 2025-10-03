using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Fractions;
using MatthL.PhysicalUnits.Core.Enums;
using MatthL.PhysicalUnits.Core.Models;
using System.Numerics;
using Xunit;

namespace MatthL.PhysicalUnits.Tests.Core.Models
{
    public class BaseUnitTests
    {
        [Fact]
        public void Exponent_SetAndGet_WorksCorrectly()
        {
            // Arrange
            var baseUnit = new BaseUnit();
            var fraction = new Fraction(3, 4);

            // Act
            baseUnit.Exponent = fraction;

            // Assert
            Assert.Equal(3, baseUnit.Exponent_Numerator);
            Assert.Equal(4, baseUnit.Exponent_Denominator);
            Assert.Equal(fraction, baseUnit.Exponent);
        }

        [Fact]
        public void Exponent_WithHugeValue_ThrowsOverflowException()
        {
            // Arrange
            var baseUnit = new BaseUnit();
            var hugeFraction = new Fraction(long.MaxValue, 1);

            // Act & Assert
            Assert.Throws<OverflowException>(() => baseUnit.Exponent = hugeFraction);
        }

        [Fact]
        public void ConversionFactor_SetAndGet_WorksCorrectly()
        {
            // Arrange
            var baseUnit = new BaseUnit();
            var factor = new Fraction(1000, 1);

            // Act
            baseUnit.ConversionFactor = factor;

            // Assert
            Assert.Equal("1000", baseUnit.ConversionFactor_Numerator);
            Assert.Equal("1", baseUnit.ConversionFactor_Denominator);
            Assert.Equal(factor, baseUnit.ConversionFactor);
        }

        [Fact]
        public void ConversionFactor_WithLargeNumbers_StoresCorrectly()
        {
            // Arrange
            var baseUnit = new BaseUnit();
            var bigNum = BigInteger.Parse("123456789012345678901234567890");
            var factor = new Fraction(bigNum, 1);

            // Act
            baseUnit.ConversionFactor = factor;
            var retrieved = baseUnit.ConversionFactor;

            // Assert
            Assert.Equal(factor, retrieved);
        }

        [Fact]
        public void PrefixedSymbol_CombinesPrefixAndSymbol()
        {
            // Arrange
            var baseUnit = new BaseUnit
            {
                Symbol = "m",
                Prefix = Prefix.kilo
            };

            // Act
            var result = baseUnit.PrefixedSymbol;

            // Assert
            Assert.Equal("km", result);
        }

        [Fact]
        public void PrefixedName_CombinesPrefixAndName()
        {
            // Arrange
            var baseUnit = new BaseUnit
            {
                Name = "meter",
                Prefix = Prefix.centi
            };

            // Act
            var result = baseUnit.PrefixedName;

            // Assert
            Assert.Equal("centimeter", result);
        }

        [Fact]
        public void Domain_ReturnsCorrectDomainFromUnitType()
        {
            // Arrange
            var baseUnit = new BaseUnit
            {
                UnitType = UnitType.Force_Mech
            };

            // Act
            var domain = baseUnit.Domain;

            // Assert
            Assert.Equal(PhysicalUnitDomain.Mechanics, domain);
        }

        [Theory]
        [InlineData("N", 1, "N")]
        [InlineData("N", 2, "N²")]
        [InlineData("N", -1, "N⁻¹")]
        [InlineData("m", 3, "m³")]
        public void ToString_WithDifferentExponents_FormatsCorrectly(string symbol, int exponent, string expected)
        {
            // Arrange
            var baseUnit = new BaseUnit
            {
                Symbol = symbol,
                Prefix = Prefix.SI,
                Exponent = new Fraction(exponent, 1)
            };

            // Act
            var result = baseUnit.ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToString_WithPrefix_IncludesPrefix()
        {
            // Arrange
            var baseUnit = new BaseUnit
            {
                Symbol = "m",
                Prefix = Prefix.kilo,
                Exponent = new Fraction(1, 1)
            };

            // Act
            var result = baseUnit.ToString();

            // Assert
            Assert.Equal("km", result);
        }

        [Fact]
        public void ToString_WithFractionalExponent_FormatsWithSuperscript()
        {
            // Arrange
            var baseUnit = new BaseUnit
            {
                Symbol = "m",
                Prefix = Prefix.SI,
                Exponent = new Fraction(1, 2)
            };

            // Act
            var result = baseUnit.ToString();

            // Assert
            Assert.Equal("m¹ᐟ²", result);
        }

        [Fact]
        public void IsSI_DefaultsToFalse()
        {
            // Arrange & Act
            var baseUnit = new BaseUnit();

            // Assert
            Assert.False(baseUnit.IsSI);
        }

        [Fact]
        public void Offset_DefaultsToZero()
        {
            // Arrange & Act
            var baseUnit = new BaseUnit();

            // Assert
            Assert.Equal(0, baseUnit.Offset);
        }

        [Fact]
        public void UnitSystem_CanBeSet()
        {
            // Arrange
            var baseUnit = new BaseUnit();

            // Act
            baseUnit.UnitSystem = StandardUnitSystem.Imperial;

            // Assert
            Assert.Equal(StandardUnitSystem.Imperial, baseUnit.UnitSystem);
        }

        [Fact]
        public void RawUnits_InitializesAsEmptyCollection()
        {
            // Arrange & Act
            var baseUnit = new BaseUnit();

            // Assert
            Assert.NotNull(baseUnit.RawUnits);
            Assert.Empty(baseUnit.RawUnits);
        }

        [Fact]
        public void RawUnits_CanAddRawUnits()
        {
            // Arrange
            var baseUnit = new BaseUnit();
            var rawUnit1 = new RawUnit(BaseUnitType.Length, 1);
            var rawUnit2 = new RawUnit(BaseUnitType.Time, -1);

            // Act
            baseUnit.RawUnits.Add(rawUnit1);
            baseUnit.RawUnits.Add(rawUnit2);

            // Assert
            Assert.Equal(2, baseUnit.RawUnits.Count);
            Assert.Contains(rawUnit1, baseUnit.RawUnits);
            Assert.Contains(rawUnit2, baseUnit.RawUnits);
        }

        [Fact]
        public void CompleteUnit_Newton_ConfiguresCorrectly()
        {
            // Arrange & Act
            var newton = new BaseUnit
            {
                Name = "Newton",
                Symbol = "N",
                UnitType = UnitType.Force_Mech,
                UnitSystem = StandardUnitSystem.SI,
                IsSI = true,
                Prefix = Prefix.SI,
                Exponent = new Fraction(1, 1),
                ConversionFactor = new Fraction(1, 1)
            };

            // Add raw units: kg·m·s⁻²
            newton.RawUnits.Add(new RawUnit(BaseUnitType.Mass, 1));
            newton.RawUnits.Add(new RawUnit(BaseUnitType.Length, 1));
            newton.RawUnits.Add(new RawUnit(BaseUnitType.Time, -2));

            // Assert
            Assert.Equal("Newton", newton.Name);
            Assert.Equal("N", newton.Symbol);
            Assert.Equal(UnitType.Force_Mech, newton.UnitType);
            Assert.True(newton.IsSI);
            Assert.Equal(3, newton.RawUnits.Count);
            Assert.Equal(PhysicalUnitDomain.Mechanics, newton.Domain);
        }

        [Fact]
        public void CompleteUnit_Kilometer_ConfiguresCorrectly()
        {
            // Arrange & Act
            var kilometer = new BaseUnit
            {
                Name = "meter",
                Symbol = "m",
                UnitType = UnitType.Length_Base,
                UnitSystem = StandardUnitSystem.SI,
                IsSI = true,
                Prefix = Prefix.kilo,
                Exponent = new Fraction(1, 1),
                ConversionFactor = new Fraction(1000, 1)
            };

            kilometer.RawUnits.Add(new RawUnit(BaseUnitType.Length, 1));

            // Assert
            Assert.Equal("km", kilometer.PrefixedSymbol);
            Assert.Equal("kilometer", kilometer.PrefixedName);
            Assert.Equal(new Fraction(1000, 1), kilometer.ConversionFactor);
        }

        [Fact]
        public void ConversionFactor_DefaultsToOne()
        {
            // Arrange & Act
            var baseUnit = new BaseUnit();

            // Assert
            Assert.Equal(new Fraction(1, 1), baseUnit.ConversionFactor);
        }

        [Fact]
        public void Exponent_DefaultsToOne()
        {
            // Arrange & Act
            var baseUnit = new BaseUnit();

            // Assert
            Assert.Equal(new Fraction(1, 1), baseUnit.Exponent);
        }
    }
}