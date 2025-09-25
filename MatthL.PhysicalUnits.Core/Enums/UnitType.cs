using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatthL.PhysicalUnits.Enums
{
    /// <summary>
    /// Types d'unités physiques organisés par domaine
    /// Convention : Type_Domain (ex: Force_Mech, Length_Base)
    /// </summary>
    public enum UnitType
    {
        // ===== DIMENSIONS DE BASE (7 SI + extensions) =====
        Length_Base,
        Mass_Base,
        Time_Base,
        ElectricCurrent_Base,
        Temperature_Base,
        AmountOfSubstance_Base,
        LuminousIntensity_Base,
        Angle_Base,             // Extension pour les angles
        Ratio_Base,             // Extension pour les ratios/pourcentages
        Currency_Base,          // Extension pour les devises
        Information_Base,       // Extension pour les données

        // ===== MÉCANIQUE =====
        // Géométrie
        Area_Mech,
        Volume_Mech,
        AreaMomentOfInertia_Mech,
        MassMomentOfInertia_Mech,
        WarpingMomentOfInertia_Mech,

        // Cinématique
        Speed_Mech,
        Acceleration_Mech,
        Jerk_Mech,
        RotationalSpeed_Mech,

        // Dynamique
        Force_Mech,
        Torque_Mech,
        ForcePerLength_Mech,
        TorquePerLength_Mech,
        ForceChangeRate_Mech,

        // Pression et contraintes
        Pressure_Mech,
        PressureChangeRate_Mech,

        // Énergie et puissance
        Energy_Mech,
        Power_Mech,
        SpecificEnergy_Mech,
        PowerDensity_Mech,
        LinearPowerDensity_Mech,

        // Propriétés des matériaux
        Density_Mech,
        LinearDensity_Mech,
        AreaDensity_Mech,
        SpecificVolume_Mech,
        SpecificWeight_Mech,

        // Autres
        Frequency_Mech,

        // ===== FLUIDIQUE =====
        VolumeFlow_Fluid,
        MassFlow_Fluid,
        MassFlux_Fluid,
        VolumePerLength_Fluid,
        DynamicViscosity_Fluid,
        KinematicViscosity_Fluid,

        // ===== THERMODYNAMIQUE =====
        HeatFlux_Thermo,
        HeatTransferCoefficient_Thermo,
        ThermalConductivity_Thermo,
        ThermalResistance_Thermo,
        SpecificThermalResistance_Thermo,
        VolumetricHeatTransferCoefficient_Thermo,

        // Capacités thermiques
        SpecificHeatCapacity_Thermo,
        Enthalpy_Thermo,
        Entropy_Thermo,
        SpecificEntropy_Thermo,

        // Propriétés thermiques
        CoefficientOfThermalExpansion_Thermo,
        TemperatureChangeRate_Thermo,
        LapseRate_Thermo,

        // ===== ÉLECTRICITÉ & MAGNÉTISME =====
        // Électricité de base
        ElectricCharge_Elec,
        ElectricPotential_Elec,
        ElectricResistance_Elec,
        ElectricConductance_Elec,
        Capacitance_Elec,
        ElectricInductance_Elec,

        // Champs et flux
        ElectricField_Elec,
        MagneticField_Elec,
        MagneticFlux_Elec,
        Magnetization_Elec,

        // Propriétés électriques
        ElectricConductivity_Elec,
        ElectricResistivity_Elec,
        Permittivity_Elec,
        Permeability_Elec,

        // Densités et gradients
        ElectricChargeDensity_Elec,
        ElectricSurfaceChargeDensity_Elec,
        ElectricCurrentDensity_Elec,
        ElectricCurrentGradient_Elec,
        ElectricPotentialChangeRate_Elec,

        // Puissance électrique
        ApparentPower_Elec,
        ReactivePower_Elec,
        ApparentEnergy_Elec,
        ReactiveEnergy_Elec,

        // ===== CHIMIE =====
        Molarity_Chem,
        MolarMass_Chem,
        MolarFlow_Chem,
        MolarEnergy_Chem,
        MolarEntropy_Chem,

        // ===== OPTIQUE & RADIATION =====
        Illuminance_Optic,
        LuminousFlux_Optic,
        Irradiance_Optic,
        Irradiation_Optic,

        // ===== INFORMATIQUE =====
        BitRate_Info,

        // ===== TRANSPORT & CARBURANT =====
        FuelEfficiency_Transport,
        BrakeSpecificFuelConsumption_Transport,

        // ===== ÉCONOMIE =====
        Cost_Econ,
        EnergyCost_Econ,
        PowerCost_Econ,
        MassCost_Econ,
        LengthCost_Econ,
        VolumeCost_Econ,
        AreaCost_Econ,

        // ===== UNITÉS SPÉCIALES =====
        Level_Special,          // Décibels, etc.
        RelativeHumidity_Special,
        VolumeConcentration_Special,
        Unknown_Special,
    }

    /// <summary>
    /// Extensions pour faciliter l'utilisation de UnitType
    /// </summary>
    public static class UnitTypeExtensions
    {
        /// <summary>
        /// Obtient le domaine d'un type d'unité
        /// </summary>
        public static PhysicalUnitDomain GetDomain(this UnitType unitType)
        {
            var parts = unitType.ToString().Split('_');
            if (parts.Length < 2) return PhysicalUnitDomain.NonDefini;

            return parts[1] switch
            {
                "Base" => PhysicalUnitDomain.DimensionsDeBase,
                "Mech" => PhysicalUnitDomain.Mecanique,
                "Fluid" => PhysicalUnitDomain.Fluidique,
                "Thermo" => PhysicalUnitDomain.Thermodynamique,
                "Elec" => PhysicalUnitDomain.Electricite,
                "Chem" => PhysicalUnitDomain.Chimie,
                "Optic" => PhysicalUnitDomain.Optique,
                "Info" => PhysicalUnitDomain.Informatique,
                "Transport" => PhysicalUnitDomain.Transport,
                "Econ" => PhysicalUnitDomain.Economique,
                "Special" => PhysicalUnitDomain.Special,
                _ => PhysicalUnitDomain.NonDefini
            };
        }

        /// <summary>
        /// Obtient le nom court du type (sans le domaine)
        /// </summary>
        public static string GetShortName(this UnitType unitType)
        {
            var parts = unitType.ToString().Split('_');
            var result = parts.Length > 0 ? parts[0] : unitType.ToString();

            // Ajouter des espaces avant les majuscules (sauf la première)
            return System.Text.RegularExpressions.Regex.Replace(
                result,
                "(?<!^)(?=[A-Z])",
                " "
            );
        }

        /// <summary>
        /// Obtient tous les types d'un domaine spécifique
        /// </summary>
        public static IEnumerable<UnitType> GetTypesForDomain(PhysicalUnitDomain domain)
        {
            var domainSuffix = domain switch
            {
                PhysicalUnitDomain.DimensionsDeBase => "_Base",
                PhysicalUnitDomain.Mecanique => "_Mech",
                PhysicalUnitDomain.Fluidique => "_Fluid",
                PhysicalUnitDomain.Thermodynamique => "_Thermo",
                PhysicalUnitDomain.Electricite => "_Elec",
                PhysicalUnitDomain.Chimie => "_Chem",
                PhysicalUnitDomain.Optique => "_Optic",
                PhysicalUnitDomain.Informatique => "_Info",
                PhysicalUnitDomain.Transport => "_Transport",
                PhysicalUnitDomain.Economique => "_Econ",
                PhysicalUnitDomain.Special => "_Special",
                _ => ""
            };

            if (string.IsNullOrEmpty(domainSuffix))
                return Enumerable.Empty<UnitType>();

            return Enum.GetValues<UnitType>()
                .Where(t => t.ToString().EndsWith(domainSuffix));
        }
    }
}
