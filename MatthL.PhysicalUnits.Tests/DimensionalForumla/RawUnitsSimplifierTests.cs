using Fractions;
using MatthL.PhysicalUnits.Core.Enums;
using MatthL.PhysicalUnits.Core.EquationModels;
using MatthL.PhysicalUnits.Core.Models;
using MatthL.PhysicalUnits.DimensionalFormulas.Helpers;
using Xunit;

namespace MatthL.PhysicalUnits.Tests.DimensionalFormulas
{
    public class RawUnitsSimplifierTests
    {
        [Fact]
        public void SimplifyFormula_SingleRawUnit_ReturnsCorrectDictionary()
        {
            // Arrange
            var rawUnit = new RawUnit(BaseUnitType.Length, 1);

            // Act
            var result = RawUnitsSimplifier.SimplifyFormula(rawUnit);

            // Assert
            Assert.Single(result);
            Assert.Equal(new Fraction(1), result[BaseUnitType.Length]);
        }

        [Fact]
        public void SimplifyFormula_MultipleRawUnits_SumsExponents()
        {
            // Arrange
            var rawUnit1 = new RawUnit(BaseUnitType.Length, 2);
            var rawUnit2 = new RawUnit(BaseUnitType.Length, 3);

            // Act
            var result = RawUnitsSimplifier.SimplifyFormula(rawUnit1, rawUnit2);

            // Assert
            Assert.Single(result);
            Assert.Equal(new Fraction(5), result[BaseUnitType.Length]);
        }

        [Fact]
        public void SimplifyFormula_OppositeExponents_CancelsOut()
        {
            // Arrange
            var rawUnit1 = new RawUnit(BaseUnitType.Length, 2);
            var rawUnit2 = new RawUnit(BaseUnitType.Length, -2);

            // Act
            var result = RawUnitsSimplifier.SimplifyFormula(rawUnit1, rawUnit2);

            // Assert
            Assert.Empty(result); // Should be empty as they cancel out
        }

        [Fact]
        public void SimplifyFormula_DifferentTypes_KeepsSeparate()
        {
            // Arrange
            var rawUnit1 = new RawUnit(BaseUnitType.Length, 1);
            var rawUnit2 = new RawUnit(BaseUnitType.Mass, 1);
            var rawUnit3 = new RawUnit(BaseUnitType.Time, -2);

            // Act
            var result = RawUnitsSimplifier.SimplifyFormula(rawUnit1, rawUnit2, rawUnit3);

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Equal(new Fraction(1), result[BaseUnitType.Length]);
            Assert.Equal(new Fraction(1), result[BaseUnitType.Mass]);
            Assert.Equal(new Fraction(-2), result[BaseUnitType.Time]);
        }

        [Fact]
        public void CalculateDimensionalFormula_PhysicalUnitTerm_ReturnsCorrectFormula()
        {
            // Arrange - Force (kg·m·s^-2) raised to power 1
            var baseUnit = CreateForceBaseUnit();
            var physicalUnit = new PhysicalUnit(baseUnit);
            var term = new PhysicalUnitTerm(physicalUnit, new Fraction(1));

            // Act
            var result = RawUnitsSimplifier.CalculateDimensionalFormula(term);

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Equal(new Fraction(1), result[BaseUnitType.Mass]);
            Assert.Equal(new Fraction(1), result[BaseUnitType.Length]);
            Assert.Equal(new Fraction(-2), result[BaseUnitType.Time]);
        }

        [Fact]
        public void CalculateDimensionalFormula_PhysicalUnitTermWithExponent_AppliesExponent()
        {
            // Arrange - Force^2
            var baseUnit = CreateForceBaseUnit();
            var physicalUnit = new PhysicalUnit(baseUnit);
            var term = new PhysicalUnitTerm(physicalUnit, new Fraction(2));

            // Act
            var result = RawUnitsSimplifier.CalculateDimensionalFormula(term);

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Equal(new Fraction(2), result[BaseUnitType.Mass]);
            Assert.Equal(new Fraction(2), result[BaseUnitType.Length]);
            Assert.Equal(new Fraction(-4), result[BaseUnitType.Time]);
        }

