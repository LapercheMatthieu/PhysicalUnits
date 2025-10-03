using Fractions;
using MatthL.PhysicalUnits.Computation.Models;
using MatthL.PhysicalUnits.Core.Enums;
using MatthL.PhysicalUnits.Core.EquationModels;
using MatthL.PhysicalUnits.Core.Models;
using Xunit;

namespace MatthL.PhysicalUnits.Tests.Computation
{
    public class EquationContextTests
    {
        [Fact]
        public void Constructor_AnalyzesForce_SetsHasForce()
        {
            // Arrange
            var force = CreateForcePhysicalUnit();
            var terms = new[] { new PhysicalUnitTerm(force, new Fraction(1)) };

            // Act
            var context = new EquationContext(terms);

            // Assert
            Assert.True(context.HasForce);
        }

        [Fact]
        public void Constructor_AnalyzesLength_SetsHasLength()
        {
            // Arrange
            var length = new PhysicalUnit(CreateLengthBaseUnit());
            var terms = new[] { new PhysicalUnitTerm(length, new Fraction(1)) };

            // Act
            var context = new EquationContext(terms);

            // Assert
            Assert.True(context.HasLength);
        }

        [Fact]
        public void Constructor_AnalyzesTime_SetsHasTime()
        {
            // Arrange
            var time = CreateTimePhysicalUnit();
            var terms = new[] { new PhysicalUnitTerm(time, new Fraction(1)) };

            // Act
            var context = new EquationContext(terms);

            // Assert
            Assert.True(context.HasTime);
        }

        [Fact]
        public void Constructor_AnalyzesMass_SetsHasMass()
        {
            // Arrange
            var mass = new PhysicalUnit(CreateMassBaseUnit());
            var terms = new[] { new PhysicalUnitTerm(mass, new Fraction(1)) };

            // Act
            var context = new EquationContext(terms);

            // Assert
            Assert.True(context.HasMass);
        }

        [Fact]
        public void Constructor_AnalyzesSpeed_SetsHasSpeed()
        {
            // Arrange
            var speed = CreateSpeedPhysicalUnit();
            var terms = new[] { new PhysicalUnitTerm(speed, new Fraction(1)) };

            // Act
            var context = new EquationContext(terms);

            // Assert
            Assert.True(context.HasSpeed);
        }

        [Fact]
        public void Constructor_AnalyzesAcceleration_SetsHasAcceleration()
        {
            // Arrange
            var acceleration = CreateAccelerationPhysicalUnit();
            var terms = new[] { new PhysicalUnitTerm(acceleration, new Fraction(1)) };

            // Act
            var context = new EquationContext(terms);

            // Assert
            Assert.True(context.HasAcceleration);
        }

        [Fact]
        public void Constructor_AnalyzesEnergy_SetsHasEnergy()
        {
            // Arrange
            var energy = CreateEnergyPhysicalUnit();
            var terms = new[] { new PhysicalUnitTerm(energy, new Fraction(1)) };

            // Act
            var context = new EquationContext(terms);

            // Assert
            Assert.True(context.HasEnergy);
        }

        [Fact]
        public void Constructor_AnalyzesPressure_SetsHasPressure()
        {
            // Arrange
            var pressure = CreatePressurePhysicalUnit();
            var terms = new[] { new PhysicalUnitTerm(pressure, new Fraction(1)) };

            // Act
            var context = new EquationContext(terms);

            // Assert
            Assert.True(context.HasPressure);
        }

        [Fact]
        public void Constructor_AnalyzesVolume_SetsHasVolume()
        {
            // Arrange
            var volume = CreateVolumePhysicalUnit();
            var terms = new[] { new PhysicalUnitTerm(volume, new Fraction(1)) };

            // Act
            var context = new EquationContext(terms);

            // Assert
            Assert.True(context.HasVolume);
        }

        [Fact]
        public void Constructor_AnalyzesArea_SetsHasArea()
        {
            // Arrange
            var area = CreateAreaPhysicalUnit();
            var terms = new[] { new PhysicalUnitTerm(area, new Fraction(1)) };

            // Act
            var context = new EquationContext(terms);

            // Assert
            Assert.True(context.HasArea);
        }

        [Fact]
        public void Constructor_PopulatesUnitTypes()
        {
            // Arrange
            var force = CreateForcePhysicalUnit();
            var length = new PhysicalUnit(CreateLengthBaseUnit());
            var terms = new[]
            {
                new PhysicalUnitTerm(force, new Fraction(1)),
                new PhysicalUnitTerm(length, new Fraction(1))
            };

            // Act
            var context = new EquationContext(terms);

            // Assert
            Assert.Contains(UnitType.Force_Mech, context.UnitTypes);
            Assert.Contains(UnitType.Length_Base, context.UnitTypes);
        }

