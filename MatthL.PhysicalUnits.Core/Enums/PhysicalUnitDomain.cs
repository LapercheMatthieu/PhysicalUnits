using MatthL.PhysicalUnits.Core.EnumHelpers;
using MatthL.PhysicalUnits.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatthL.PhysicalUnits.Core.Enums
{
    /// <summary>
    /// Physical domains to categorize units by their application area
    /// </summary>
    public enum PhysicalUnitDomain
    {
        [Description("Fundamental units of the International System")]
        [DisplayedName("Base Units")]
        BaseUnits,

        [Description("Units related to mechanics and motion")]
        [DisplayedName("Mechanics")]
        Mechanics,

        [Description("Electrical and electromagnetic units")]
        [DisplayedName("Electricity")]
        Electricity,

        [Description("Units related to heat and thermodynamics")]
        [DisplayedName("Thermodynamics")]
        Thermodynamics,

        [Description("Units related to fluids and flow")]
        [DisplayedName("Fluidics")]
        Fluidics,

        [Description("Chemical units and concentration")]
        [DisplayedName("Chemistry")]
        Chemistry,

        [Description("Units related to light and radiation")]
        [DisplayedName("Optics")]
        Optics,

        [Description("Economic and monetary units")]
        [DisplayedName("Economics")]
        Economics,

        [Description("Computing and data units")]
        [DisplayedName("Computing")]
        Computing,

        [Description("Transportation and fuel units")]
        [DisplayedName("Transport")]
        Transport,

        [Description("Special units and dimensionless quantities")]
        [DisplayedName("Special")]
        Special,

        [Description("Undefined domain")]
        [DisplayedName("Undefined")]
        Undefined,
        Thermodynamique
    }
}