        [Fact]
        public void CalculateDimensionalFormula_PhysicalUnit_ReturnsCorrectFormula()
        {
            // Arrange - Speed (m/s)
            var baseUnit = CreateSpeedBaseUnit();
            var physicalUnit = new PhysicalUnit(baseUnit);

            // Act
            var result = RawUnitsSimplifier.CalculateDimensionalFormula(physicalUnit);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(new Fraction(1), result[BaseUnitType.Length]);
            Assert.Equal(new Fraction(-1), result[BaseUnitType.Time]);
        }

        [Fact]
        public void CalculateDimensionalFormula_MultiplePhysicalUnits_CombinesFormulas()
        {
            // Arrange - Force × Distance = Energy
            var force = new PhysicalUnit(CreateForceBaseUnit());
            var distance = new PhysicalUnit(CreateLengthBaseUnit());

            // Act
            var result = RawUnitsSimplifier.CalculateDimensionalFormula(force, distance);

            // Assert - kg·m²·s^-2
            Assert.Equal(3, result.Count);
            Assert.Equal(new Fraction(1), result[BaseUnitType.Mass]);
            Assert.Equal(new Fraction(2), result[BaseUnitType.Length]);
            Assert.Equal(new Fraction(-2), result[BaseUnitType.Time]);
        }

        [Fact]
        public void CalculateDimensionalFormula_BaseUnit_ReturnsCorrectFormula()
        {
            // Arrange
            var baseUnit = CreateForceBaseUnit();

            // Act
            var result = RawUnitsSimplifier.CalculateDimensionalFormula(baseUnit);

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Equal(new Fraction(1), result[BaseUnitType.Mass]);
            Assert.Equal(new Fraction(1), result[BaseUnitType.Length]);
            Assert.Equal(new Fraction(-2), result[BaseUnitType.Time]);
        }

        [Fact]
        public void CalculateDimensionalFormula_RawUnits_PassesThrough()
        {
            // Arrange
            var rawUnit1 = new RawUnit(BaseUnitType.Length, 2);
            var rawUnit2 = new RawUnit(BaseUnitType.Mass, 1);

            // Act
            var result = RawUnitsSimplifier.CalculateDimensionalFormula(rawUnit1, rawUnit2);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(new Fraction(2), result[BaseUnitType.Length]);
            Assert.Equal(new Fraction(1), result[BaseUnitType.Mass]);
        }

        [Fact]
        public void CalculateDimensionalFormula_FractionalExponents_HandlesCorrectly()
        {
            // Arrange - Square root of area (m²)^(1/2) = m
            var rawUnit = new RawUnit(BaseUnitType.Length, new Fraction(1, 2));

            // Act
            var result = RawUnitsSimplifier.SimplifyFormula(rawUnit);

            // Assert
            Assert.Single(result);
            Assert.Equal(new Fraction(1, 2), result[BaseUnitType.Length]);
        }

        // Helper methods to create test units
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

            // Force = kg·m·s^-2
            baseUnit.RawUnits.Add(new RawUnit(BaseUnitType.Mass, 1));
            baseUnit.RawUnits.Add(new RawUnit(BaseUnitType.Length, 1));
            baseUnit.RawUnits.Add(new RawUnit(BaseUnitType.Time, -2));

            return baseUnit;
        }

        private BaseUnit CreateSpeedBaseUnit()
        {
            var baseUnit = new BaseUnit
            {
                Name = "MeterPerSecond",
                Symbol = "m/s",
                UnitType = UnitType.Speed_Mech,
                UnitSystem = StandardUnitSystem.SI,
                IsSI = true,
                Prefix = Prefix.SI,
                Exponent = new Fraction(1)
            };

            // Speed = m·s^-1
            baseUnit.RawUnits.Add(new RawUnit(BaseUnitType.Length, 1));
            baseUnit.RawUnits.Add(new RawUnit(BaseUnitType.Time, -1));

            return baseUnit;
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
    }
}