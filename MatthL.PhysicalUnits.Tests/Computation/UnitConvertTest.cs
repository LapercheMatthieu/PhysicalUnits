using Fractions;
using MatthL.PhysicalUnits.Computation.Converters;
using MatthL.PhysicalUnits.Core.Enums;
using MatthL.PhysicalUnits.Core.Models;
using Xunit;

namespace MatthL.PhysicalUnits.Tests.Converters
{
    public class UnitConverterTests
    {
        [Fact]
        public void ConvertToSIValue_SimpleUnit_ConvertsCorrectly()
        {
            // Arrange - kilometer to meter
            var kilometer = CreateLengthBaseUnit();
            kilometer.Prefix = Prefix.kilo;
            var physicalUnit = new PhysicalUnit(kilometer);

            // Act
            var result = physicalUnit.ConvertToSIValue(5.0); // 5 km = 5000 m

            // Assert
            Assert.Equal(5000.0, result, 2);
        }

        [Fact]
        public void ConvertToSIValue_Millimeter_ConvertsCorrectly()
        {
            // Arrange
            var millimeter = CreateLengthBaseUnit();
            millimeter.Prefix = Prefix.milli;
            var physicalUnit = new PhysicalUnit(millimeter);

            // Act
            var result = physicalUnit.ConvertToSIValue(1000.0); // 1000 mm = 1 m

            // Assert
            Assert.Equal(1.0, result, 2);
        }

        [Fact]
        public void ConvertFromSIValue_SimpleUnit_ConvertsCorrectly()
        {
            // Arrange - meter to kilometer
            var kilometer = CreateLengthBaseUnit();
            kilometer.Prefix = Prefix.kilo;
            var physicalUnit = new PhysicalUnit(kilometer);

            // Act
            var result = physicalUnit.ConvertFromSIValue(5000.0); // 5000 m = 5 km

            // Assert
            Assert.Equal(5.0, result, 2);
        }

        [Fact]
        public void ConvertFromSIValue_Millimeter_ConvertsCorrectly()
        {
            // Arrange
            var millimeter = CreateLengthBaseUnit();
            millimeter.Prefix = Prefix.milli;
            var physicalUnit = new PhysicalUnit(millimeter);

            // Act
            var result = physicalUnit.ConvertFromSIValue(1.0); // 1 m = 1000 mm

            // Assert
            Assert.Equal(1000.0, result, 2);
        }

        [Fact]
        public void ConvertToSIValue_WithExponent_ConvertsCorrectly()
        {
            // Arrange - square kilometer to square meter (km² to m²)
            var squareKilometer = CreateLengthBaseUnit();
            squareKilometer.Prefix = Prefix.kilo;
            squareKilometer.Exponent = new Fraction(2);
            var physicalUnit = new PhysicalUnit(squareKilometer);

            // Act
            var result = physicalUnit.ConvertToSIValue(1.0); // 1 km² = 1,000,000 m²

            // Assert
            Assert.Equal(1000000.0, result, 2);
        }

        [Fact]
        public void ConvertFromSIValue_WithExponent_ConvertsCorrectly()
        {
            // Arrange
            var squareKilometer = CreateLengthBaseUnit();
            squareKilometer.Prefix = Prefix.kilo;
            squareKilometer.Exponent = new Fraction(2);
            var physicalUnit = new PhysicalUnit(squareKilometer);

            // Act
            var result = physicalUnit.ConvertFromSIValue(1000000.0); // 1,000,000 m² = 1 km²

            // Assert
            Assert.Equal(1.0, result, 2);
        }

        [Fact]
        public void ConvertToSIValues_Array_ConvertsAllElements()
        {
            // Arrange
            var kilometer = CreateLengthBaseUnit();
            kilometer.Prefix = Prefix.kilo;
            var physicalUnit = new PhysicalUnit(kilometer);
            var values = new double[] { 1.0, 2.0, 3.0, 4.0, 5.0 };

            // Act
            var results = physicalUnit.ConvertToSIValues(values);

            // Assert
            Assert.Equal(5, results.Length);
            Assert.Equal(1000.0, results[0], 2);
            Assert.Equal(2000.0, results[1], 2);
            Assert.Equal(3000.0, results[2], 2);
            Assert.Equal(4000.0, results[3], 2);
            Assert.Equal(5000.0, results[4], 2);
        }

