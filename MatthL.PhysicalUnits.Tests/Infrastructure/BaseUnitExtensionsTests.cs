using Fractions;
using MatthL.PhysicalUnits.Core.Enums;
using MatthL.PhysicalUnits.Core.Models;
using MatthL.PhysicalUnits.Infrastructure.Extensions;
using Xunit;

namespace MatthL.PhysicalUnits.Tests.Infrastructure
{
    public class BaseUnitExtensionsTests
    {
        [Fact]
        public void Clone_SimpleBaseUnit_CreatesIndependentCopy()
        {
            // Arrange
            var original = CreateTestBaseUnit();

            // Act
            var cloned = original.Clone();

            // Assert
            Assert.NotSame(original, cloned);
            Assert.Equal(original.Name, cloned.Name);
            Assert.Equal(original.Symbol, cloned.Symbol);
            Assert.Equal(original.UnitType, cloned.UnitType);
            Assert.Equal(original.UnitSystem, cloned.UnitSystem);
            Assert.Equal(original.Prefix, cloned.Prefix);
            Assert.Equal(original.IsSI, cloned.IsSI);
            Assert.Equal(original.Exponent, cloned.Exponent);
            Assert.Equal(original.ConversionFactor, cloned.ConversionFactor);
            Assert.Equal(original.Offset, cloned.Offset);
        }

        [Fact]
        public void Clone_WithRawUnits_ClonesRawUnitsCollection()
        {
            // Arrange
            var original = CreateForceBaseUnit();

            // Act
            var cloned = original.Clone();

            // Assert
            Assert.NotSame(original.RawUnits, cloned.RawUnits);
            Assert.Equal(original.RawUnits.Count, cloned.RawUnits.Count);

            // Verify raw units are also cloned (not same references)
            foreach (var (origRaw, clonedRaw) in original.RawUnits.Zip(cloned.RawUnits))
            {
                Assert.NotSame(origRaw, clonedRaw);
                Assert.Equal(origRaw.UnitType, clonedRaw.UnitType);
                Assert.Equal(origRaw.Exponent, clonedRaw.Exponent);
            }
        }

        [Fact]
        public void Clone_ModifyingClone_DoesNotAffectOriginal()
        {
            // Arrange
            var original = CreateTestBaseUnit();
            var cloned = original.Clone();

            // Act
            cloned.Name = "Modified";
            cloned.Exponent = new Fraction(5);
            cloned.Prefix = Prefix.kilo;

            // Assert
            Assert.NotEqual(original.Name, cloned.Name);
            Assert.NotEqual(original.Exponent, cloned.Exponent);
            Assert.NotEqual(original.Prefix, cloned.Prefix);
        }

        [Fact]
        public void Clone_WithOffset_PreservesOffset()
        {
            // Arrange - Temperature unit with offset (like Celsius)
            var original = CreateTestBaseUnit();
            original.Offset = 273.15;

            // Act
            var cloned = original.Clone();

            // Assert
            Assert.Equal(original.Offset, cloned.Offset);
        }

        [Fact]
        public void Clone_WithPrefix_PreservesPrefix()
        {
            // Arrange
            var original = CreateTestBaseUnit();
            original.Prefix = Prefix.kilo;

            // Act
            var cloned = original.Clone();

            // Assert
            Assert.Equal(Prefix.kilo, cloned.Prefix);
        }

        [Fact]
        public void AddPrefix_CreatesNewInstanceWithPrefix()
        {
            // Arrange
            var original = CreateTestBaseUnit();
            original.Prefix = Prefix.SI;

            // Act
            var withPrefix = original.AddPrefix(Prefix.kilo);

            // Assert
            Assert.NotSame(original, withPrefix);
            Assert.Equal(Prefix.SI, original.Prefix);
            Assert.Equal(Prefix.kilo, withPrefix.Prefix);
        }

        [Fact]
        public void AddPrefix_PreservesOtherProperties()
        {
            // Arrange
            var original = CreateTestBaseUnit();

            // Act
            var withPrefix = original.AddPrefix(Prefix.milli);

            // Assert
            Assert.Equal(original.Name, withPrefix.Name);
            Assert.Equal(original.Symbol, withPrefix.Symbol);
            Assert.Equal(original.UnitType, withPrefix.UnitType);
            Assert.Equal(original.UnitSystem, withPrefix.UnitSystem);
            Assert.Equal(original.Exponent, withPrefix.Exponent);
        }

        [Fact]
        public void AddPrefix_WithMultiplePrefixes_EachCreatesNewInstance()
        {
            // Arrange
            var original = CreateTestBaseUnit();

            // Act
            var kilo = original.AddPrefix(Prefix.kilo);
            var milli = original.AddPrefix(Prefix.milli);
            var mega = original.AddPrefix(Prefix.mega);

            // Assert
            Assert.NotSame(kilo, milli);
            Assert.NotSame(kilo, mega);
            Assert.NotSame(milli, mega);
            Assert.Equal(Prefix.kilo, kilo.Prefix);
            Assert.Equal(Prefix.milli, milli.Prefix);
            Assert.Equal(Prefix.mega, mega.Prefix);
        }

        [Fact]
        public void Clone_WithFractionalExponent_PreservesExponent()
        {
            // Arrange
            var original = CreateTestBaseUnit();
            original.Exponent = new Fraction(3, 2);

            // Act
            var cloned = original.Clone();

            // Assert
            Assert.Equal(new Fraction(3, 2), cloned.Exponent);
        }

        [Fact]
        public void Clone_WithConversionFactor_PreservesConversionFactor()
        {
            // Arrange
            var original = CreateTestBaseUnit();
            original.ConversionFactor = new Fraction(1000);

            // Act
            var cloned = original.Clone();

            // Assert
            Assert.Equal(new Fraction(1000), cloned.ConversionFactor);
        }

        [Fact]
        public void Clone_EmptyRawUnits_CreatesEmptyCollection()
        {
            // Arrange
            var original = CreateTestBaseUnit();
            original.RawUnits.Clear();

            // Act
            var cloned = original.Clone();

            // Assert
            Assert.Empty(cloned.RawUnits);
        }

        [Fact]
        public void AddPrefix_ToUnitWithRawUnits_PreservesRawUnits()
        {
            // Arrange
            var original = CreateForceBaseUnit();
            var originalRawCount = original.RawUnits.Count;

            // Act
            var withPrefix = original.AddPrefix(Prefix.kilo);

            // Assert
            Assert.Equal(originalRawCount, withPrefix.RawUnits.Count);
        }

        // Helper methods
        private BaseUnit CreateTestBaseUnit()
        {
            return new BaseUnit
            {
                Name = "TestUnit",
                Symbol = "T",
                UnitType = UnitType.Length_Base,
                UnitSystem = StandardUnitSystem.SI,
                IsSI = true,
                Prefix = Prefix.SI,
                Exponent = new Fraction(1),
                ConversionFactor = new Fraction(1),
                Offset = 0
            };
        }

        private BaseUnit CreateForceBaseUnit()
        {
            var baseUnit = new BaseUnit
            {
                Name = "Newton",
                Symbol = "N",
                UnitType = UnitType.Force_Mech,
                UnitSystem = StandardUnitSystem.SI,
                IsSI = true,
                Prefix = Prefix.SI,
                Exponent = new Fraction(1)
            };

            baseUnit.RawUnits.Add(new RawUnit(BaseUnitType.Mass, 1));
            baseUnit.RawUnits.Add(new RawUnit(BaseUnitType.Length, 1));
            baseUnit.RawUnits.Add(new RawUnit(BaseUnitType.Time, -2));

            return baseUnit;
        }
    }
}