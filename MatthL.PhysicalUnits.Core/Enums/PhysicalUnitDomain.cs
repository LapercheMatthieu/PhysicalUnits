using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatthL.PhysicalUnits.Enums
{
    /// <summary>
    /// Domaines d'unités physiques avec attributs de description
    /// </summary>
    public enum PhysicalUnitDomain
    {
        [Description("Unités fondamentales du système international")]
        [DisplayName("Unités Standards")]
        DimensionsDeBase,

        [Description("Unités liées à la mécanique et au mouvement")]
        [DisplayName("Mécanique")]
        Mecanique,

        [Description("Unités électriques et électromagnétiques")]
        [DisplayName("Electricité")]
        Electricite,

        [Description("Unités liées à la chaleur et à la thermodynamique")]
        [DisplayName("Thermodynamique")]
        Thermodynamique,

        [Description("Unités liées aux fluides et écoulements")]
        [DisplayName("Fluide")]
        Fluidique,

        [Description("Unités chimiques et de concentration")]
        [DisplayName("Chimie")]
        Chimie,

        [Description("Unités liées à la lumière et aux radiations")]
        [DisplayName("Optique")]
        Optique,

        [Description("Unités de coût et économiques")]
        [DisplayName("Economique")]
        Economique,

        [Description("Unités informatiques et de données")]
        [DisplayName("Informatique")]
        Informatique,

        [Description("Unités de transport et carburant")]
        [DisplayName("Transport")]
        Transport,

        [Description("Unités spéciales et sans dimension")]
        [DisplayName("Spécial")]
        Special,

        [Description("Domaine non défini")]
        [DisplayName("Autres")]
        NonDefini
    }
}
