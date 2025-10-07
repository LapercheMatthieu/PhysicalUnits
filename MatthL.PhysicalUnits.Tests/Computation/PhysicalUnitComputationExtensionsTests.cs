using Fractions;
using MatthL.PhysicalUnits.Computation.Extensions;
using MatthL.PhysicalUnits.Core.Enums;
using MatthL.PhysicalUnits.Core.EquationModels;
using MatthL.PhysicalUnits.Core.Models;
using MatthL.PhysicalUnits.DimensionalFormulas.Extensions;
using MatthL.PhysicalUnits.DimensionalFormulas.Helpers;
using MatthL.PhysicalUnits.Infrastructure.Extensions;
using MatthL.PhysicalUnits.Infrastructure.Library;
using MatthL.PhysicalUnits.Infrastructure.Repositories;
using Xunit;

namespace MatthL.PhysicalUnits.Tests.Computation
{
    public class PhysicalUnitComputationExtensionsTests
    {
        public PhysicalUnitComputationExtensionsTests()
        {
            PhysicalUnitRepository.Initialize();
        }

        [Fact]
        public void Divide_TwoUnits_CreatesCorrectResult()
        {
            // Arrange - Speed = Length / Time
            var length = StandardUnits.Meter();
            var time = StandardUnits.Second() ;

            // Act
            var speed = length.Divide(time);

            // Assert
            var simplified = RawUnitsSimplifier.CalculateDimensionalFormula(speed);
            Assert.Equal(1, simplified[BaseUnitType.Length]);
            Assert.Equal(-1, simplified[BaseUnitType.Time]);
        }

        [Fact]
        public void Divide_ComplexUnits_HandlesCorrectly()
        {
            // Arrange - Energy / Time = Power
            var energy = StandardUnits.Joule();
            var time = StandardUnits.Second();

            // Act
            var power = energy.Divide(time);

            // Assert - Power should be kg·m²·s^-3
            var simplified = RawUnitsSimplifier.CalculateDimensionalFormula(power);
            Assert.Equal(1, simplified[BaseUnitType.Mass]);
            Assert.Equal(2, simplified[BaseUnitType.Length]);
            Assert.Equal(-3, simplified[BaseUnitType.Time]);
        }

        [Fact]
        public void Multiply_WithSingleTerm_CreatesComposite()
        {
            // Arrange - Force × Distance = Energy
            var force = StandardUnits.Newton();
            var distance = StandardUnits.Meter();
            var distanceTerm = new PhysicalUnitTerm(distance, new Fraction(1));

            // Act
            var energy = force.Multiply(distanceTerm);

            // Assert - Energy should be kg·m²·s^-2
            var simplified = RawUnitsSimplifier.CalculateDimensionalFormula(energy);
            Assert.Equal(1, simplified[BaseUnitType.Mass]);
            Assert.Equal(2, simplified[BaseUnitType.Length]);
            Assert.Equal(-2, simplified[BaseUnitType.Time]);
        }

        [Fact]
        public void Multiply_WithMultipleTerms_CombinesAll()
        {
            // Arrange - Mass × Acceleration = Force (m·a = F)
            var mass = StandardUnits.Kilogram();
            var acceleration = StandardUnits.MeterPerSecondSquared;

            var accelerationTerm = new PhysicalUnitTerm(acceleration, new Fraction(1));

            // Act
            var force = mass.Multiply(accelerationTerm);

            // Assert - Force should be kg·m·s^-2
            var simplified = force.Simplify();
            Assert.Equal(3, simplified.BaseUnits.Count);

            var massUnit = simplified.BaseUnits.First(b => b.Symbol == "kg");
            Assert.Equal(1, massUnit.Exponent.ToDouble(), 2);

            var lengthUnit = simplified.BaseUnits.First(b => b.Symbol == "m");
            Assert.Equal(1, lengthUnit.Exponent.ToDouble(), 2);

            var timeUnit = simplified.BaseUnits.First(b => b.Symbol == "s");
            Assert.Equal(-2, timeUnit.Exponent.ToDouble(), 2);
        }

        [Fact]
        public void Multiply_WithNegativeExponent_PerformsDivision()
        {
            // Arrange - Energy / Time
            var energy = StandardUnits.Joule();
            var time = StandardUnits.Second();
            var timeTerm = new PhysicalUnitTerm(time, new Fraction(-1));

            // Act
            var power = energy.Multiply(timeTerm);

            // Assert - Power should be kg·m²·s^-3
            var simplified = RawUnitsSimplifier.CalculateDimensionalFormula(power);
            Assert.Equal(1, simplified[BaseUnitType.Mass]);
            Assert.Equal(2, simplified[BaseUnitType.Length]);
            Assert.Equal(-3, simplified[BaseUnitType.Time]);
        }

