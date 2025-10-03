using Fractions;
using MatthL.PhysicalUnits.Computation.Extensions;
using MatthL.PhysicalUnits.Core.Enums;
using Xunit;

namespace MatthL.PhysicalUnits.Tests.Computation
{
    public class ComputationExtensionsTests
    {
        #region DictionaryExtensions Tests

        [Fact]
        public void FilterPhysicalDimensions_RemovesNonPhysicalTypes()
        {
            // Arrange
            var dimensions = new Dictionary<BaseUnitType, Fraction>
            {
                { BaseUnitType.Length, new Fraction(1) },
                { BaseUnitType.Mass, new Fraction(1) },
                { BaseUnitType.Angle, new Fraction(1) },
                { BaseUnitType.Ratio, new Fraction(1) },
                { BaseUnitType.Currency, new Fraction(1) }
            };

            // Act
            var result = dimensions.FilterPhysicalDimensions();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(BaseUnitType.Length, result.Keys);
            Assert.Contains(BaseUnitType.Mass, result.Keys);
            Assert.DoesNotContain(BaseUnitType.Angle, result.Keys);
            Assert.DoesNotContain(BaseUnitType.Ratio, result.Keys);
            Assert.DoesNotContain(BaseUnitType.Currency, result.Keys);
        }

        [Fact]
        public void FilterPhysicalDimensions_OnlyPhysical_ReturnsAll()
        {
            // Arrange
            var dimensions = new Dictionary<BaseUnitType, Fraction>
            {
                { BaseUnitType.Length, new Fraction(1) },
                { BaseUnitType.Mass, new Fraction(1) },
                { BaseUnitType.Time, new Fraction(-2) },
                { BaseUnitType.ElectricCurrent, new Fraction(1) }
            };

            // Act
            var result = dimensions.FilterPhysicalDimensions();

            // Assert
            Assert.Equal(4, result.Count);
        }

