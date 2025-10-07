using Fractions;
using MatthL.PhysicalUnits.Computation.Converters;
using MatthL.PhysicalUnits.Computation.Extensions;
using MatthL.PhysicalUnits.Core.Enums;
using MatthL.PhysicalUnits.Core.Models;
using MatthL.PhysicalUnits.Infrastructure.Extensions;
using MatthL.PhysicalUnits.Infrastructure.Library;
using MatthL.PhysicalUnits.Infrastructure.Repositories;
using Xunit;

namespace MatthL.PhysicalUnits.Tests.Converters
{
    public class UnitConverterTests
    {
        [Fact]
        public void ConvertToSIValue_SimpleUnit_ConvertsCorrectly()
        {
            // Arrange - kilometer to meter
            var kilometer = StandardUnits.Meter(Prefix.kilo);

            // Act
            var result = kilometer.ConvertToSIValue(5.0); // 5 km = 5000 m

            // Assert
            Assert.Equal(5000.0, result, 2);
        }

        [Fact]
        public void ConvertToSIValue_Millimeter_ConvertsCorrectly()
        {
            // Arrange
            var millimeter = StandardUnits.Meter(Prefix.milli);

            // Act
            var result = millimeter.ConvertToSIValue(1000.0); // 1000 mm = 1 m

            // Assert
            Assert.Equal(1.0, result, 2);
        }

        [Fact]
        public void ConvertFromSIValue_SimpleUnit_ConvertsCorrectly()
        {
            // Arrange - meter to kilometer
            var kilometer = StandardUnits.Meter(Prefix.kilo);
            // Act
            var result = kilometer.ConvertFromSIValue(5000.0); // 5000 m = 5 km

            // Assert
            Assert.Equal(5.0, result, 2);
        }

        [Fact]
        public void ConvertFromSIValue_Millimeter_ConvertsCorrectly()
        {
            // Arrange
            var millimeter = StandardUnits.Meter(Prefix.milli);
            // Act
            var result = millimeter.ConvertFromSIValue(1.0); // 1 m = 1000 mm

            // Assert
            Assert.Equal(1000.0, result, 2);
        }

        [Fact]
        public void ConvertToSIValue_WithExponent_ConvertsCorrectly()
        {
            // Arrange - square kilometer to square meter (km² to m²)
            var squareKilometer = StandardUnits.Meter(Prefix.kilo);
            squareKilometer = squareKilometer.Pow(2);

            // Act
            var result = squareKilometer.ConvertToSIValue(1.0); // 1 km² = 1,000,000 m²

            // Assert
            Assert.Equal(1000000.0, result, 2);
        }

        [Fact]
        public void ConvertFromSIValue_WithExponent_ConvertsCorrectly()
        {
            // Arrange
            var squareKilometer = StandardUnits.Meter(Prefix.kilo);
            squareKilometer = squareKilometer.Pow(2);

            // Act
            var result = squareKilometer.ConvertFromSIValue(1000000.0); // 1,000,000 m² = 1 km²

            // Assert
            Assert.Equal(1.0, result, 2);
        }

        [Fact]
        public void ConvertToSIValues_Array_ConvertsAllElements()
        {
            // Arrange
            var kilometer = StandardUnits.Meter(Prefix.kilo);
            var values = new double[] { 1.0, 2.0, 3.0, 4.0, 5.0 };

            // Act
            var results = kilometer.ConvertToSIValues(values);

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
            var kilometer = StandardUnits.Meter(Prefix.kilo);
            var values = new double[] { 1000.0, 2000.0, 3000.0, 4000.0, 5000.0 };

            // Act
            var results = kilometer.ConvertFromSIValues(values);

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
            var kilometer = StandardUnits.Meter(Prefix.kilo);

            var millimeter = StandardUnits.Meter(Prefix.milli);

            // Act
            var result = kilometer.ConvertValue(millimeter, 1.0); // 1 km = 1,000,000 mm

            // Assert
            Assert.Equal(1000000.0, result, 2);
        }

        [Fact]
        public void ConvertValue_SameUnit_ReturnsIdentical()
        {
            // Arrange
            var meter1 = StandardUnits.Meter();
            var meter2 = StandardUnits.Meter();

            // Act
            var result = meter1.ConvertValue(meter2, 42.0);

            // Assert
            Assert.Equal(42.0, result, 2);
        }

        [Fact]
        public void ConvertValues_Array_BetweenTwoUnits_ConvertsAllElements()
        {
            // Arrange
            var kilometer = StandardUnits.Meter(Prefix.kilo);

            var meter = StandardUnits.Meter();

            var values = new double[] { 1.0, 2.0, 3.0 };

            // Act
            var results = kilometer.ConvertValues(meter, values);

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
            var celsius = StandardUnits.Celsius;

            // Act
            var result = celsius.ConvertToSIValue(0.0); // 0°C = 273.15 K

            // Assert
            Assert.Equal(273.15, result, 2);
        }

        [Fact]
        public void ConvertFromSIValue_WithOffset_HandlesTemperature()
        {
            // Arrange - Kelvin to Celsius
            var celsius = StandardUnits.Celsius;

            // Act
            var result = celsius.ConvertFromSIValue(273.15); // 273.15 K = 0°C

            // Assert
            Assert.Equal(0.0, result, 2);
        }

        [Fact]
        public void ConvertToSIValue_WithConversionFactor_ConvertsCorrectly()
        {
            // Arrange - inch to meter (1 inch = 0.0254 m)
            var inch = StandardUnits.Inch();

            // Act
            var result = inch.ConvertToSIValue(1.0); // 1 inch = 0.0254 m

            // Assert
            Assert.Equal(0.0254, result, 4);
        }

        [Fact]
        public void ConvertFromSIValue_WithConversionFactor_ConvertsCorrectly()
        {
            // Arrange - meter to inch
            var inch = StandardUnits.Inch();

            // Act
            var result = inch.ConvertFromSIValue(0.0254); // 0.0254 m = 1 inch

            // Assert
            Assert.Equal(1.0, result, 4);
        }

        [Fact]
        public void GetToSiFunction_ReturnsReusableFunction()
        {
            // Arrange
            var kilometer = StandardUnits.Meter(Prefix.kilo);

            // Act
            var function = kilometer.GetToSiFunction();
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
            var kilometer = StandardUnits.Meter(Prefix.kilo);

            // Act
            var function = kilometer.GetFromSIFunction();
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
            PhysicalUnitRepository.Initialize();
            // Arrange - km/h to m/s
            var kmh = StandardUnits.MeterPerHour;
            kmh.BaseUnits.Where(p => p.Symbol == "m").First().Prefix = Prefix.kilo;// km/h
            var ms = StandardUnits.MeterPerSecond;     // m/s

            // Act - 36 km/h = 10 m/s
            var result = kmh.ConvertValue(ms, 36.0);

            // Assert
            Assert.Equal(10.0, result, 1);
        }

        
    }
}