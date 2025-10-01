using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatthL.PhysicalUnits.Core.Abstractions
{
    /// <summary>
    /// An interface that specifies if a unit has a formula
    /// </summary>
    public interface IDimensionableUnit
    {
        public string DimensionalFormula { get; }
    }
}