        [Fact]
        public void FilterPhysicalDimensions_OnlyNonPhysical_ReturnsEmpty()
        {
            // Arrange
            var dimensions = new Dictionary<BaseUnitType, Fraction>
            {
                { BaseUnitType.Angle, new Fraction(1) },
                { BaseUnitType.Ratio, new Fraction(1) },
                { BaseUnitType.Currency, new Fraction(1) }
            };

            // Act
            var result = dimensions.FilterPhysicalDimensions();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void FilterPhysicalDimensions_EmptyDictionary_ReturnsEmpty()
        {
            // Arrange
            var dimensions = new Dictionary<BaseUnitType, Fraction>();

            // Act
            var result = dimensions.FilterPhysicalDimensions();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void IsDimensionsEqualTo_SameDimensions_ReturnsTrue()
        {
            // Arrange
            var dim1 = new Dictionary<BaseUnitType, Fraction>
            {
                { BaseUnitType.Length, new Fraction(1) },
                { BaseUnitType.Mass, new Fraction(1) },
                { BaseUnitType.Time, new Fraction(-2) }
            };
            var dim2 = new Dictionary<BaseUnitType, Fraction>
            {
                { BaseUnitType.Length, new Fraction(1) },
                { BaseUnitType.Mass, new Fraction(1) },
                { BaseUnitType.Time, new Fraction(-2) }
            };

            // Act
            var result = dim1.IsDimensionsEqualTo(dim2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsDimensionsEqualTo_DifferentExponents_ReturnsFalse()
        {
            // Arrange
            var dim1 = new Dictionary<BaseUnitType, Fraction>
            {
                { BaseUnitType.Length, new Fraction(1) },
                { BaseUnitType.Time, new Fraction(-2) }
            };
            var dim2 = new Dictionary<BaseUnitType, Fraction>
            {
                { BaseUnitType.Length, new Fraction(2) },
                { BaseUnitType.Time, new Fraction(-2) }
            };

            // Act
            var result = dim1.IsDimensionsEqualTo(dim2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsDimensionsEqualTo_DifferentKeys_ReturnsFalse()
        {
            // Arrange
            var dim1 = new Dictionary<BaseUnitType, Fraction>
            {
                { BaseUnitType.Length, new Fraction(1) },
                { BaseUnitType.Mass, new Fraction(1) }
            };
            var dim2 = new Dictionary<BaseUnitType, Fraction>
            {
                { BaseUnitType.Length, new Fraction(1) },
                { BaseUnitType.Time, new Fraction(1) }
            };

            // Act
            var result = dim1.IsDimensionsEqualTo(dim2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsDimensionsEqualTo_DifferentSizes_ReturnsFalse()
        {
            // Arrange
            var dim1 = new Dictionary<BaseUnitType, Fraction>
            {
                { BaseUnitType.Length, new Fraction(1) },
                { BaseUnitType.Mass, new Fraction(1) },
                { BaseUnitType.Time, new Fraction(-2) }
            };
            var dim2 = new Dictionary<BaseUnitType, Fraction>
            {
                { BaseUnitType.Length, new Fraction(1) },
                { BaseUnitType.Mass, new Fraction(1) }
            };

            // Act
            var result = dim1.IsDimensionsEqualTo(dim2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsDimensionsEqualTo_BothEmpty_ReturnsTrue()
        {
            // Arrange
            var dim1 = new Dictionary<BaseUnitType, Fraction>();
            var dim2 = new Dictionary<BaseUnitType, Fraction>();

            // Act
            var result = dim1.IsDimensionsEqualTo(dim2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsDimensionsEqualTo_FractionalExponents_ComparesCorrectly()
        {
            // Arrange
            var dim1 = new Dictionary<BaseUnitType, Fraction>
            {
                { BaseUnitType.Length, new Fraction(3, 2) },
                { BaseUnitType.Time, new Fraction(-1, 2) }
            };
            var dim2 = new Dictionary<BaseUnitType, Fraction>
            {
                { BaseUnitType.Length, new Fraction(3, 2) },
                { BaseUnitType.Time, new Fraction(-1, 2) }
            };

            // Act
            var result = dim1.IsDimensionsEqualTo(dim2);

            // Assert
            Assert.True(result);
        }

        #endregion

        #region FractionExtensions Tests

        [Fact]
        public void Pow_IntegerExponent_CalculatesCorrectly()
        {
            // Arrange
            var baseValue = new Fraction(2);
            var exponent = new Fraction(3);

            // Act
            var result = baseValue.Pow(exponent);

            // Assert
            Assert.Equal(new Fraction(8), result);
        }

        [Fact]
        public void Pow_NegativeIntegerExponent_CalculatesCorrectly()
        {
            // Arrange
            var baseValue = new Fraction(2);
            var exponent = new Fraction(-2);

            // Act
            var result = baseValue.Pow(exponent);

            // Assert
            Assert.Equal(new Fraction(1, 4), result);
        }

        [Fact]
        public void Pow_ZeroExponent_ReturnsOne()
        {
            // Arrange
            var baseValue = new Fraction(5);
            var exponent = new Fraction(0);

            // Act
            var result = baseValue.Pow(exponent);

            // Assert
            Assert.Equal(new Fraction(1), result);
        }

        [Fact]
        public void Pow_OneExponent_ReturnsBase()
        {
            // Arrange
            var baseValue = new Fraction(42);
            var exponent = new Fraction(1);

            // Act
            var result = baseValue.Pow(exponent);

            // Assert
            Assert.Equal(new Fraction(42), result);
        }

        [Fact]
        public void Pow_FractionalExponent_SquareRoot()
        {
            // Arrange - √4 = 2
            var baseValue = new Fraction(4);
            var exponent = new Fraction(1, 2);

            // Act
            var result = baseValue.Pow(exponent);

            // Assert
            Assert.Equal(2.0, result.ToDouble(), 5);
        }

        [Fact]
        public void Pow_FractionalExponent_CubeRoot()
        {
            // Arrange - ∛8 = 2
            var baseValue = new Fraction(8);
            var exponent = new Fraction(1, 3);

            // Act
            var result = baseValue.Pow(exponent);

            // Assert
            Assert.Equal(2.0, result.ToDouble(), 5);
        }

        [Fact]
        public void Pow_FractionalBase_IntegerExponent()
        {
            // Arrange - (1/2)² = 1/4
            var baseValue = new Fraction(1, 2);
            var exponent = new Fraction(2);

            // Act
            var result = baseValue.Pow(exponent);

            // Assert
            Assert.Equal(new Fraction(1, 4), result);
        }

        [Fact]
        public void Pow_FractionalBaseAndExponent_CalculatesCorrectly()
        {
            // Arrange - (1/4)^(1/2) = 1/2
            var baseValue = new Fraction(1, 4);
            var exponent = new Fraction(1, 2);

            // Act
            var result = baseValue.Pow(exponent);

            // Assert
            Assert.Equal(0.5, result.ToDouble(), 5);
        }

        [Fact]
        public void Pow_LargeIntegerExponent_CalculatesCorrectly()
        {
            // Arrange
            var baseValue = new Fraction(2);
            var exponent = new Fraction(10);

            // Act
            var result = baseValue.Pow(exponent);

            // Assert
            Assert.Equal(new Fraction(1024), result);
        }

        [Fact]
        public void Pow_NegativeBase_EvenExponent()
        {
            // Arrange - (-2)² = 4
            var baseValue = new Fraction(-2);
            var exponent = new Fraction(2);

            // Act
            var result = baseValue.Pow(exponent);

            // Assert
            Assert.Equal(new Fraction(4), result);
        }

        [Fact]
        public void Pow_NegativeBase_OddExponent()
        {
            // Arrange - (-2)³ = -8
            var baseValue = new Fraction(-2);
            var exponent = new Fraction(3);

            // Act
            var result = baseValue.Pow(exponent);

            // Assert
            Assert.Equal(new Fraction(-8), result);
        }

        #endregion
    }
}