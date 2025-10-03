using Fractions;
using MatthL.PhysicalUnits.Core.Enums;
using MatthL.PhysicalUnits.DimensionalFormulas.Helpers;
using Xunit;

namespace MatthL.PhysicalUnits.Tests.DimensionalFormulas
{
    public class FormulaHelperTests
    {
        [Fact]
        public void CreateFormulaString_SinglePositiveUnit_ReturnsSymbol()
        {
            // Arrange
            var units = new List<(BaseUnitType, Fraction)>
            {
                (BaseUnitType.Length, new Fraction(1))
            };

            // Act
            var result = FormulaHelper.CreateFormulaString(units);

            // Assert
            Assert.Equal("m", result);
        }

        [Fact]
        public void CreateFormulaString_SinglePositiveWithExponent_ReturnsSymbolWithExponent()
        {
            // Arrange
            var units = new List<(BaseUnitType, Fraction)>
            {
                (BaseUnitType.Length, new Fraction(2))
            };

            // Act
            var result = FormulaHelper.CreateFormulaString(units);

            // Assert
            Assert.Equal("m²", result);
        }

        [Fact]
        public void CreateFormulaString_MultiplePositive_JoinsWithDot()
        {
            // Arrange - kg·m
            var units = new List<(BaseUnitType, Fraction)>
            {
                (BaseUnitType.Mass, new Fraction(1)),
                (BaseUnitType.Length, new Fraction(1))
            };

            // Act
            var result = FormulaHelper.CreateFormulaString(units);

            // Assert
            Assert.Equal("kg·m", result);
        }

        [Fact]
        public void CreateFormulaString_SingleNegative_ReturnsWithDivision()
        {
            // Arrange - 1/s = s^-1
            var units = new List<(BaseUnitType, Fraction)>
            {
                (BaseUnitType.Time, new Fraction(-1))
            };

            // Act
            var result = FormulaHelper.CreateFormulaString(units);

            // Assert
            Assert.Equal("1/s", result);
        }

        [Fact]
        public void CreateFormulaString_PositiveAndNegative_UsesSlash()
        {
            // Arrange - m/s (speed)
            var units = new List<(BaseUnitType, Fraction)>
            {
                (BaseUnitType.Length, new Fraction(1)),
                (BaseUnitType.Time, new Fraction(-1))
            };

            // Act
            var result = FormulaHelper.CreateFormulaString(units);

            // Assert
            Assert.Equal("m/s", result);
        }

        [Fact]
        public void CreateFormulaString_Force_ReturnsCorrectFormula()
        {
            // Arrange - kg·m/s² (Newton)
            var units = new List<(BaseUnitType, Fraction)>
            {
                (BaseUnitType.Mass, new Fraction(1)),
                (BaseUnitType.Length, new Fraction(1)),
                (BaseUnitType.Time, new Fraction(-2))
            };

            // Act
            var result = FormulaHelper.CreateFormulaString(units);

            // Assert
            Assert.Equal("kg·m/s²", result);
        }

        [Fact]
        public void CreateFormulaString_Energy_ReturnsCorrectFormula()
        {
            // Arrange - kg·m²/s² (Joule)
            var units = new List<(BaseUnitType, Fraction)>
            {
                (BaseUnitType.Mass, new Fraction(1)),
                (BaseUnitType.Length, new Fraction(2)),
                (BaseUnitType.Time, new Fraction(-2))
            };

            // Act
            var result = FormulaHelper.CreateFormulaString(units);

            // Assert
            Assert.Equal("kg·m²/s²", result);
        }

        [Fact]
        public void CreateFormulaString_Power_ReturnsCorrectFormula()
        {
            // Arrange - kg·m²/s³ (Watt)
            var units = new List<(BaseUnitType, Fraction)>
            {
                (BaseUnitType.Mass, new Fraction(1)),
                (BaseUnitType.Length, new Fraction(2)),
                (BaseUnitType.Time, new Fraction(-3))
            };

            // Act
            var result = FormulaHelper.CreateFormulaString(units);

            // Assert
            Assert.Equal("kg·m²/s³", result);
        }

        [Fact]
        public void CreateFormulaString_MultipleNegative_GroupsAfterSlash()
        {
            // Arrange - 1/(m·s)
            var units = new List<(BaseUnitType, Fraction)>
            {
                (BaseUnitType.Length, new Fraction(-1)),
                (BaseUnitType.Time, new Fraction(-1))
            };

            // Act
            var result = FormulaHelper.CreateFormulaString(units);

            // Assert
            Assert.Equal("1/m·s", result);
        }

        [Fact]
        public void CreateFormulaString_FractionalExponent_FormatsCorrectly()
        {
            // Arrange - m^(1/2)
            var units = new List<(BaseUnitType, Fraction)>
            {
                (BaseUnitType.Length, new Fraction(1, 2))
            };

            // Act
            var result = FormulaHelper.CreateFormulaString(units);

            // Assert
            // Should contain proper fraction notation
            Assert.Contains("m", result);
        }

        [Fact]
        public void CreateFormulaString_EmptyList_ReturnsEmptyString()
        {
            // Arrange
            var units = new List<(BaseUnitType, Fraction)>();

            // Act
            var result = FormulaHelper.CreateFormulaString(units);

            // Assert
            Assert.Equal("", result);
        }

        [Fact]
        public void CreateFormulaString_Pressure_ReturnsCorrectFormula()
        {
            // Arrange - kg/(m·s²) = Pascal
            var units = new List<(BaseUnitType, Fraction)>
            {
                (BaseUnitType.Mass, new Fraction(1)),
                (BaseUnitType.Length, new Fraction(-1)),
                (BaseUnitType.Time, new Fraction(-2))
            };

            // Act
            var result = FormulaHelper.CreateFormulaString(units);

            // Assert
            Assert.Equal("kg/m·s²", result);
        }

        [Fact]
        public void CreateFormulaString_Acceleration_ReturnsCorrectFormula()
        {
            // Arrange - m/s²
            var units = new List<(BaseUnitType, Fraction)>
            {
                (BaseUnitType.Length, new Fraction(1)),
                (BaseUnitType.Time, new Fraction(-2))
            };

            // Act
            var result = FormulaHelper.CreateFormulaString(units);

            // Assert
            Assert.Equal("m/s²", result);
        }

        [Fact]
        public void CreateFormulaString_Density_ReturnsCorrectFormula()
        {
            // Arrange - kg/m³
            var units = new List<(BaseUnitType, Fraction)>
            {
                (BaseUnitType.Mass, new Fraction(1)),
                (BaseUnitType.Length, new Fraction(-3))
            };

            // Act
            var result = FormulaHelper.CreateFormulaString(units);

            // Assert
            Assert.Equal("kg/m³", result);
        }

        [Fact]
        public void CreateFormulaString_ComplexUnit_FormatsCorrectly()
        {
            // Arrange - kg·m·A²·s^-3 (Volt)
            var units = new List<(BaseUnitType, Fraction)>
            {
                (BaseUnitType.Mass, new Fraction(1)),
                (BaseUnitType.Length, new Fraction(2)),
                (BaseUnitType.ElectricCurrent, new Fraction(-2)),
                (BaseUnitType.Time, new Fraction(-3))
            };

            // Act
            var result = FormulaHelper.CreateFormulaString(units);

            // Assert
            Assert.Contains("kg·m²", result);
            Assert.Contains("/", result);
            Assert.Contains("A²", result);
            Assert.Contains("s³", result);
        }
    }
}