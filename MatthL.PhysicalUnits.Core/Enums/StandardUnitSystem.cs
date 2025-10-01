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
    /// Standardization system for physical units
    /// </summary>
    public enum StandardUnitSystem
    {
        [Description("International System of Units")]
        [DisplayedName("SI")]
        SI,

        [Description("Metric system (non-SI)")]
        [DisplayedName("Metric")]
        Metric,

        [Description("British Imperial system")]
        [DisplayedName("Imperial")]
        Imperial,

        [Description("United States customary system")]
        [DisplayedName("US")]
        US,

        [Description("Astronomical units")]
        [DisplayedName("Astronomical")]
        Astronomical,

        [Description("Mixed system (combination of multiple systems)")]
        [DisplayedName("Mixed")]
        Mixed,

        [Description("Other systems")]
        [DisplayedName("Other")]
        Other
    }
}
