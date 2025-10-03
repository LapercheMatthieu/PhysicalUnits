using Fractions;
using MatthL.PhysicalUnits.Computation.Extensions;
using MatthL.PhysicalUnits.Core.Enums;
using MatthL.PhysicalUnits.Core.EquationModels;
using MatthL.PhysicalUnits.Core.Models;
using MatthL.PhysicalUnits.Infrastructure.Extensions;
using Xunit;

namespace MatthL.PhysicalUnits.Tests.Computation
{
    public class PhysicalUnitComputationExtensionsTests
    {
        [Fact]
        public void Divide_TwoUnits_CreatesCorrectResult()
        {
            // Arrange - Speed = Length / Time
            var length = new PhysicalUnit(CreateLengthBaseUnit());
            var time = new PhysicalUnit(CreateTimeBaseUnit());

            // Act
            var speed = length.Divide(time);

            // Assert
            Assert.Equal(2, speed.BaseUnits.Count);
            var lengthUnit = speed.BaseUnits.First(b => b.Symbol == "m");
            var timeUnit = speed.BaseUnits.First(b => b.Symbol == "s");

            Assert.Equal(new Fraction(1), lengthUnit.Exponent);
            Assert.Equal(new Fraction(-1), timeUnit.Exponent);
        }

        [Fact]
        public void Divide_ComplexUnits_HandlesCorrectly()
        {
            // Arrange - Energy / Time = Power
            var energy = CreateEnergyPhysicalUnit();
            var time = new PhysicalUnit(CreateTimeBaseUnit());

            // Act
            var power = energy.Divide(time);

            // Assert - Power should be kg·m²·s^-3
            var simplified = power.Simplify();
            Assert.Equal(3, simplified.BaseUnits.Count);

            var massUnit = simplified.BaseUnits.First(b => b.Symbol == "kg");
            Assert.Equal(1, massUnit.Exponent.ToDouble(), 2);

            var lengthUnit = simplified.BaseUnits.First(b => b.Symbol == "m");
            Assert.Equal(2, lengthUnit.Exponent.ToDouble(), 2);

            var timeUnit = simplified.BaseUnits.First(b => b.Symbol == "s");
            Assert.Equal(-3, timeUnit.Exponent.ToDouble(), 2);
        }

        [Fact]
        public void Multiply_WithSingleTerm_CreatesComposite()
        {
            // Arrange - Force × Distance = Energy
            var force = CreateForcePhysicalUnit();
            var distance = new PhysicalUnit(CreateLengthBaseUnit());
            var distanceTerm = new PhysicalUnitTerm(distance, new Fraction(1));

            // Act
            var energy = force.Multiply(distanceTerm);

            // Assert - Energy should be kg·m²·s^-2
            var simplified = energy.Simplify();
            Assert.Equal(3, simplified.BaseUnits.Count);

            var lengthUnit = simplified.BaseUnits.First(b => b.Symbol == "m");
            Assert.Equal(2, lengthUnit.Exponent.ToDouble(), 2);
        }

        [Fact]
        public void Multiply_WithMultipleTerms_CombinesAll()
        {
            // Arrange - Mass × Acceleration = Force (m·a = F)
            var mass = new PhysicalUnit(CreateMassBaseUnit());
            var acceleration = CreateAccelerationPhysicalUnit();

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
            var energy = CreateEnergyPhysicalUnit();
            var time = new PhysicalUnit(CreateTimeBaseUnit());
            var timeTerm = new PhysicalUnitTerm(time, new Fraction(-1));

            // Act
            var power = energy.Multiply(timeTerm);

            // Assert - Power should be kg·m²·s^-3
            var simplified = power.Simplify();
            var timeUnit = simplified.BaseUnits.First(b => b.Symbol == "s");
            Assert.Equal(-3, timeUnit.Exponent.ToDouble(), 2);
        }

