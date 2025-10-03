using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatthL.PhysicalUnits.Core.EnumHelpers;
using MatthL.PhysicalUnits.Core.Enums;
using Xunit;

namespace MatthL.PhysicalUnits.Tests.Core.EnumHelpers
{
    public class BaseUnitTypeExtensionsTests
    {
        [Theory]
        [InlineData(BaseUnitType.Length, "m")]
        [InlineData(BaseUnitType.Mass, "kg")]
        [InlineData(BaseUnitType.Time, "s")]
        [InlineData(BaseUnitType.ElectricCurrent, "A")]
        [InlineData(BaseUnitType.Temperature, "K")]
        [InlineData(BaseUnitType.AmountOfSubstance, "mol")]
        [InlineData(BaseUnitType.LuminousIntensity, "cd")]
        [InlineData(BaseUnitType.Angle, "rad")]
        [InlineData(BaseUnitType.Currency, "$")]
        [InlineData(BaseUnitType.Information, "bit")]
        [InlineData(BaseUnitType.Ratio, "1")]
        public void GetBaseSymbol_ReturnsCorrectSymbol(BaseUnitType type, string expected)
        {
            // Arrange & Act
            var result = type.GetBaseSymbol();

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(BaseUnitType.Length, true)]
        [InlineData(BaseUnitType.Mass, true)]
        [InlineData(BaseUnitType.Time, true)]
        [InlineData(BaseUnitType.ElectricCurrent, true)]
        [InlineData(BaseUnitType.Temperature, true)]
        [InlineData(BaseUnitType.AmountOfSubstance, true)]
        [InlineData(BaseUnitType.LuminousIntensity, true)]
        [InlineData(BaseUnitType.Angle, false)]
        [InlineData(BaseUnitType.Currency, false)]
        [InlineData(BaseUnitType.Information, false)]
        [InlineData(BaseUnitType.Ratio, false)]
        public void IsPhysicalBase_ReturnsCorrectValue(BaseUnitType type, bool expected)
        {
            // Arrange & Act
            var result = type.IsPhysicalBase();

            // Assert
            Assert.Equal(expected, result);
        }
    }

    public class UnitTypeExtensionsTests
    {
        [Theory]
        [InlineData(UnitType.Length_Base, PhysicalUnitDomain.BaseUnits)]
        [InlineData(UnitType.Force_Mech, PhysicalUnitDomain.Mechanics)]
        [InlineData(UnitType.VolumeFlow_Fluid, PhysicalUnitDomain.Fluidics)]
        [InlineData(UnitType.Temperature_Base, PhysicalUnitDomain.BaseUnits)]
        [InlineData(UnitType.HeatFlux_Thermo, PhysicalUnitDomain.Thermodynamics)]
        [InlineData(UnitType.ElectricCharge_Elec, PhysicalUnitDomain.Electricity)]
        [InlineData(UnitType.Molarity_Chem, PhysicalUnitDomain.Chemistry)]
        [InlineData(UnitType.Illuminance_Optic, PhysicalUnitDomain.Optics)]
        [InlineData(UnitType.BitRate_Info, PhysicalUnitDomain.Computing)]
        [InlineData(UnitType.FuelEfficiency_Transport, PhysicalUnitDomain.Transport)]
        [InlineData(UnitType.Cost_Econ, PhysicalUnitDomain.Economics)]
        [InlineData(UnitType.Level_Special, PhysicalUnitDomain.Special)]
        public void GetDomain_ReturnsCorrectDomain(UnitType unitType, PhysicalUnitDomain expected)
        {
            // Arrange & Act
            var result = unitType.GetDomain();

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(UnitType.Force_Mech, "Force")]
        [InlineData(UnitType.VolumeFlow_Fluid, "Volume Flow")]
        [InlineData(UnitType.HeatFlux_Thermo, "Heat Flux")]
        [InlineData(UnitType.ElectricCharge_Elec, "Electric Charge")]
        [InlineData(UnitType.MassMomentOfInertia_Mech, "Mass Moment Of Inertia")]
        public void GetShortName_FormatsCorrectly(UnitType unitType, string expected)
        {
            // Arrange & Act
            var result = unitType.GetShortName();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetTypesForDomain_Mechanics_ReturnsOnlyMechanicsTypes()
        {
            // Arrange & Act
            var types = UnitTypeExtensions.GetTypesForDomain(PhysicalUnitDomain.Mechanics);

            // Assert
            Assert.NotEmpty(types);
            Assert.All(types, t => Assert.Equal(PhysicalUnitDomain.Mechanics, t.GetDomain()));
        }

        [Fact]
        public void GetTypesForDomain_BaseUnits_ReturnsOnlyBaseTypes()
        {
            // Arrange & Act
            var types = UnitTypeExtensions.GetTypesForDomain(PhysicalUnitDomain.BaseUnits);

            // Assert
            Assert.NotEmpty(types);
            Assert.All(types, t => Assert.Equal(PhysicalUnitDomain.BaseUnits, t.GetDomain()));
        }

        [Fact]
        public void GetTypesForDomain_Undefined_ReturnsEmpty()
        {
            // Arrange & Act
            var types = UnitTypeExtensions.GetTypesForDomain(PhysicalUnitDomain.Undefined);

            // Assert
            Assert.Empty(types);
        }
    }
}