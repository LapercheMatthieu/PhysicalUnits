using Fractions;
using MatthL.PhysicalUnits.Core.EnumHelpers;
using MatthL.PhysicalUnits.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatthL.PhysicalUnits.Computation.Extensions
{
    /// <summary>
    /// Extensions of the Dictionary<BaseUnitType, Fraction> widely used for Dimensions
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Filter non physical dimensions to verify the homogeneity not counting 
        /// the angle ratio etc...
        /// </summary>
        public static Dictionary<BaseUnitType, Fraction> FilterPhysicalDimensions(this Dictionary<BaseUnitType, Fraction> dimensions)
        {
            return dimensions
                .Where(d => d.Key.IsPhysicalBase())
                .ToDictionary(d => d.Key, d => d.Value);
        }



        /// <summary>
        /// Compare two dimensions vor equality
        /// </summary>
        public static bool IsDimensionsEqualTo(
            this Dictionary<BaseUnitType, Fraction> dim1,
            Dictionary<BaseUnitType, Fraction> dim2)
        {
            // Vérifier que toutes les clés sont identiques
            var keys1 = dim1.Keys.OrderBy(k => k).ToList();
            var keys2 = dim2.Keys.OrderBy(k => k).ToList();

            if (keys1.Count != keys2.Count)
                return false;

            for (int i = 0; i < keys1.Count; i++)
            {
                if (keys1[i] != keys2[i])
                    return false;

                // Vérifier que les exposants sont égaux
                if (dim1[keys1[i]] != dim2[keys2[i]])
                    return false;
            }

            return true;
        }
    }
}