        [Fact]
        public void Multiply_WithFractionalExponent_HandlesCorrectly()
        {
            // Arrange - Length^(1/2)
            var area = StandardUnits.SquareMeter;
            var areaTerm = new PhysicalUnitTerm(area, new Fraction(1, 2));

            // Act
            var result = new PhysicalUnit().Multiply(areaTerm);

            // Assert - Should give m^1
            var simplified = result.Simplify();
            var lengthUnit = simplified.BaseUnits.First(b => b.Symbol == "m");
            Assert.Equal(1, lengthUnit.Exponent.ToDouble(), 2);
        }

        [Fact]
        public void Multiply_SimplifiesDuplicates_CorrectlyCombinesExponents()
        {
            // Arrange - m × m = m²
            var length1 = StandardUnits.Meter(); 
            var length2 = StandardUnits.Meter();
            var length2Term = new PhysicalUnitTerm(length2, new Fraction(1));

            // Act
            var area = length1.Multiply(length2Term);

            // Assert
            var simplified = area.Simplify();
            Assert.Single(simplified.BaseUnits);
            Assert.Equal(2, simplified.BaseUnits.First().Exponent.ToDouble(), 2);
        }

        [Fact]
        public void Multiply_WithZeroExponent_DoesNotAffectResult()
        {
            // Arrange
            var length = StandardUnits.Meter();
            var mass = StandardUnits.Kilogram();
            var massTerm = new PhysicalUnitTerm(mass, new Fraction(0));

            // Act
            var result = length.Multiply(massTerm);

            // Assert - Should still be just length since mass^0 = 1
            var simplified = result.Simplify();
            Assert.Single(simplified.BaseUnits);
            Assert.Equal("m", simplified.BaseUnits.First().Symbol);
        }

        [Fact]
        public void Pow_IntegerExponent_RaisesCorrectly()
        {
            // Arrange - m^2
            var length = StandardUnits.Meter();

            // Act
            var area = length.Pow(new Fraction(2));

            // Assert
            var simplified = area.Simplify();
            Assert.Single(simplified.BaseUnits);
            Assert.Equal(2, simplified.BaseUnits.First().Exponent.ToDouble(), 2);
        }

        [Fact]
        public void Pow_FractionalExponent_RaisesCorrectly()
        {
            // Arrange - m²^(1/2) = m

            var area = StandardUnits.SquareMeter;

            // Act
            var length = area.Pow(new Fraction(1, 2));

            // Assert
           // var simplified = length.Simplify();
            Assert.Single(length.BaseUnits);
            Assert.Equal(1, length.BaseUnits.First().Exponent.ToDouble(), 2);
        }

        [Fact]
        public void Pow_NegativeExponent_InvertsUnit()
        {
            // Arrange - m^-1
            var length = StandardUnits.Meter();

            // Act
            var inverse = length.Pow(new Fraction(-1));

            // Assert
            var simplified = inverse.Simplify();
            Assert.Single(simplified.BaseUnits);
            Assert.Equal(-1, simplified.BaseUnits.First().Exponent.ToDouble(), 2);
        }

        [Fact]
        public void Pow_ZeroExponent_CreatesDimensionless()
        {
            // Arrange
            var length = StandardUnits.Meter();

            // Act
            var dimensionless = length.Pow(new Fraction(0));

            // Assert
            var simplified = dimensionless.Simplify();
            Assert.Empty(simplified.BaseUnits);
        }

        [Fact]
        public void Pow_ComplexUnit_RaisesAllExponents()
        {
            // Arrange - (kg·m/s²)² = kg²·m²/s⁴
            var force = StandardUnits.Newton();

            // Act
            var squared = force.Pow(new Fraction(2));

            var dimension = RawUnitsSimplifier.CalculateDimensionalFormula(squared);

            // Assert
            Assert.Equal(2,dimension[BaseUnitType.Mass]);
            Assert.Equal(2,dimension[BaseUnitType.Length]);
            Assert.Equal(-4,dimension[BaseUnitType.Time]);
        }

        [Fact]
        public void Multiply_ChainedOperations_CombinesCorrectly()
        {
            // Arrange - F × d × t (Force × distance × time)
            var force = StandardUnits.Newton();
            var distance = StandardUnits.Meter();
            var time = StandardUnits.Second();

            var distanceTerm = new PhysicalUnitTerm(distance, new Fraction(1));
            var timeTerm = new PhysicalUnitTerm(time, new Fraction(1));

            // Act
            var result = force.Multiply(distanceTerm, timeTerm);

            // Assert - Should be kg·m²·s^-1
            var simplified = RawUnitsSimplifier.CalculateDimensionalFormula(result);
            Assert.Equal(1, simplified[BaseUnitType.Mass]);
            Assert.Equal(2, simplified[BaseUnitType.Length]);
            Assert.Equal(-1, simplified[BaseUnitType.Time]);
        }

        
    }
}