        [Fact]
        public void Multiply_WithFractionalExponent_HandlesCorrectly()
        {
            // Arrange - Length^(1/2)
            var area = CreateAreaPhysicalUnit();
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
            var length1 = new PhysicalUnit(CreateLengthBaseUnit());
            var length2 = new PhysicalUnit(CreateLengthBaseUnit());
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
            var length = new PhysicalUnit(CreateLengthBaseUnit());
            var mass = new PhysicalUnit(CreateMassBaseUnit());
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
            var length = new PhysicalUnit(CreateLengthBaseUnit());

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
            var area = CreateAreaPhysicalUnit();

            // Act
            var length = area.Pow(new Fraction(1, 2));

            // Assert
            var simplified = length.Simplify();
            Assert.Single(simplified.BaseUnits);
            Assert.Equal(1, simplified.BaseUnits.First().Exponent.ToDouble(), 2);
        }

        [Fact]
        public void Pow_NegativeExponent_InvertsUnit()
        {
            // Arrange - m^-1
            var length = new PhysicalUnit(CreateLengthBaseUnit());

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
            var length = new PhysicalUnit(CreateLengthBaseUnit());

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
            var force = CreateForcePhysicalUnit();

            // Act
            var squared = force.Pow(new Fraction(2));

            // Assert
            var simplified = squared.Simplify();
            Assert.Equal(3, simplified.BaseUnits.Count);

            var massUnit = simplified.BaseUnits.First(b => b.Symbol == "kg");
            Assert.Equal(2, massUnit.Exponent.ToDouble(), 2);

            var lengthUnit = simplified.BaseUnits.First(b => b.Symbol == "m");
            Assert.Equal(2, lengthUnit.Exponent.ToDouble(), 2);

            var timeUnit = simplified.BaseUnits.First(b => b.Symbol == "s");
            Assert.Equal(-4, timeUnit.Exponent.ToDouble(), 2);
        }

        [Fact]
        public void Multiply_ChainedOperations_CombinesCorrectly()
        {
            // Arrange - F × d × t (Force × distance × time)
            var force = CreateForcePhysicalUnit();
            var distance = new PhysicalUnit(CreateLengthBaseUnit());
            var time = new PhysicalUnit(CreateTimeBaseUnit());

            var distanceTerm = new PhysicalUnitTerm(distance, new Fraction(1));
            var timeTerm = new PhysicalUnitTerm(time, new Fraction(1));

            // Act
            var result = force.Multiply(distanceTerm, timeTerm);

            // Assert - Should be kg·m²·s^-1
            var simplified = result.Simplify();
            Assert.Equal(3, simplified.BaseUnits.Count);

            var timeUnit = simplified.BaseUnits.First(b => b.Symbol == "s");
            Assert.Equal(-1, timeUnit.Exponent.ToDouble(), 2);
        }

        // Helper methods to create test units
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

        private PhysicalUnit CreateForcePhysicalUnit()
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
            return new PhysicalUnit(baseUnit);
        }

        private PhysicalUnit CreateEnergyPhysicalUnit()
        {
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
            return new PhysicalUnit(baseUnit);
        }

        private PhysicalUnit CreateAccelerationPhysicalUnit()
        {
            var baseUnit = new BaseUnit
            {
                Name = "MeterPerSecondSquared",
                Symbol = "m/s²",
                UnitType = UnitType.Acceleration_Mech,
                UnitSystem = StandardUnitSystem.SI,
                IsSI = true,
                Prefix = Prefix.SI,
                Exponent = new Fraction(1)
            };
            baseUnit.RawUnits.Add(new RawUnit(BaseUnitType.Length, 1));
            baseUnit.RawUnits.Add(new RawUnit(BaseUnitType.Time, -2));
            return new PhysicalUnit(baseUnit);
        }

        private PhysicalUnit CreateAreaPhysicalUnit()
        {
            var baseUnit = new BaseUnit
            {
                Name = "SquareMeter",
                Symbol = "m²",
                UnitType = UnitType.Area_Mech,
                UnitSystem = StandardUnitSystem.SI,
                IsSI = true,
                Prefix = Prefix.SI,
                Exponent = new Fraction(1)
            };
            baseUnit.RawUnits.Add(new RawUnit(BaseUnitType.Length, 2));
            return new PhysicalUnit(baseUnit);
        }
    }
}