        [Fact]
        public void Constructor_PopulatesDomains()
        {
            // Arrange
            var force = CreateForcePhysicalUnit();
            var terms = new[] { new PhysicalUnitTerm(force, new Fraction(1)) };

            // Act
            var context = new EquationContext(terms);

            // Assert
            Assert.Contains(PhysicalUnitDomain.Mechanics, context.Domains);
        }

        [Fact]
        public void CheckScore_ForceAndLength_PrefersEnergy()
        {
            // Arrange - Force × Distance context
            var force = CreateForcePhysicalUnit();
            var length = new PhysicalUnit(CreateLengthBaseUnit());
            var terms = new[]
            {
                new PhysicalUnitTerm(force, new Fraction(1)),
                new PhysicalUnitTerm(length, new Fraction(1))
            };
            var context = new EquationContext(terms);

            var energy = CreateEnergyPhysicalUnit();
            var torque = CreateTorquePhysicalUnit();

            // Act
            var energyScore = context.CheckScore(energy);
            var torqueScore = context.CheckScore(torque);

            // Assert
            Assert.True(energyScore > torqueScore);
        }

        [Fact]
        public void CheckScore_MassAndAcceleration_PrefersForce()
        {
            // Arrange - F = m × a context
            var mass = new PhysicalUnit(CreateMassBaseUnit());
            var acceleration = CreateAccelerationPhysicalUnit();
            var terms = new[]
            {
                new PhysicalUnitTerm(mass, new Fraction(1)),
                new PhysicalUnitTerm(acceleration, new Fraction(1))
            };
            var context = new EquationContext(terms);

            var force = CreateForcePhysicalUnit();

            // Act
            var score = context.CheckScore(force);

            // Assert
            Assert.True(score >= 3.0); // Newton's 2nd law bonus
        }

        [Fact]
        public void CheckScore_ForceAndArea_PrefersPressure()
        {
            // Arrange - P = F/A context
            var force = CreateForcePhysicalUnit();
            var area = CreateAreaPhysicalUnit();
            var terms = new[]
            {
                new PhysicalUnitTerm(force, new Fraction(1)),
                new PhysicalUnitTerm(area, new Fraction(-1))
            };
            var context = new EquationContext(terms);

            var pressure = CreatePressurePhysicalUnit();

            // Act
            var score = context.CheckScore(pressure);

            // Assert
            Assert.True(score >= 3.0);
        }

        [Fact]
        public void CheckScore_VolumeAndTime_PrefersVolumeFlow()
        {
            // Arrange - Q = V/t context
            var volume = CreateVolumePhysicalUnit();
            var time = CreateTimePhysicalUnit();
            var terms = new[]
            {
                new PhysicalUnitTerm(volume, new Fraction(1)),
                new PhysicalUnitTerm(time, new Fraction(-1))
            };
            var context = new EquationContext(terms);

            var volumeFlow = CreateVolumeFlowPhysicalUnit();

            // Act
            var score = context.CheckScore(volumeFlow);

            // Assert
            Assert.True(score >= 3.0);
        }

        [Fact]
        public void CheckScore_LengthAndTime_PrefersSpeed()
        {
            // Arrange - v = d/t context
            var length = new PhysicalUnit(CreateLengthBaseUnit());
            var time = CreateTimePhysicalUnit();
            var terms = new[]
            {
                new PhysicalUnitTerm(length, new Fraction(1)),
                new PhysicalUnitTerm(time, new Fraction(-1))
            };
            var context = new EquationContext(terms);

            var speed = CreateSpeedPhysicalUnit();

            // Act
            var score = context.CheckScore(speed);

            // Assert
            Assert.True(score >= 3.0);
        }

        [Fact]
        public void CheckScore_EnergyAndTime_PrefersPower()
        {
            // Arrange - P = E/t context
            var energy = CreateEnergyPhysicalUnit();
            var time = CreateTimePhysicalUnit();
            var terms = new[]
            {
                new PhysicalUnitTerm(energy, new Fraction(1)),
                new PhysicalUnitTerm(time, new Fraction(-1))
            };
            var context = new EquationContext(terms);

            var power = CreatePowerPhysicalUnit();

            // Act
            var score = context.CheckScore(power);

            // Assert
            Assert.True(score >= 3.0);
        }

        [Fact]
        public void CheckScore_UnrelatedUnit_ReturnsLowScore()
        {
            // Arrange
            var force = CreateForcePhysicalUnit();
            var terms = new[] { new PhysicalUnitTerm(force, new Fraction(1)) };
            var context = new EquationContext(terms);

            var temperature = CreateTemperaturePhysicalUnit();

            // Act
            var score = context.CheckScore(temperature);

            // Assert
            Assert.True(score < 1.0); // Should have no bonus
        }

        // Helper methods to create test units
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

