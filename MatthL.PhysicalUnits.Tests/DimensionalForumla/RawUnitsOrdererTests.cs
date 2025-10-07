using Fractions;
using MatthL.PhysicalUnits.Core.Enums;
using MatthL.PhysicalUnits.DimensionalFormulas.Helpers;
using Xunit;

namespace MatthL.PhysicalUnits.Tests.DimensionalFormulas
{
    public class RawUnitsOrdererTests
    {
        [Fact]
        public void OrderFormula_PositiveExponents_OrdersByKey()
        {
            // Arrange
            var formula = new Dictionary<BaseUnitType, Fraction>
            {
                { BaseUnitType.Time, new Fraction(1) },
                { BaseUnitType.Length, new Fraction(2) },
                { BaseUnitType.Mass, new Fraction(1) }
            };

            // Act
            var result = RawUnitsOrderer.OrderFormula(formula);

            // Assert
            Assert.Equal(3, result.Count);
            // Should be ordered by BaseUnitType enum value
            Assert.Equal(BaseUnitType.Length, result[1].Item1);
            Assert.Equal(BaseUnitType.Mass, result[0].Item1);
            Assert.Equal(BaseUnitType.Time, result[2].Item1);
        }

        [Fact]
        public void OrderFormula_NegativeExponents_PlacedAfterPositive()
        {
            // Arrange
            var formula = new Dictionary<BaseUnitType, Fraction>
            {
                { BaseUnitType.Time, new Fraction(-2) },
                { BaseUnitType.Length, new Fraction(1) },
                { BaseUnitType.Mass, new Fraction(1) }
            };

            // Act
            var result = RawUnitsOrderer.OrderFormula(formula);

            // Assert
            Assert.Equal(3, result.Count);
            // Positive first
            Assert.True(result[0].Item2 > 0);
            Assert.True(result[1].Item2 > 0);
            // Negative last
            Assert.True(result[2].Item2 < 0);
            Assert.Equal(BaseUnitType.Time, result[2].Item1);
        }

        [Fact]
        public void OrderFormula_MixedExponents_GroupsCorrectly()
        {
            // Arrange - Force: kg·m·s^-2
            var formula = new Dictionary<BaseUnitType, Fraction>
            {
                { BaseUnitType.Time, new Fraction(-2) },
                { BaseUnitType.Mass, new Fraction(1) },
                { BaseUnitType.Length, new Fraction(1) }
            };

            // Act
            var result = RawUnitsOrderer.OrderFormula(formula);

            // Assert
            Assert.Equal(3, result.Count);

            // First two should be positive
            Assert.True(result[0].Item2 > 0);
            Assert.True(result[1].Item2 > 0);

            // Last should be negative
            Assert.True(result[2].Item2 < 0);
            Assert.Equal(BaseUnitType.Time, result[2].Item1);
            Assert.Equal(new Fraction(-2), result[2].Item2);
        }

        [Fact]
        public void OrderFormula_EmptyDictionary_ReturnsEmptyList()
        {
            // Arrange
            var formula = new Dictionary<BaseUnitType, Fraction>();

            // Act
            var result = RawUnitsOrderer.OrderFormula(formula);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void OrderFormula_SinglePositive_ReturnsSingleElement()
        {
            // Arrange
            var formula = new Dictionary<BaseUnitType, Fraction>
            {
                { BaseUnitType.Length, new Fraction(2) }
            };

            // Act
            var result = RawUnitsOrderer.OrderFormula(formula);

            // Assert
            Assert.Single(result);
            Assert.Equal(BaseUnitType.Length, result[0].Item1);
            Assert.Equal(new Fraction(2), result[0].Item2);
        }

        [Fact]
        public void OrderFormula_SingleNegative_ReturnsSingleElement()
        {
            // Arrange
            var formula = new Dictionary<BaseUnitType, Fraction>
            {
                { BaseUnitType.Time, new Fraction(-1) }
            };

            // Act
            var result = RawUnitsOrderer.OrderFormula(formula);

            // Assert
            Assert.Single(result);
            Assert.Equal(BaseUnitType.Time, result[0].Item1);
            Assert.Equal(new Fraction(-1), result[0].Item2);
        }

        [Fact]
        public void OrderFormula_AllNegative_OrdersByKey()
        {
            // Arrange
            var formula = new Dictionary<BaseUnitType, Fraction>
            {
                { BaseUnitType.Time, new Fraction(-1) },
                { BaseUnitType.Mass, new Fraction(-2) },
                { BaseUnitType.Length, new Fraction(-1) }
            };

            // Act
            var result = RawUnitsOrderer.OrderFormula(formula);

            // Assert
            Assert.Equal(3, result.Count);
            Assert.All(result, r => Assert.True(r.Item2 < 0));

            // Should be ordered by key
            Assert.Equal(BaseUnitType.Length, result[1].Item1);
            Assert.Equal(BaseUnitType.Mass, result[0].Item1);
            Assert.Equal(BaseUnitType.Time, result[2].Item1);
        }

        [Fact]
        public void OrderFormula_FractionalExponents_PreservesValues()
        {
            // Arrange
            var formula = new Dictionary<BaseUnitType, Fraction>
            {
                { BaseUnitType.Length, new Fraction(3, 2) },
                { BaseUnitType.Time, new Fraction(-1, 2) }
            };

            // Act
            var result = RawUnitsOrderer.OrderFormula(formula);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(new Fraction(3, 2), result[0].Item2);
            Assert.Equal(new Fraction(-1, 2), result[1].Item2);
        }

        [Fact]
        public void OrderFormula_MultiplePositiveAndNegative_CorrectSeparation()
        {
            // Arrange - Energy/Time = Power: kg·m²·s^-3
            var formula = new Dictionary<BaseUnitType, Fraction>
            {
                { BaseUnitType.Time, new Fraction(-3) },
                { BaseUnitType.Length, new Fraction(2) },
                { BaseUnitType.Mass, new Fraction(1) }
            };

            // Act
            var result = RawUnitsOrderer.OrderFormula(formula);

            // Assert
            Assert.Equal(3, result.Count);

            // First two positive
            Assert.Equal(2, result.Count(r => r.Item2 > 0));
            Assert.Equal(BaseUnitType.Length, result[1].Item1);
            Assert.Equal(BaseUnitType.Mass, result[0].Item1);

            // Last one negative
            Assert.Single(result.Where(r => r.Item2 < 0));
            Assert.Equal(BaseUnitType.Time, result[2].Item1);
        }
    }
}