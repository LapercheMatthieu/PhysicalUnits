using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatthL.PhysicalUnits.Core.EnumHelpers
{
    /// <summary>
    /// Attribut to specify a display name for the enums
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class DisplayedNameAttribute : Attribute
    {
        public string DisplayedName { get; }

        public DisplayedNameAttribute(string displayName)
        {
            DisplayedName = displayName;
        }
    }
}