        private PhysicalUnit CreateTimePhysicalUnit()
        {
            var baseUnit = new BaseUnit
            {
                Name = "Second",
                Symbol = "s",
                UnitType = UnitType.Time_Base,
                UnitSystem = StandardUnitSystem.SI,
                IsSI = true,
                Prefix = Prefix.SI,
                Exponent = new Fraction(1)
            };
            baseUnit.RawUnits.Add(new RawUnit(BaseUnitType.Time, 1));
            return new PhysicalUnit(baseUnit);
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

        private PhysicalUnit CreateAccelerationPhysicalUnit()
        {
            var baseUnit = new BaseUnit
            {
                Name = "MeterPerSecondSquared",
                Symbol = "m/s²",
                UnitType = UnitType.Acceleration_Mech,
                UnitSystem = StandardUnitSystem.SI,
                IsSI = true,
                Prefix = Prefix.SI,
                Exponent = new Fraction(1)
            };
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

        private PhysicalUnit CreateTorquePhysicalUnit()
        {
            var baseUnit = new BaseUnit
            {
                Name = "NewtonMeter",
                Symbol = "N·m",
                UnitType = UnitType.Torque_Mech,
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

        private PhysicalUnit CreatePressurePhysicalUnit()
        {
            var baseUnit = new BaseUnit
            {
                Name = "Pascal",
                Symbol = "Pa",
                UnitType = UnitType.Pressure_Mech,
                UnitSystem = StandardUnitSystem.SI,
                IsSI = true,
                Prefix = Prefix.SI,
                Exponent = new Fraction(1)
            };
            baseUnit.RawUnits.Add(new RawUnit(BaseUnitType.Mass, 1));
            baseUnit.RawUnits.Add(new RawUnit(BaseUnitType.Length, -1));
            baseUnit.RawUnits.Add(new RawUnit(BaseUnitType.Time, -2));
            return new PhysicalUnit(baseUnit);
        }

        private PhysicalUnit CreateVolumePhysicalUnit()
        {
            var baseUnit = new BaseUnit
            {
                Name = "CubicMeter",
                Symbol = "m³",
                UnitType = UnitType.Volume_Mech,
                UnitSystem = StandardUnitSystem.SI,
                IsSI = true,
                Prefix = Prefix.SI,
                Exponent = new Fraction(1)
            };
            baseUnit.RawUnits.Add(new RawUnit(BaseUnitType.Length, 3));
            return new PhysicalUnit(baseUnit);
        }

        private PhysicalUnit CreateAreaPhysicalUnit()
        {
            var baseUnit = new BaseUnit
            {
                Name = "SquareMeter",
                Symbol = "m²",
                UnitType = UnitType.Area_Mech,
                UnitSystem = StandardUnitSystem.SI,
                IsSI = true,
                Prefix = Prefix.SI,
                Exponent = new Fraction(1)
            };
            baseUnit.RawUnits.Add(new RawUnit(BaseUnitType.Length, 2));
            return new PhysicalUnit(baseUnit);
        }

        private PhysicalUnit CreateVolumeFlowPhysicalUnit()
        {
            var baseUnit = new BaseUnit
            {
                Name = "CubicMeterPerSecond",
                Symbol = "m³/s",
                UnitType = UnitType.VolumeFlow_Fluid,
                UnitSystem = StandardUnitSystem.SI,
                IsSI = true,
                Prefix = Prefix.SI,
                Exponent = new Fraction(1)
            };
            baseUnit.RawUnits.Add(new RawUnit(BaseUnitType.Length, 3));
            baseUnit.RawUnits.Add(new RawUnit(BaseUnitType.Time, -1));
            return new PhysicalUnit(baseUnit);
        }

        private PhysicalUnit CreatePowerPhysicalUnit()
        {
            var baseUnit = new BaseUnit
            {
                Name = "Watt",
                Symbol = "W",
                UnitType = UnitType.Power_Mech,
                UnitSystem = StandardUnitSystem.SI,
                IsSI = true,
                Prefix = Prefix.SI,
                Exponent = new Fraction(1)
            };
            baseUnit.RawUnits.Add(new RawUnit(BaseUnitType.Mass, 1));
            baseUnit.RawUnits.Add(new RawUnit(BaseUnitType.Length, 2));
            baseUnit.RawUnits.Add(new RawUnit(BaseUnitType.Time, -3));
            return new PhysicalUnit(baseUnit);
        }

        private PhysicalUnit CreateTemperaturePhysicalUnit()
        {
            var baseUnit = new BaseUnit
            {
                Name = "Kelvin",
                Symbol = "K",
                UnitType = UnitType.Temperature_Base,
                UnitSystem = StandardUnitSystem.SI,
                IsSI = true,
                Prefix = Prefix.SI,
                Exponent = new Fraction(1)
            };
            baseUnit.RawUnits.Add(new RawUnit(BaseUnitType.Temperature, 1));
            return new PhysicalUnit(baseUnit);
        }
    }
}