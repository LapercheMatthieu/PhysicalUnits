using Fractions;
using MatthL.PhysicalUnits.Core.Enums;
using MatthL.PhysicalUnits.Core.Models;
using MatthL.PhysicalUnits.Infrastructure.Extensions;
using Xunit;

namespace MatthL.PhysicalUnits.Tests.Infrastructure
{
    public class RawUnitExtensionsTests
    {
        [Fact]
        public void Clone_CreatesIndependentCopy()
        {
            // Arrange
            var original = new RawUnit(BaseUnitType.Length, 2);

            // Act
            var cloned = original.Clone();

            // Assert
            Assert.NotSame(original, cloned);
            Assert.Equal(original.UnitType, cloned.UnitType);
            Assert.Equal(original.Exponent, cloned.Exponent);
        }

        [Fact]
        public void Clone_WithFractionalExponent_PreservesExponent()
        {
            // Arrange
            var original = new RawUnit(BaseUnitType.Length, new Fraction(3, 2));

            // Act
            var cloned = original.Clone();

            // Assert
            Assert.Equal(new Fraction(3, 2), cloned.Exponent);
        }

        [Fact]
        public void Clone_ModifyingClone_DoesNotAffectOriginal()
        {
            // Arrange
            var original = new RawUnit(BaseUnitType.Mass, 1);
            var cloned = original.Clone();

            // Act
            cloned.Exponent = new Fraction(5);

            // Assert
            Assert.Equal(new Fraction(1), original.Exponent);
            Assert.Equal(new Fraction(5), cloned.Exponent);
        }

        [Fact]
        public void Clone_NegativeExponent_PreservesSign()
        {
            // Arrange
            var original = new RawUnit(BaseUnitType.Time, -2);

            // Act
            var cloned = original.Clone();

            // Assert
            Assert.Equal(new Fraction(-2), cloned.Exponent);
        }

        [Fact]
        public void Clone_ZeroExponent_PreservesZero()
        {
            // Arrange
            var original = new RawUnit(BaseUnitType.Length, 0);

            // Act
            var cloned = original.Clone();

            // Assert
            Assert.Equal(new Fraction(0), cloned.Exponent);
        }

        [Fact]
        public void Clone_DifferentUnitTypes_PreservesType()
        {
            // Arrange
            var types = new[]
            {
                BaseUnitType.Length,
                BaseUnitType.Mass,
                BaseUnitType.Time,
                BaseUnitType.ElectricCurrent,
                BaseUnitType.Temperature,
                BaseUnitType.AmountOfSubstance,
                BaseUnitType.LuminousIntensity,
                BaseUnitType.Angle
            };

            foreach (var type in types)
            {
                // Arrange
                var original = new RawUnit(type, 1);

                // Act
                var cloned = original.Clone();

                // Assert
                Assert.Equal(type, cloned.UnitType);
            }
        }

        [Fact]
        public void Clone_MultipleClones_AreIndependent()
        {
            // Arrange
            var original = new RawUnit(BaseUnitType.Length, 1);

            // Act
            var clone1 = original.Clone();
            var clone2 = original.Clone();
            var clone3 = original.Clone();

            // Assert
            Assert.NotSame(clone1, clone2);
            Assert.NotSame(clone1, clone3);
            Assert.NotSame(clone2, clone3);
        }

        [Fact]
        public void Clone_LargeExponent_PreservesValue()
        {
            // Arrange
            var original = new RawUnit(BaseUnitType.Length, 1000);

            // Act
            var cloned = original.Clone();

            // Assert
            Assert.Equal(new Fraction(1000), cloned.Exponent);
        }

        [Fact]
        public void Clone_ComplexFraction_PreservesExactValue()
        {
            // Arrange
            var original = new RawUnit(BaseUnitType.Mass, new Fraction(7, 13));

            // Act
            var cloned = original.Clone();

            // Assert
            Assert.Equal(new Fraction(7, 13), cloned.Exponent);
            Assert.Equal(original.Exponent.Numerator, cloned.Exponent.Numerator);
            Assert.Equal(original.Exponent.Denominator, cloned.Exponent.Denominator);
        }
    }
}