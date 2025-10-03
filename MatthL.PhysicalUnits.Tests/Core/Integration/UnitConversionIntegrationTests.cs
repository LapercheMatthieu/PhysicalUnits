using Fractions;
using MatthL.PhysicalUnits.Core.EnumHelpers;
using MatthL.PhysicalUnits.Core.Enums;
using MatthL.PhysicalUnits.Core.Models;
using System.Numerics;
using Xunit;

namespace MatthL.PhysicalUnits.Tests.Core.Integration
{
    public class UnitConversionIntegrationTests
    {
        [Fact]
        public void ConvertKilometersToMeters_UsingPrefixConversion()
        {
            // Arrange
            var valueInKm = 5.5m;
            var fromPrefix = Prefix.kilo;
            var toPrefix = Prefix.SI;

            // Act
            var valueInMeters = PrefixHelper.Convert(valueInKm, fromPrefix, toPrefix);

            // Assert
            Assert.Equal(5500m, valueInMeters);
        }

        [Fact]
        public void ConvertMillimetersToKilometers_MultipleSteps()
        {
            // Arrange
            var valueInMm = 5000000m;
            var fromPrefix = Prefix.milli;
            var toPrefix = Prefix.kilo;

            // Act
            var valueInKm = PrefixHelper.Convert(valueInMm, fromPrefix, toPrefix);

            // Assert
            Assert.Equal(5m, valueInKm);
        }

        [Fact]
        public void CreateNewtonUnit_WithAllComponents()
        {
            // Arrange & Act - Newton = kg·m·s⁻²
            var newton = new BaseUnit
            {
                Name = "Newton",
                Symbol = "N",
                UnitType = UnitType.Force_Mech,
                UnitSystem = StandardUnitSystem.SI,
                IsSI = true,
                Prefix = Prefix.SI,
                Exponent = new Fraction(1, 1),
                ConversionFactor = new Fraction(1, 1),
                Offset = 0
            };

            newton.RawUnits.Add(new RawUnit(BaseUnitType.Mass, 1));
            newton.RawUnits.Add(new RawUnit(BaseUnitType.Length, 1));
            newton.RawUnits.Add(new RawUnit(BaseUnitType.Time, -2));

            // Assert
            Assert.Equal("N", newton.ToString());
            Assert.Equal(3, newton.RawUnits.Count);
            Assert.True(newton.IsSI);
            Assert.Equal(PhysicalUnitDomain.Mechanics, newton.Domain);
        }

        [Fact]
        public void CreateKilonewtonUnit_WithPrefixConversion()
        {
            // Arrange
            var kilonewton = new BaseUnit
            {
                Name = "Newton",
                Symbol = "N",
                UnitType = UnitType.Force_Mech,
                Prefix = Prefix.kilo,
                ConversionFactor = new Fraction(1000, 1)
            };

            // Act
            var displayName = kilonewton.PrefixedSymbol;
            var factor = kilonewton.ConversionFactor;

            // Assert
            Assert.Equal("kN", displayName);
            Assert.Equal(new Fraction(1000, 1), factor);
        }

        [Fact]
        public void CreateVelocityUnit_MetersPerSecond()
        {
            // Arrange - m/s
            var meter = new BaseUnit
            {
                Symbol = "m",
                Prefix = Prefix.SI,
                Exponent = new Fraction(1, 1),
                IsSI = true,
                UnitSystem = StandardUnitSystem.SI
            };
            meter.RawUnits.Add(new RawUnit(BaseUnitType.Length, 1));

            var second = new BaseUnit
            {
                Symbol = "s",
                Prefix = Prefix.SI,
                Exponent = new Fraction(-1, 1),
                IsSI = true,
                UnitSystem = StandardUnitSystem.SI
            };
            second.RawUnits.Add(new RawUnit(BaseUnitType.Time, -1));

            var velocity = new PhysicalUnit { UnitType = UnitType.Speed_Mech };
            velocity.BaseUnits.Add(meter);
            velocity.BaseUnits.Add(second);

            // Assert
            Assert.Equal(2, velocity.BaseUnits.Count);
            Assert.True(velocity.IsSI);
            Assert.NotEmpty(velocity.ToString());
        }

        [Fact]
        public void CreateAccelerationUnit_MetersPerSecondSquared()
        {
            // Arrange - m/s²
            var meter = new BaseUnit
            {
                Symbol = "m",
                Prefix = Prefix.SI,
                Exponent = new Fraction(1, 1),
                IsSI = true
            };
            meter.RawUnits.Add(new RawUnit(BaseUnitType.Length, 1));

            var secondSquared = new BaseUnit
            {
                Symbol = "s",
                Prefix = Prefix.SI,
                Exponent = new Fraction(-2, 1),
                IsSI = true
            };
            secondSquared.RawUnits.Add(new RawUnit(BaseUnitType.Time, -2));

            var acceleration = new PhysicalUnit { UnitType = UnitType.Acceleration_Mech };
            acceleration.BaseUnits.Add(meter);
            acceleration.BaseUnits.Add(secondSquared);

            // Act
            var exponentOfTime = acceleration.BaseUnits.Last().Exponent;

            // Assert
            Assert.Equal(new Fraction(-2, 1), exponentOfTime);
            Assert.Equal("m", acceleration.BaseUnits.First().Symbol);
            Assert.Equal("s", acceleration.BaseUnits.Last().Symbol);
        }