        [Fact]
        public void ConvertFromSIValues_Array_ConvertsAllElements()
        {
            // Arrange
            var kilometer = CreateLengthBaseUnit();
            kilometer.Prefix = Prefix.kilo;
            var physicalUnit = new PhysicalUnit(kilometer);
            var values = new double[] { 1000.0, 2000.0, 3000.0, 4000.0, 5000.0 };

            // Act
            var results = physicalUnit.ConvertFromSIValues(values);

            // Assert
            Assert.Equal(5, results.Length);
            Assert.Equal(1.0, results[0], 2);
            Assert.Equal(2.0, results[1], 2);
            Assert.Equal(3.0, results[2], 2);
            Assert.Equal(4.0, results[3], 2);
            Assert.Equal(5.0, results[4], 2);
        }

        [Fact]
        public void ConvertValue_BetweenTwoUnits_ConvertsCorrectly()
        {
            // Arrange - km to mm
            var kilometer = CreateLengthBaseUnit();
            kilometer.Prefix = Prefix.kilo;
            var kmUnit = new PhysicalUnit(kilometer);

            var millimeter = CreateLengthBaseUnit();
            millimeter.Prefix = Prefix.milli;
            var mmUnit = new PhysicalUnit(millimeter);

            // Act
            var result = kmUnit.ConvertValue(mmUnit, 1.0); // 1 km = 1,000,000 mm

            // Assert
            Assert.Equal(1000000.0, result, 2);
        }

        [Fact]
        public void ConvertValue_SameUnit_ReturnsIdentical()
        {
            // Arrange
            var meter1 = new PhysicalUnit(CreateLengthBaseUnit());
            var meter2 = new PhysicalUnit(CreateLengthBaseUnit());

            // Act
            var result = meter1.ConvertValue(meter2, 42.0);

            // Assert
            Assert.Equal(42.0, result, 2);
        }

        [Fact]
        public void ConvertValues_Array_BetweenTwoUnits_ConvertsAllElements()
        {
            // Arrange
            var kilometer = CreateLengthBaseUnit();
            kilometer.Prefix = Prefix.kilo;
            var kmUnit = new PhysicalUnit(kilometer);

            var meter = CreateLengthBaseUnit();
            var mUnit = new PhysicalUnit(meter);

            var values = new double[] { 1.0, 2.0, 3.0 };

            // Act
            var results = kmUnit.ConvertValues(mUnit, values);

            // Assert
            Assert.Equal(3, results.Length);
            Assert.Equal(1000.0, results[0], 2);
            Assert.Equal(2000.0, results[1], 2);
            Assert.Equal(3000.0, results[2], 2);
        }

        [Fact]
        public void ConvertToSIValue_WithOffset_HandlesTemperature()
        {
            // Arrange - Celsius to Kelvin
            var celsius = CreateTemperatureBaseUnit();
            celsius.Offset = 273.15;
            var physicalUnit = new PhysicalUnit(celsius);

            // Act
            var result = physicalUnit.ConvertToSIValue(0.0); // 0°C = 273.15 K

            // Assert
            Assert.Equal(273.15, result, 2);
        }

        [Fact]
        public void ConvertFromSIValue_WithOffset_HandlesTemperature()
        {
            // Arrange - Kelvin to Celsius
            var celsius = CreateTemperatureBaseUnit();
            celsius.Offset = 273.15;
            var physicalUnit = new PhysicalUnit(celsius);

            // Act
            var result = physicalUnit.ConvertFromSIValue(273.15); // 273.15 K = 0°C

            // Assert
            Assert.Equal(0.0, result, 2);
        }

        [Fact]
        public void ConvertToSIValue_WithConversionFactor_ConvertsCorrectly()
        {
            // Arrange - inch to meter (1 inch = 0.0254 m)
            var inch = CreateLengthBaseUnit();
            inch.ConversionFactor = new Fraction(254, 10000);
            var physicalUnit = new PhysicalUnit(inch);

            // Act
            var result = physicalUnit.ConvertToSIValue(1.0); // 1 inch = 0.0254 m

            // Assert
            Assert.Equal(0.0254, result, 4);
        }

