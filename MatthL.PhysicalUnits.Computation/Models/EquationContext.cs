using MatthL.PhysicalUnits.Core.EnumHelpers;
using MatthL.PhysicalUnits.Core.Enums;
using MatthL.PhysicalUnits.Core.EquationModels;
using MatthL.PhysicalUnits.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatthL.PhysicalUnits.Computation.Models
{
    /// <summary>
    /// Classe helper pour analyser le contexte
    /// </summary>
    public class EquationContext
    {
        public HashSet<UnitType> UnitTypes { get; } = new HashSet<UnitType>();
        public HashSet<PhysicalUnitDomain> Domains { get; } = new HashSet<PhysicalUnitDomain>();

        public bool HasForce { get; set; }
        public bool HasLength { get; set; }
        public bool HasTime { get; set; }
        public bool HasMass { get; set; }
        public bool HasTemperature { get; set; }
        public bool HasElectric { get; set; }
        public bool HasVolume { get; set; }
        public bool HasArea { get; set; }
        public bool HasSpeed { get; set; }
        public bool HasAcceleration { get; set; }
        public bool HasEnergy { get; set; }
        public bool HasPressure { get; set; }
        public bool HasDensity { get; set; }

        public EquationContext(PhysicalUnitTerm[] terms)
        {
            AnalyzeEquationContext(terms);

        }

        /// <summary>
        /// Fill the booleans for comparisons
        /// </summary>
        private void AnalyzeEquationContext(PhysicalUnitTerm[] terms)
        {
            foreach (var term in terms)
            {
                var unitType = term.Unit.UnitType;
                UnitTypes.Add(unitType);
                Domains.Add(unitType.GetDomain());

                // Analyser les types d'unités présents
                string typeStr = unitType.ToString();
                if (typeStr.Contains("Force")) this.HasForce = true;
                if (typeStr.Contains("Length")) this.HasLength = true;
                if (typeStr.Contains("Time")) this.HasTime = true;
                if (typeStr.Contains("Mass")) this.HasMass = true;
                if (typeStr.Contains("Temperature")) this.HasTemperature = true;
                if (typeStr.Contains("Electric")) this.HasElectric = true;
                if (typeStr.Contains("Volume")) this.HasVolume = true;
                if (typeStr.Contains("Area")) this.HasArea = true;
                if (typeStr.Contains("Speed")) this.HasSpeed = true;
                if (typeStr.Contains("Acceleration")) this.HasAcceleration = true;
                if (typeStr.Contains("Energy")) this.HasEnergy = true;
                if (typeStr.Contains("Pressure")) this.HasPressure = true;
                if (typeStr.Contains("Density")) this.HasDensity = true;
            }
        }

        /// <summary>
        /// Gives the score of a unit compared to the context
        /// </summary>
        public double CheckScore(PhysicalUnit unit)
        {
            double bonus = 0.0;
            string unitTypeStr = unit.UnitType.ToString();

            // Force × Distance
            if (this.HasForce && this.HasLength)
            {
                if (unitTypeStr.Contains("Energy"))
                {
                    bonus += 2.0; // Travail/Énergie est le cas le plus courant
                }
                else if (unitTypeStr.Contains("Torque"))
                {
                    bonus += 1.5; // Couple est aussi très courant
                }
            }

            // Mass × Acceleration
            if (this.HasMass && this.HasAcceleration)
            {
                if (unitTypeStr.Contains("Force")) bonus += 3.0; // Newton's 2nd law
            }

            // Force / Area
            if (this.HasForce && this.HasArea)
            {
                if (unitTypeStr.Contains("Pressure")) bonus += 3.0;
            }

            // Volume / Time
            if (this.HasVolume && this.HasTime)
            {
                if (unitTypeStr.Contains("VolumeFlow")) bonus += 3.0;
            }

            // Distance / Time
            if (this.HasLength && this.HasTime)
            {
                if (unitTypeStr.Contains("Speed")) bonus += 3.0;
            }

            // Energy / Time
            if (this.HasEnergy && this.HasTime)
            {
                if (unitTypeStr.Contains("Power")) bonus += 3.0;
            }

            // Électricité : V × A = W
            if (this.UnitTypes.Any(t => t.ToString().Contains("ElectricPotential")) &&
                this.UnitTypes.Any(t => t.ToString().Contains("ElectricCurrent")))
            {
                if (unitTypeStr.Contains("Power")) bonus += 3.0;
            }

            // Thermodynamique
            if (this.HasTemperature)
            {
                if (this.Domains.Contains(PhysicalUnitDomain.Thermodynamics))
                {
                    if (unit.UnitType.GetDomain() == PhysicalUnitDomain.Thermodynamique) bonus += 2.0;
                }
            }

            // Mécanique des fluides
            if (this.HasDensity && this.HasSpeed)
            {
                if (unitTypeStr.Contains("Pressure")) bonus += 2.0; // Pression dynamique
            }

            // Pression × Volume = Énergie
            if (this.HasPressure && this.HasVolume)
            {
                if (unitTypeStr.Contains("Energy")) bonus += 2.5;
            }

            return bonus;
        }
    }
}