        [Fact]
        public void ConversionFactor_LargeNumbers_BigInteger()
        {
            // Arrange
            var unit = new BaseUnit();
            var veryLargeNumber = BigInteger.Parse("999999999999999999999999999999");
            var factor = new Fraction(veryLargeNumber, 1);

            // Act
            unit.ConversionFactor = factor;
            var retrieved = unit.ConversionFactor;

            // Assert
            Assert.Equal(veryLargeNumber, retrieved.Numerator);
        }

        [Fact]
        public void CreatePressureUnit_PascalFromNewton()
        {
            // Arrange - Pascal = N/m² = kg/(m·s²)
            var pascal = new BaseUnit
            {
                Name = "Pascal",
                Symbol = "Pa",
                UnitType = UnitType.Pressure_Mech,
                IsSI = true,
                Prefix = Prefix.SI
            };

            pascal.RawUnits.Add(new RawUnit(BaseUnitType.Mass, 1));
            pascal.RawUnits.Add(new RawUnit(BaseUnitType.Length, -1));
            pascal.RawUnits.Add(new RawUnit(BaseUnitType.Time, -2));

            // Assert
            Assert.Equal(3, pascal.RawUnits.Count);
            Assert.Contains(pascal.RawUnits, r => r.UnitType == BaseUnitType.Mass && r.Exponent == new Fraction(1, 1));
            Assert.Contains(pascal.RawUnits, r => r.UnitType == BaseUnitType.Length && r.Exponent == new Fraction(-1, 1));
            Assert.Contains(pascal.RawUnits, r => r.UnitType == BaseUnitType.Time && r.Exponent == new Fraction(-2, 1));
        }

        [Fact]
        public void ChainedConversions_MicroToGiga()
        {
            // Arrange
            var value = 1m;

            // Act - Convert 1 microsecond to gigaseconds
            var result = PrefixHelper.Convert(value, Prefix.micro, Prefix.giga);

            // Assert
            Assert.Equal(1e-15m, result);
        }

        [Fact]
        public void ComplexUnit_EnergyDensity_JoulePerCubicMeter()
        {
            // Arrange - J/m³ = kg/(m·s²)
            var joule = new BaseUnit
            {
                Symbol = "J",
                Prefix = Prefix.SI,
                Exponent = new Fraction(1, 1),
                IsSI = true
            };
            joule.RawUnits.Add(new RawUnit(BaseUnitType.Mass, 1));
            joule.RawUnits.Add(new RawUnit(BaseUnitType.Length, 2));
            joule.RawUnits.Add(new RawUnit(BaseUnitType.Time, -2));

            var cubicMeter = new BaseUnit
            {
                Symbol = "m",
                Prefix = Prefix.SI,
                Exponent = new Fraction(-3, 1),
                IsSI = true
            };
            cubicMeter.RawUnits.Add(new RawUnit(BaseUnitType.Length, -3));

            var energyDensity = new PhysicalUnit { UnitType = UnitType.PowerDensity_Mech };
            energyDensity.BaseUnits.Add(joule);
            energyDensity.BaseUnits.Add(cubicMeter);

            // Assert
            Assert.Equal(2, energyDensity.BaseUnits.Count);
            Assert.True(energyDensity.IsSI);
        }

        [Theory]
        [InlineData(100, Prefix.centi, Prefix.SI, 1)]
        [InlineData(1000, Prefix.milli, Prefix.SI, 1)]
        [InlineData(1, Prefix.kilo, Prefix.milli, 1000000)]
        [InlineData(1, Prefix.giga, Prefix.mega, 1000)]
        public void PrefixConversion_CommonScenarios(decimal input, Prefix from, Prefix to, decimal expected)
        {
            // Act
            var result = PrefixHelper.Convert(input, from, to);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void TemperatureUnit_WithOffset_CelsiusToKelvin()
        {
            // Arrange
            var celsius = new BaseUnit
            {
                Name = "Celsius",
                Symbol = "°C",
                UnitType = UnitType.Temperature_Base,
                Offset = 273.15, // Offset for Celsius to Kelvin
                ConversionFactor = new Fraction(1, 1)
            };

            // Assert
            Assert.Equal(273.15, celsius.Offset);
            Assert.Equal(new Fraction(1, 1), celsius.ConversionFactor);
        }

        [Fact]
        public void FractionalExponent_SquareRoot()
        {
            // Arrange - sqrt(m) = m^(1/2)
            var sqrtMeter = new BaseUnit
            {
                Symbol = "m",
                Prefix = Prefix.SI,
                Exponent = new Fraction(1, 2)
            };
            sqrtMeter.RawUnits.Add(new RawUnit(BaseUnitType.Length, new Fraction(1, 2)));

            // Act
            var displayString = sqrtMeter.ToString();

            // Assert
            Assert.Equal(new Fraction(1, 2), sqrtMeter.Exponent);
            Assert.Contains("¹ᐟ²", displayString);
        }
    }
}