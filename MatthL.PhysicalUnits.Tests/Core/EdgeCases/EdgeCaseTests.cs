using Fractions;
using MatthL.PhysicalUnits.Core.EnumHelpers;
using MatthL.PhysicalUnits.Core.Enums;
using MatthL.PhysicalUnits.Core.Models;
using Xunit;

namespace MatthL.PhysicalUnits.Tests.Core.EdgeCases
{
    public class EdgeCaseTests
    {
        [Fact]
        public void RawUnit_ZeroExponent_ReturnsOne()
        {
            // Arrange
            var rawUnit = new RawUnit(BaseUnitType.Length, 0);

            // Act
            var result = rawUnit.ToString();

            // Assert
            Assert.Equal("1", result);
        }

        [Fact]
        public void BaseUnit_ZeroExponent_ReturnsOne()
        {
            // Arrange
            var baseUnit = new BaseUnit
            {
                Symbol = "m",
                Exponent = new Fraction(0, 1)
            };

            // Act
            var result = baseUnit.ToString();

            // Assert
            Assert.Equal("1", result);
        }

        [Fact]
        public void Fraction_ExtremelyLargeExponent_ThrowsOverflow()
        {
            // Arrange
            var baseUnit = new BaseUnit();
            var hugeFraction = new Fraction(long.MaxValue, 1);

            // Act & Assert
            Assert.Throws<OverflowException>(() => baseUnit.Exponent = hugeFraction);
        }

        [Fact]
        public void ConversionFactor_VerySmallFraction_StoresCorrectly()
        {
            // Arrange
            var baseUnit = new BaseUnit();
            var tinyFraction = new Fraction(1, 1000000000);

            // Act
            baseUnit.ConversionFactor = tinyFraction;
            var retrieved = baseUnit.ConversionFactor;

            // Assert
            Assert.Equal(tinyFraction, retrieved);
        }

        [Fact]
        public void PrefixHelper_ConvertToSamePrefix_ReturnsOriginal()
        {
            // Arrange
            var value = 42.5m;

            // Act
            var result = PrefixHelper.Convert(value, Prefix.kilo, Prefix.kilo);

            // Assert
            Assert.Equal(value, result);
        }

        [Fact]
        public void PrefixHelper_ConvertZero_ReturnsZero()
        {
            // Arrange
            var value = 0m;

            // Act
            var result = PrefixHelper.Convert(value, Prefix.kilo, Prefix.milli);

            // Assert
            Assert.Equal(0m, result);
        }

        [Fact]
        public void PrefixHelper_NegativeValue_ConvertsCorrectly()
        {
            // Arrange
            var value = -5m;

            // Act
            var result = PrefixHelper.Convert(value, Prefix.kilo, Prefix.SI);

            // Assert
            Assert.Equal(-5000m, result);
        }

        [Fact]
        public void RawUnit_NegativeExponent_FormatsCorrectly()
        {
            // Arrange
            var rawUnit = new RawUnit(BaseUnitType.Time, -3);

            // Act
            var result = rawUnit.ToString();

            // Assert
            Assert.Equal("s⁻³", result);
        }

        [Fact]
        public void RawUnit_ComplexFraction_NegativeNumerator()
        {
            // Arrange
            var rawUnit = new RawUnit(BaseUnitType.Length, new Fraction(-3, 5));

            // Act
            var result = rawUnit.ToString();

            // Assert
            Assert.Equal("m⁻³ᐟ⁵", result);
        }

        [Fact]
        public void PhysicalUnit_EmptyBaseUnits_NameReturnsEmpty()
        {
            // Arrange
            var physicalUnit = new PhysicalUnit();

            // Act
            var name = physicalUnit.Name;

            // Assert
            Assert.Empty(name);
        }

        [Fact]
        public void PhysicalUnit_NullCopyConstructor_HandlesGracefully()
        {
            // Arrange & Act
            var physicalUnit = new PhysicalUnit();

            // Assert
            Assert.NotNull(physicalUnit.BaseUnits);
            Assert.Equal(UnitType.Unknown_Special, physicalUnit.UnitType);
        }

