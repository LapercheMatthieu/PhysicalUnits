using MatthL.PhysicalUnits.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatthL.PhysicalUnits.Infrastructure.Extensions
{
    public static class RawUnitExtensions
    {
        public static RawUnit Clone(this RawUnit copiedUnit)
        {
            return new RawUnit()
            {
                UnitType = copiedUnit.UnitType,
                Exponent = copiedUnit.Exponent,
            };
        }
    }
}
