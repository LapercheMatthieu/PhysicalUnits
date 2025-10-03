using Fractions;
using MatthL.PhysicalUnits.Core.Enums;
using MatthL.PhysicalUnits.Core.EquationModels;
using MatthL.PhysicalUnits.Core.Models;
using MatthL.PhysicalUnits.DimensionalFormulas.Extensions;
using MatthL.PhysicalUnits.Infrastructure.Library;
using Xunit;

namespace MatthL.PhysicalUnits.Tests.DimensionalFormulas
{
    public class DimensionalFormulasExtensionTests
    {
        [Fact]
        public void GetDimensionalFormula_PhysicalUnit_ReturnsCorrectFormula()
        {
            // Arrange - Speed (m/s)
            var baseUnit = StandardUnits.MeterPerSecond;
            var physicalUnit = new PhysicalUnit(baseUnit);

            // Act
            var result = physicalUnit.GetDimensionalFormula();

            // Assert
            Assert.Equal("m/s", result);
        }

        [Fact]
        public void GetDimensionalFormula_BaseUnit_ReturnsCorrectFormula()
        {
            // Arrange - Force (kg·m/s²)
            var baseUnit = StandardUnits.Newton();

            // Act
            var result = baseUnit.GetDimensionalFormula();

            // Assert
            Assert.Equal("kg·m/s²", result);
        }

        [Fact]
        public void GetDimensionalFormula_RawUnit_ReturnsSymbolWithExponent()
        {
            // Arrange
            var rawUnit = new RawUnit(BaseUnitType.Length, 2);

            // Act
            var result = rawUnit.GetDimensionalFormula();

            // Assert
            Assert.Equal("m²", result);
        }

        [Fact]
        public void GetDimensionalFormula_PhysicalUnitTerm_AppliesExponent()
        {
            // Arrange - Force^2
            var physicalUnit = StandardUnits.Newton();
            var term = new PhysicalUnitTerm(physicalUnit, new Fraction(2));

            // Act
            var result = term.GetDimensionalFormula();

            // Assert
            // Force = kg·m/s², Force² = kg²·m²/s⁴
            Assert.Equal("kg²·m²/s⁴", result);
        }

        [Fact]
        public void GetDimensionalFormula_EquationTerms_CombinesMultipleTerms()
        {
            // Arrange - Force × Distance = Energy
            var force = StandardUnits.Newton();
            var distance = StandardUnits.Meter();

            var terms = new EquationTerms(
                new PhysicalUnitTerm(force, new Fraction(1)),
                new PhysicalUnitTerm(distance, new Fraction(1))
            );

            // Act
            var result = terms.GetDimensionalFormula();

            // Assert
            // Force × Distance = kg·m/s² × m = kg·m²/s²
            Assert.Equal("kg·m²/s²", result);
        }

        [Fact]
        public void GetDimensionalFormula_ComplexPhysicalUnit_SimplifiesCorrectly()
        {
            // Arrange - Energy (kg·m²/s²)
            var baseUnit = new BaseUnit
            {
                Name = "Joule",
                Symbol = "J",
                UnitType = UnitType.Energy_Mech,
                UnitSystem = StandardUnitSystem.SI,
                IsSI = true,
                Prefix = Prefix.SI,
                Exponent = new Fraction(1)
            };

            baseUnit.RawUnits.Add(new RawUnit(BaseUnitType.Mass, 1));
            baseUnit.RawUnits.Add(new RawUnit(BaseUnitType.Length, 2));
            baseUnit.RawUnits.Add(new RawUnit(BaseUnitType.Time, -2));

            var physicalUnit = new PhysicalUnit(baseUnit);

            // Act
            var result = physicalUnit.GetDimensionalFormula();

            // Assert
            Assert.Equal("kg·m²/s²", result);
        }

        [Fact]
        public void GetDimensionalFormula_DivisionEquation_HandlesNegativeExponents()
        {
            // Arrange - Energy / Time = Power
            var energy = StandardUnits.Joule();
            var time = StandardUnits.Second();

            var terms = new EquationTerms(
                new PhysicalUnitTerm(energy, new Fraction(1)),
                new PhysicalUnitTerm(time, new Fraction(-1))
            );

            // Act
            var result = terms.GetDimensionalFormula();

            // Assert
            // Energy/Time = kg·m²/s² / s = kg·m²/s³
            Assert.Equal("kg·m²/s³", result);
        }

        [Fact]
        public void GetDimensionalFormula_SquareRoot_HandlesFractionalExponent()
        {
            // Arrange - sqrt(Area) = Length
            var area = StandardUnits.SquareMeter;
            var term = new PhysicalUnitTerm(area, new Fraction(1, 2));

            // Act
            var result = term.GetDimensionalFormula();

            // Assert
            Assert.Equal("m", result);
        }

        [Fact]
        public void GetDimensionalFormula_CancellingTerms_SimplifiesToDimensionless()
        {
            // Arrange - Length / Length = dimensionless
            var length1 = StandardUnits.Meter();
            var length2 = StandardUnits.Meter();

            var terms = new EquationTerms(
                new PhysicalUnitTerm(length1, new Fraction(1)),
                new PhysicalUnitTerm(length2, new Fraction(-1))
            );

            // Act
            var result = terms.GetDimensionalFormula();

            // Assert
            Assert.Equal("", result); // Empty string for dimensionless
        }

        [Fact]
        public void GetDimensionalFormula_EmptyBaseUnit_ReturnsEmptyString()
        {
            // Arrange
            var baseUnit = new BaseUnit
            {
                Name = "Dimensionless",
                Symbol = "",
                UnitType = UnitType.Ratio_Base,
                UnitSystem = StandardUnitSystem.SI,
                IsSI = true,
                Prefix = Prefix.SI,
                Exponent = new Fraction(1)
            };

            // Act
            var result = baseUnit.GetDimensionalFormula();

            // Assert
            Assert.Equal("", result);
        }

    }
}