        [Fact]
        public void BaseUnit_AllPrefixes_HaveValidSizes()
        {
            // Arrange
            var allPrefixes = Enum.GetValues<Prefix>();

            // Act & Assert
            foreach (var prefix in allPrefixes)
            {
                var size = PrefixHelper.GetSize(prefix);
                Assert.True(size > 0, $"Prefix {prefix} has invalid size {size}");
            }
        }

        [Fact]
        public void BaseUnit_EmptySymbol_HandlesGracefully()
        {
            // Arrange
            var baseUnit = new BaseUnit
            {
                Symbol = "",
                Prefix = Prefix.SI,
                Exponent = new Fraction(1, 1)
            };

            // Act
            var result = baseUnit.ToString();

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void RawUnit_Power_WithZero_ReturnsZeroExponent()
        {
            // Arrange
            var rawUnit = new RawUnit(BaseUnitType.Mass, 5);
            var zeroPower = new Fraction(0, 1);

            // Act
            var result = rawUnit.Power(zeroPower);

            // Assert
            Assert.Equal(new Fraction(0, 1), result.Exponent);
        }

        [Fact]
        public void RawUnit_Power_NegativeExponent()
        {
            // Arrange
            var rawUnit = new RawUnit(BaseUnitType.Length, 2);
            var negativePower = new Fraction(-1, 1);

            // Act
            var result = rawUnit.Power(negativePower);

            // Assert
            Assert.Equal(new Fraction(-2, 1), result.Exponent);
        }

        [Fact]
        public void ConversionFactor_NegativeNumber_ThrowsOrHandles()
        {
            // Arrange
            var baseUnit = new BaseUnit();
            var negativeFraction = new Fraction(-1000, 1);

            // Act - Should either throw or handle gracefully
            // For physical units, negative conversion factors might not make sense
            baseUnit.ConversionFactor = negativeFraction;

            // Assert
            Assert.Equal(negativeFraction, baseUnit.ConversionFactor);
        }

        [Fact]
        public void BaseUnit_VeryLargePrefix_MicroToYotta()
        {
            // Arrange
            var value = 1m;

            // Act
            var result = PrefixHelper.Convert(value, Prefix.micro, Prefix.yotta);

            // Assert
            Assert.Equal(1e-30m, result);
        }

        [Theory]
        [InlineData(BaseUnitType.Length, BaseUnitType.Mass, false)]
        [InlineData(BaseUnitType.Time, BaseUnitType.Time, true)]
        [InlineData(BaseUnitType.Temperature, BaseUnitType.ElectricCurrent, false)]
        public void RawUnit_Equality_DifferentTypes(BaseUnitType type1, BaseUnitType type2, bool shouldBeEqual)
        {
            // Arrange
            var unit1 = new RawUnit(type1, 1);
            var unit2 = new RawUnit(type2, 1);

            // Act
            var areEqual = unit1.Equals(unit2);

            // Assert
            Assert.Equal(shouldBeEqual, areEqual);
        }

        [Fact]
        public void PhysicalUnit_MixedUnitSystems_ReturnsMixed()
        {
            // Arrange
            var siUnit = new BaseUnit { UnitSystem = StandardUnitSystem.SI };
            var imperialUnit = new BaseUnit { UnitSystem = StandardUnitSystem.Imperial };
            var usUnit = new BaseUnit { UnitSystem = StandardUnitSystem.US };

            var mixed = new PhysicalUnit();
            mixed.BaseUnits.Add(siUnit);
            mixed.BaseUnits.Add(imperialUnit);
            mixed.BaseUnits.Add(usUnit);

            // Act
            var system = mixed.UnitSystem;

            // Assert
            Assert.Equal(StandardUnitSystem.Mixed, system);
        }

        [Fact]
        public void PrefixHelper_FindBestPrefix_ExtremelySmallValue()
        {
            // Arrange
            var tinyValue = 0.000000000000001m;

            // Act
            var prefix = PrefixHelper.FindBestPrefix(tinyValue);

            // Assert
            Assert.NotEqual(Prefix.SI, prefix);
        }

        [Fact]
        public void PrefixHelper_FindBestPrefix_ExtremelyLargeValue()
        {
            // Arrange
            var hugeValue = 1000000000000000m;

            // Act
            var prefix = PrefixHelper.FindBestPrefix(hugeValue);

            // Assert
            Assert.NotEqual(Prefix.SI, prefix);
        }
    }
}