        [Fact]
        public void ConvertFromSIValue_WithConversionFactor_ConvertsCorrectly()
        {
            // Arrange - meter to inch
            var inch = CreateLengthBaseUnit();
            inch.ConversionFactor = new Fraction(254, 10000);
            var physicalUnit = new PhysicalUnit(inch);

            // Act
            var result = physicalUnit.ConvertFromSIValue(0.0254); // 0.0254 m = 1 inch

            // Assert
            Assert.Equal(1.0, result, 4);
        }

        [Fact]
        public void GetToSiFunction_ReturnsReusableFunction()
        {
            // Arrange
            var kilometer = CreateLengthBaseUnit();
            kilometer.Prefix = Prefix.kilo;
            var physicalUnit = new PhysicalUnit(kilometer);

            // Act
            var function = physicalUnit.GetToSiFunction();
            var result1 = function(1.0);
            var result2 = function(2.0);
            var result3 = function(3.0);

            // Assert
            Assert.Equal(1000.0, result1, 2);
            Assert.Equal(2000.0, result2, 2);
            Assert.Equal(3000.0, result3, 2);
        }

        [Fact]
        public void GetFromSIFunction_ReturnsReusableFunction()
        {
            // Arrange
            var kilometer = CreateLengthBaseUnit();
            kilometer.Prefix = Prefix.kilo;
            var physicalUnit = new PhysicalUnit(kilometer);

            // Act
            var function = physicalUnit.GetFromSIFunction();
            var result1 = function(1000.0);
            var result2 = function(2000.0);
            var result3 = function(3000.0);

            // Assert
            Assert.Equal(1.0, result1, 2);
            Assert.Equal(2.0, result2, 2);
            Assert.Equal(3.0, result3, 2);
        }

        [Fact]
        public void ConvertValue_ComplexUnit_ConvertsCorrectly()
        {
            // Arrange - km/h to m/s
            var kmh = CreateSpeedUnit(Prefix.kilo); // km/h
            var ms = CreateSpeedUnit(Prefix.SI);     // m/s

            // Act - 36 km/h = 10 m/s
            var result = kmh.ConvertValue(ms, 36.0);

            // Assert
            Assert.Equal(10.0, result, 1);
        }

        // Helper methods
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
                Exponent = new Fraction(1),
                ConversionFactor = new Fraction(1)
            };
            baseUnit.RawUnits.Add(new RawUnit(BaseUnitType.Length, 1));
            return baseUnit;
        }

        private BaseUnit CreateTemperatureBaseUnit()
        {
            var baseUnit = new BaseUnit
            {
                Name = "Celsius",
                Symbol = "°C",
                UnitType = UnitType.Temperature_Base,
                UnitSystem = StandardUnitSystem.SI,
                IsSI = false,
                Prefix = Prefix.SI,
                Exponent = new Fraction(1),
                ConversionFactor = new Fraction(1),
                Offset = 0
            };
            baseUnit.RawUnits.Add(new RawUnit(BaseUnitType.Temperature, 1));
            return baseUnit;
        }

        private PhysicalUnit CreateSpeedUnit(Prefix prefix)
        {
            var lengthUnit = CreateLengthBaseUnit();
            lengthUnit.Prefix = prefix;
            lengthUnit.Exponent = new Fraction(1);

            var timeUnit = new BaseUnit
            {
                Name = "Hour",
                Symbol = "h",
                UnitType = UnitType.Time_Base,
                UnitSystem = StandardUnitSystem.SI,
                IsSI = false,
                Prefix = Prefix.SI,
                Exponent = new Fraction(-1),
                ConversionFactor = new Fraction(3600) // 1 hour = 3600 seconds
            };
            timeUnit.RawUnits.Add(new RawUnit(BaseUnitType.Time, -1));

            var speedUnit = new PhysicalUnit { UnitType = UnitType.Speed_Mech };
            speedUnit.BaseUnits.Add(lengthUnit);
            speedUnit.BaseUnits.Add(timeUnit);

            return speedUnit;
        }
    }
}