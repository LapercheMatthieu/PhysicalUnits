using Fractions;
using MatthL.PhysicalUnits.Core.Enums;
using MatthL.PhysicalUnits.Core.Models;
using MatthL.PhysicalUnits.Infrastructure.Extensions;
using Xunit;

namespace MatthL.PhysicalUnits.Tests.Infrastructure
{
    public class PhysicalUnitInfrastructureExtensionsTests
    {
        [Fact]
        public void HasPrefixes_WithSIPrefix_ReturnsFalse()
        {
            // Arrange
            var unit = CreatePhysicalUnit();
            unit.BaseUnits.First().Prefix = Prefix.SI;

            // Act
            var result = unit.HasPrefixes();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void HasPrefixes_WithNonSIPrefix_ReturnsTrue()
        {
            // Arrange
            var unit = CreatePhysicalUnit();
            unit.BaseUnits.First().Prefix = Prefix.kilo;

            // Act
            var result = unit.HasPrefixes();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void HasPrefixes_MultipleBaseUnits_ChecksAll()
        {
            // Arrange
            var unit = CreateComplexPhysicalUnit();
            unit.BaseUnits.First().Prefix = Prefix.SI;
            unit.BaseUnits.Last().Prefix = Prefix.milli;

            // Act
            var result = unit.HasPrefixes();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void HasPrefixes_AllSI_ReturnsFalse()
        {
            // Arrange
            var unit = CreateComplexPhysicalUnit();
            foreach (var baseUnit in unit.BaseUnits)
            {
                baseUnit.Prefix = Prefix.SI;
            }

            // Act
            var result = unit.HasPrefixes();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ToTerm_CreatesPhysicalUnitTermWithExponent()
        {
            // Arrange
            var unit = CreatePhysicalUnit();
            var exponent = new Fraction(2);

            // Act
            var term = unit.ToTerm(exponent);

            // Assert
            Assert.Same(unit, term.Unit);
            Assert.Equal(exponent, term.Exponent);
        }

        [Fact]
        public void Add_AddsBaseUnitsFromOtherUnit()
        {
            // Arrange
            var unit1 = CreatePhysicalUnit();
            var unit2 = CreatePhysicalUnit();
            var originalCount = unit1.BaseUnits.Count;

            // Act
            var result = unit1.Add(unit2);

            // Assert
            Assert.Same(unit1, result);
            Assert.Equal(originalCount + unit2.BaseUnits.Count, result.BaseUnits.Count);
        }

        [Fact]
        public void Add_NullUnit_ReturnsUnchanged()
        {
            // Arrange
            var unit = CreatePhysicalUnit();
            var originalCount = unit.BaseUnits.Count;

            // Act
            var result = unit.Add(null);

            // Assert
            Assert.Same(unit, result);
            Assert.Equal(originalCount, result.BaseUnits.Count);
        }

        [Fact]
        public void Clone_CreatesIndependentCopy()
        {
            // Arrange
            var original = CreatePhysicalUnit();

            // Act
            var cloned = original.Clone();

            // Assert
            Assert.NotSame(original, cloned);
            Assert.Equal(original.UnitType, cloned.UnitType);
            Assert.Equal(original.BaseUnits.Count, cloned.BaseUnits.Count);
        }

        [Fact]
        public void Clone_ModifyingClone_DoesNotAffectOriginal()
        {
            // Arrange
            var original = CreatePhysicalUnit();
            var cloned = original.Clone();

            // Act
            cloned.UnitType = UnitType.Unknown_Special;
            cloned.BaseUnits.Clear();

            // Assert
            Assert.NotEqual(original.UnitType, cloned.UnitType);
            Assert.NotEmpty(original.BaseUnits);
        }

        [Fact]
        public void Clone_NullUnit_ReturnsEmptyPhysicalUnit()
        {
            // Arrange
            PhysicalUnit original = null;

            // Act
            var result = original.Clone();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.BaseUnits);
        }

        [Fact]
        public void Simplify_DuplicateBaseUnits_CombinesExponents()
        {
            // Arrange
            var unit = new PhysicalUnit { UnitType = UnitType.Unknown_Special };

            var baseUnit1 = CreateLengthBaseUnit();
            baseUnit1.Exponent = new Fraction(2);

            var baseUnit2 = CreateLengthBaseUnit();
            baseUnit2.Exponent = new Fraction(3);

            unit.BaseUnits.Add(baseUnit1);
            unit.BaseUnits.Add(baseUnit2);

            // Act
            var simplified = unit.Simplify();

            // Assert
            Assert.Single(simplified.BaseUnits);
            Assert.Equal(5, simplified.BaseUnits.First().Exponent.ToDouble(), 2);
        }

        [Fact]
        public void Simplify_CancellingExponents_RemovesUnit()
        {
            // Arrange
            var unit = new PhysicalUnit { UnitType = UnitType.Unknown_Special };

            var baseUnit1 = CreateLengthBaseUnit();
            baseUnit1.Exponent = new Fraction(2);

            var baseUnit2 = CreateLengthBaseUnit();
            baseUnit2.Exponent = new Fraction(-2);

            unit.BaseUnits.Add(baseUnit1);
            unit.BaseUnits.Add(baseUnit2);

            // Act
            var simplified = unit.Simplify();

            // Assert
            Assert.Empty(simplified.BaseUnits);
        }

        [Fact]
        public void Simplify_DifferentUnitTypes_KeepsSeparate()
        {
            // Arrange
            var unit = new PhysicalUnit { UnitType = UnitType.Unknown_Special };
            unit.BaseUnits.Add(CreateLengthBaseUnit());
            unit.BaseUnits.Add(CreateMassBaseUnit());
            unit.BaseUnits.Add(CreateTimeBaseUnit());

            // Act
            var simplified = unit.Simplify();

            // Assert
            Assert.Equal(3, simplified.BaseUnits.Count);
        }

        [Fact]
        public void Simplify_PreservesUnitType()
        {
            // Arrange
            var unit = CreatePhysicalUnit();
            var originalType = unit.UnitType;

            // Act
            var simplified = unit.Simplify();

            // Assert
            Assert.Equal(originalType, simplified.UnitType);
        }

        [Fact]
        public void Simplify_AlreadySimplified_ReturnsEquivalent()
        {
            // Arrange
            var unit = CreatePhysicalUnit();

            // Act
            var simplified = unit.Simplify();

            // Assert
            Assert.Equal(unit.BaseUnits.Count, simplified.BaseUnits.Count);
        }

        [Fact]
        public void Simplify_ComplexUnit_SimplifiesCorrectly()
        {
            // Arrange - m·m·kg·kg·s^-1·s^-1
            var unit = new PhysicalUnit { UnitType = UnitType.Unknown_Special };

            unit.BaseUnits.Add(CreateLengthBaseUnit());
            unit.BaseUnits.Add(CreateLengthBaseUnit());
            unit.BaseUnits.Add(CreateMassBaseUnit());
            unit.BaseUnits.Add(CreateMassBaseUnit());

            var time1 = CreateTimeBaseUnit();
            time1.Exponent = new Fraction(-1);
            unit.BaseUnits.Add(time1);

            var time2 = CreateTimeBaseUnit();
            time2.Exponent = new Fraction(-1);
            unit.BaseUnits.Add(time2);

            // Act
            var simplified = unit.Simplify();

            // Assert - Should be m²·kg²·s^-2
            Assert.Equal(3, simplified.BaseUnits.Count);

            var length = simplified.BaseUnits.First(b => b.Symbol == "m");
            Assert.Equal(2, length.Exponent.ToDouble(), 2);

            var mass = simplified.BaseUnits.First(b => b.Symbol == "kg");
            Assert.Equal(2, mass.Exponent.ToDouble(), 2);

            var time = simplified.BaseUnits.First(b => b.Symbol == "s");
            Assert.Equal(-2, time.Exponent.ToDouble(), 2);
        }

        // Helper methods
        private PhysicalUnit CreatePhysicalUnit()
        {
            var baseUnit = CreateLengthBaseUnit();
            return new PhysicalUnit(baseUnit);
        }

        private PhysicalUnit CreateComplexPhysicalUnit()
        {
            var unit = new PhysicalUnit { UnitType = UnitType.Force_Mech };
            unit.BaseUnits.Add(CreateMassBaseUnit());
            unit.BaseUnits.Add(CreateLengthBaseUnit());

            var timeUnit = CreateTimeBaseUnit();
            timeUnit.Exponent = new Fraction(-2);
            unit.BaseUnits.Add(timeUnit);

            return unit;
        }

        private BaseUnit CreateLengthBaseUnit()
        {
            var baseUnit = new BaseUnit
            {
                Name = "Meter",
                Symbol = "m",
                UnitType = UnitType.Length_Base,
                UnitSystem = StandardUnitSystem.SI,
                IsSI = true,
                Prefix = Prefix.SI,
                Exponent = new Fraction(1)
            };
            baseUnit.RawUnits.Add(new RawUnit(BaseUnitType.Length, 1));
            return baseUnit;
        }

        private BaseUnit CreateMassBaseUnit()
        {
            var baseUnit = new BaseUnit
            {
                Name = "Kilogram",
                Symbol = "kg",
                UnitType = UnitType.Mass_Base,
                UnitSystem = StandardUnitSystem.SI,
                IsSI = true,
                Prefix = Prefix.SI,
                Exponent = new Fraction(1)
            };
            baseUnit.RawUnits.Add(new RawUnit(BaseUnitType.Mass, 1));
            return baseUnit;
        }

        private BaseUnit CreateTimeBaseUnit()
        {
            var baseUnit = new BaseUnit
            {
                Name = "Second",
                Symbol = "s",
                UnitType = UnitType.Time_Base,
                UnitSystem = StandardUnitSystem.SI,
                IsSI = true,
                Prefix = Prefix.SI,
                Exponent = new Fraction(1)
            };
            baseUnit.RawUnits.Add(new RawUnit(BaseUnitType.Time, 1));
            return baseUnit;
        }
    }
}