using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fractions;
using MatthL.PhysicalUnits.Core.Enums;
using MatthL.PhysicalUnits.Core.Models;
using Xunit;

namespace MatthL.PhysicalUnits.Tests.Core.Models
{
    public class PhysicalUnitTests
    {
        [Fact]
        public void Constructor_Empty_CreatesWithEmptyBaseUnits()
        {
            // Arrange & Act
            var physicalUnit = new PhysicalUnit();

            // Assert
            Assert.NotNull(physicalUnit.BaseUnits);
            Assert.Empty(physicalUnit.BaseUnits);
            Assert.Equal(UnitType.Unknown_Special, physicalUnit.UnitType);
        }

        [Fact]
        public void Constructor_WithBaseUnit_AddsBaseUnitAndSetsUnitType()
        {
            // Arrange
            var baseUnit = new BaseUnit
            {
                UnitType = UnitType.Force_Mech,
                Symbol = "N",
                Name = "Newton",
                IsSI = true
            };

            // Act
            var physicalUnit = new PhysicalUnit(baseUnit);

            // Assert
            Assert.Single(physicalUnit.BaseUnits);
            Assert.Contains(baseUnit, physicalUnit.BaseUnits);
            Assert.Equal(UnitType.Force_Mech, physicalUnit.UnitType);
        }

        [Fact]
        public void Constructor_CopyConstructor_CopiesBaseUnitsAndUnitType()
        {
            // Arrange
            var baseUnit1 = new BaseUnit { UnitType = UnitType.Force_Mech, Symbol = "N" };
            var baseUnit2 = new BaseUnit { UnitType = UnitType.Length_Base, Symbol = "m" };
            var original = new PhysicalUnit { UnitType = UnitType.Force_Mech };
            original.BaseUnits.Add(baseUnit1);
            original.BaseUnits.Add(baseUnit2);

            // Act
            var copy = new PhysicalUnit(original);

            // Assert
            Assert.Equal(original.UnitType, copy.UnitType);
            Assert.Equal(original.BaseUnits.Count, copy.BaseUnits.Count);
        }

        [Fact]
        public void Constructor_CopyWithPrefix_CopiesAndChangesPrefix()
        {
            // Arrange
            var baseUnit = new BaseUnit
            {
                Symbol = "m",
                Prefix = Prefix.SI,
                UnitType = UnitType.Length_Base
            };
            var original = new PhysicalUnit(baseUnit);

            // Act
            var modified = new PhysicalUnit(original, Prefix.kilo);

            // Assert
            Assert.Equal(Prefix.kilo, modified.BaseUnits.First().Prefix);
            Assert.Equal(original.UnitType, modified.UnitType);
        }

