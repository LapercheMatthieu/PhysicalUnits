using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatthL.PhysicalUnits.Enums
{
    /// <summary>
    /// Type de standardisation d'une unité
    /// </summary>
    public enum StandardUnitSystem
    {
        [Description("Système International d'unités")]
        [DisplayName("SI")]
        SI,

        [Description("Système métrique (non-SI)")]
        [DisplayName("Métrique")]
        Metrique,

        [Description("Système impérial britannique")]
        [DisplayName("Impérial")]
        Imperial,

        [Description("Système américain")]
        [DisplayName("US")]
        US,

        [Description("Unités astronomiques")]
        [DisplayName("Astronomique")]
        Astronomique,

        [Description("Système mixte (mélange de plusieurs systèmes)")]
        [DisplayName("Mixte")]
        Mixte,

        [Description("Autres systèmes")]
        [DisplayName("Autre")]
        Autre
    }
}
