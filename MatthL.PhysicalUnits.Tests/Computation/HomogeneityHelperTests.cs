using Fractions;
using MatthL.PhysicalUnits.Computation.Helpers;
using MatthL.PhysicalUnits.Core.Enums;
using MatthL.PhysicalUnits.Core.EquationModels;
using MatthL.PhysicalUnits.Core.Models;
using Xunit;

namespace MatthL.PhysicalUnits.Tests.Computation
{
    public class HomogeneityHelperTests
    {
        [Fact]
        public void VerifyHomogeneity_SameUnits_ReturnsTrue()
        {
            // Arrange
            var length1 = new PhysicalUnit(CreateLengthBaseUnit());
            var length2 = new PhysicalUnit(CreateLengthBaseUnit());

            var term1 = new PhysicalUnitTerm(length1, new Fraction(1));
            var term2 = new PhysicalUnitTerm(length2, new Fraction(1));

            // Act
            var result = HomogeneityHelper.VerifyHomogeneity(term1, term2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void VerifyHomogeneity_DifferentUnits_ReturnsFalse()
        {
            // Arrange
            var length = new PhysicalUnit(CreateLengthBaseUnit());
            var mass = new PhysicalUnit(CreateMassBaseUnit());

            var term1 = new PhysicalUnitTerm(length, new Fraction(1));
            var term2 = new PhysicalUnitTerm(mass, new Fraction(1));

            // Act
            var result = HomogeneityHelper.VerifyHomogeneity(term1, term2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void VerifyHomogeneity_SameDimensionDifferentExponent_ReturnsFalse()
        {
            // Arrange - m vs m²
            var length1 = new PhysicalUnit(CreateLengthBaseUnit());
            var length2 = new PhysicalUnit(CreateLengthBaseUnit());

            var term1 = new PhysicalUnitTerm(length1, new Fraction(1));
            var term2 = new PhysicalUnitTerm(length2, new Fraction(2));

            // Act
            var result = HomogeneityHelper.VerifyHomogeneity(term1, term2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void VerifyHomogeneity_SingleTerm_ReturnsTrue()
        {
            // Arrange
            var length = new PhysicalUnit(CreateLengthBaseUnit());
            var term = new PhysicalUnitTerm(length, new Fraction(1));

            // Act
            var result = HomogeneityHelper.VerifyHomogeneity(term);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void VerifyHomogeneity_NullTerms_ReturnsTrue()
        {
            // Act
            var result = HomogeneityHelper.VerifyHomogeneity((PhysicalUnitTerm[])null);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void VerifyHomogeneity_EmptyArray_ReturnsTrue()
        {
            // Act
            var result = HomogeneityHelper.VerifyHomogeneity(Array.Empty<PhysicalUnitTerm>());

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void VerifyHomogeneity_SpeedUnits_ReturnsTrue()
        {
            // Arrange - m/s and km/h (same dimension)
            var speed1 = CreateSpeedPhysicalUnit();
            var speed2 = CreateSpeedPhysicalUnit();

            var term1 = new PhysicalUnitTerm(speed1, new Fraction(1));
            var term2 = new PhysicalUnitTerm(speed2, new Fraction(1));

            // Act
            var result = HomogeneityHelper.VerifyHomogeneity(term1, term2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void VerifyHomogeneity_ForceUnits_ReturnsTrue()
        {
            // Arrange - kg·m/s² (Newton) twice
            var force1 = CreateForcePhysicalUnit();
            var force2 = CreateForcePhysicalUnit();

            var term1 = new PhysicalUnitTerm(force1, new Fraction(1));
            var term2 = new PhysicalUnitTerm(force2, new Fraction(1));

            // Act
            var result = HomogeneityHelper.VerifyHomogeneity(term1, term2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void VerifyHomogeneity_PhysicalUnits_SameDimension_ReturnsTrue()
        {
            // Arrange
            var length1 = new PhysicalUnit(CreateLengthBaseUnit());
            var length2 = new PhysicalUnit(CreateLengthBaseUnit());

            // Act
            var result = HomogeneityHelper.VerifyHomogeneity(length1, length2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void VerifyHomogeneity_PhysicalUnits_DifferentDimension_ReturnsFalse()
        {
            // Arrange
            var length = new PhysicalUnit(CreateLengthBaseUnit());
            var mass = new PhysicalUnit(CreateMassBaseUnit());

            // Act
            var result = HomogeneityHelper.VerifyHomogeneity(length, mass);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void VerifyHomogeneity_EquationTerms_Homogeneous_ReturnsTrue()
        {
            // Arrange
            var length = new PhysicalUnit(CreateLengthBaseUnit());

            var equation1 = new EquationTerms(
                new PhysicalUnitTerm(length, new Fraction(1))
            );
            var equation2 = new EquationTerms(
                new PhysicalUnitTerm(length, new Fraction(1))
            );

            // Act
            var result = HomogeneityHelper.VerifyHomogeneity(equation1, equation2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void VerifyHomogeneity_EquationTerms_NonHomogeneous_ReturnsFalse()
        {
            // Arrange
            var length = new PhysicalUnit(CreateLengthBaseUnit());
            var mass = new PhysicalUnit(CreateMassBaseUnit());

            var equation1 = new EquationTerms(
                new PhysicalUnitTerm(length, new Fraction(1))
            );
            var equation2 = new EquationTerms(
                new PhysicalUnitTerm(mass, new Fraction(1))
            );

            // Act
            var result = HomogeneityHelper.VerifyHomogeneity(equation1, equation2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void VerifyHomogeneity_EquationTerms_MultipleTerms_Homogeneous_ReturnsTrue()
        {
            // Arrange - Force × Distance = Energy (both should be kg·m²/s²)
            var force = CreateForcePhysicalUnit();
            var distance = new PhysicalUnit(CreateLengthBaseUnit());

            var equation1 = new EquationTerms(
                new PhysicalUnitTerm(force, new Fraction(1)),
                new PhysicalUnitTerm(distance, new Fraction(1))
            );

            var energy = CreateEnergyPhysicalUnit();
            var equation2 = new EquationTerms(
                new PhysicalUnitTerm(energy, new Fraction(1))
            );

            // Act
            var result = HomogeneityHelper.VerifyHomogeneity(equation1, equation2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void VerifyHomogeneity_ComplexUnits_SameDimension_ReturnsTrue()
        {
            // Arrange - Both are kg·m²/s² (energy)
            var energy1 = CreateEnergyPhysicalUnit();
            var energy2 = CreateEnergyPhysicalUnit();

            var term1 = new PhysicalUnitTerm(energy1, new Fraction(1));
            var term2 = new PhysicalUnitTerm(energy2, new Fraction(1));

            // Act
            var result = HomogeneityHelper.VerifyHomogeneity(term1, term2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void VerifyHomogeneity_WithAngleAndRatio_IgnoresNonPhysical()
        {
            // Arrange - m·rad vs m (angle should be ignored)
            var lengthWithAngle = new PhysicalUnit { UnitType = UnitType.Unknown_Special };
            lengthWithAngle.BaseUnits.Add(CreateLengthBaseUnit());
            var angleUnit = CreateAngleBaseUnit();
            lengthWithAngle.BaseUnits.Add(angleUnit);

            var lengthOnly = new PhysicalUnit(CreateLengthBaseUnit());

            var term1 = new PhysicalUnitTerm(lengthWithAngle, new Fraction(1));
            var term2 = new PhysicalUnitTerm(lengthOnly, new Fraction(1));

            // Act
            var result = HomogeneityHelper.VerifyHomogeneity(term1, term2);

            // Assert
            Assert.True(result); // Should be homogeneous when non-physical dimensions are filtered
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
                Exponent = new Fraction(1)
            };
            baseUnit.RawUnits.Add(new RawUnit(BaseUnitType.Length, 1));
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

        private BaseUnit CreateAngleBaseUnit()
        {
            var baseUnit = new BaseUnit
            {
                Name = "Radian",
                Symbol = "rad",
                UnitType = UnitType.Angle_Base,
                UnitSystem = StandardUnitSystem.SI,
                IsSI = true,
                Prefix = Prefix.SI,
                Exponent = new Fraction(1)
            };
            baseUnit.RawUnits.Add(new RawUnit(BaseUnitType.Angle, 1));
            return baseUnit;
        }

        private PhysicalUnit CreateSpeedPhysicalUnit()
        {
            var baseUnit = new BaseUnit
            {
                Name = "MeterPerSecond",
                Symbol = "m/s",
                UnitType = UnitType.Speed_Mech,
                UnitSystem = StandardUnitSystem.SI,
                IsSI = true,
                Prefix = Prefix.SI,
                Exponent = new Fraction(1)
            };
            baseUnit.RawUnits.Add(new RawUnit(BaseUnitType.Length, 1));
            baseUnit.RawUnits.Add(new RawUnit(BaseUnitType.Time, -1));
            return new PhysicalUnit(baseUnit);
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
    }
}