        [Fact]
        public void IsSI_AllBaseUnitsSI_ReturnsTrue()
        {
            // Arrange
            var baseUnit1 = new BaseUnit { IsSI = true, Symbol = "m" };
            var baseUnit2 = new BaseUnit { IsSI = true, Symbol = "kg" };
            var physicalUnit = new PhysicalUnit();
            physicalUnit.BaseUnits.Add(baseUnit1);
            physicalUnit.BaseUnits.Add(baseUnit2);

            // Act
            var result = physicalUnit.IsSI;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsSI_OneBaseUnitNotSI_ReturnsFalse()
        {
            // Arrange
            var baseUnit1 = new BaseUnit { IsSI = true, Symbol = "m" };
            var baseUnit2 = new BaseUnit { IsSI = false, Symbol = "ft" };
            var physicalUnit = new PhysicalUnit();
            physicalUnit.BaseUnits.Add(baseUnit1);
            physicalUnit.BaseUnits.Add(baseUnit2);

            // Act
            var result = physicalUnit.IsSI;

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void UnitSystem_SingleSystem_ReturnsThatSystem()
        {
            // Arrange
            var baseUnit1 = new BaseUnit { UnitSystem = StandardUnitSystem.SI };
            var baseUnit2 = new BaseUnit { UnitSystem = StandardUnitSystem.SI };
            var physicalUnit = new PhysicalUnit();
            physicalUnit.BaseUnits.Add(baseUnit1);
            physicalUnit.BaseUnits.Add(baseUnit2);

            // Act
            var result = physicalUnit.UnitSystem;

            // Assert
            Assert.Equal(StandardUnitSystem.SI, result);
        }

        [Fact]
        public void UnitSystem_MultipleSystems_ReturnsMixed()
        {
            // Arrange
            var baseUnit1 = new BaseUnit { UnitSystem = StandardUnitSystem.SI };
            var baseUnit2 = new BaseUnit { UnitSystem = StandardUnitSystem.Imperial };
            var physicalUnit = new PhysicalUnit();
            physicalUnit.BaseUnits.Add(baseUnit1);
            physicalUnit.BaseUnits.Add(baseUnit2);

            // Act
            var result = physicalUnit.UnitSystem;

            // Assert
            Assert.Equal(StandardUnitSystem.Mixed, result);
        }

        [Fact]
        public void ToString_CallsPhysicalUnitNameHelper()
        {
            // Arrange
            var baseUnit = new BaseUnit
            {
                Symbol = "N",
                Prefix = Prefix.SI,
                Exponent = new Fraction(1, 1)
            };
            var physicalUnit = new PhysicalUnit(baseUnit);

            // Act
            var result = physicalUnit.ToString();

            // Assert - Should return formatted string
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void Name_WithSingleBaseUnit_ReturnsFormattedName()
        {
            // Arrange
            var baseUnit = new BaseUnit
            {
                Name = "Newton",
                Symbol = "N",
                Prefix = Prefix.SI,
                Exponent = new Fraction(1, 1)
            };
            var physicalUnit = new PhysicalUnit(baseUnit);

            // Act
            var name = physicalUnit.Name;

            // Assert
            Assert.NotNull(name);
            Assert.NotEmpty(name);
        }

        [Fact]
        public void Name_WithEmptyBaseUnits_ReturnsEmpty()
        {
            // Arrange
            var physicalUnit = new PhysicalUnit();

            // Act
            var name = physicalUnit.Name;

            // Assert
            Assert.Empty(name);
        }

        [Fact]
        public void ComplexUnit_VelocitySquared_ConfiguresCorrectly()
        {
            // Arrange
            var meter = new BaseUnit
            {
                Symbol = "m",
                Prefix = Prefix.SI,
                Exponent = new Fraction(2, 1)
            };
            meter.RawUnits.Add(new RawUnit(BaseUnitType.Length, 2));

            var second = new BaseUnit
            {
                Symbol = "s",
                Prefix = Prefix.SI,
                Exponent = new Fraction(-2, 1)
            };
            second.RawUnits.Add(new RawUnit(BaseUnitType.Time, -2));

            var physicalUnit = new PhysicalUnit { UnitType = UnitType.Speed_Mech };
            physicalUnit.BaseUnits.Add(meter);
            physicalUnit.BaseUnits.Add(second);

            // Assert
            Assert.Equal(2, physicalUnit.BaseUnits.Count);
            Assert.True(physicalUnit.IsSI);
        }

        [Fact]
        public void ComplexUnit_Force_NewtonConfigured()
        {
            // Arrange - Force = kg·m·s⁻²
            var newton = new BaseUnit
            {
                Name = "Newton",
                Symbol = "N",
                UnitType = UnitType.Force_Mech,
                IsSI = true,
                Prefix = Prefix.SI,
                Exponent = new Fraction(1, 1)
            };
            newton.RawUnits.Add(new RawUnit(BaseUnitType.Mass, 1));
            newton.RawUnits.Add(new RawUnit(BaseUnitType.Length, 1));
            newton.RawUnits.Add(new RawUnit(BaseUnitType.Time, -2));

            // Act
            var physicalUnit = new PhysicalUnit(newton);

            // Assert
            Assert.Single(physicalUnit.BaseUnits);
            Assert.Equal(3, physicalUnit.BaseUnits.First().RawUnits.Count);
            Assert.True(physicalUnit.IsSI);
            Assert.Equal(StandardUnitSystem.SI, physicalUnit.UnitSystem);
        }

        [Fact]
        public void MultipleBaseUnits_WithDifferentExponents_Supported()
        {
            // Arrange - Example: N·m (torque)
            var newton = new BaseUnit
            {
                Symbol = "N",
                Exponent = new Fraction(1, 1),
                UnitSystem = StandardUnitSystem.SI,
                IsSI = true
            };

            var meter = new BaseUnit
            {
                Symbol = "m",
                Exponent = new Fraction(1, 1),
                UnitSystem = StandardUnitSystem.SI,
                IsSI = true
            };

            var torque = new PhysicalUnit { UnitType = UnitType.Torque_Mech };
            torque.BaseUnits.Add(newton);
            torque.BaseUnits.Add(meter);

            // Assert
            Assert.Equal(2, torque.BaseUnits.Count);
            Assert.True(torque.IsSI);
            Assert.Equal(StandardUnitSystem.SI, torque.UnitSystem);
        }
    }
}