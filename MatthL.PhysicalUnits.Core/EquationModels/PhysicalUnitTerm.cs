using Fractions;
using MatthL.PhysicalUnits.Core.Models;

namespace MatthL.PhysicalUnits.Core.EquationModels
{
    /// <summary>
    /// Class used for equations
    /// PhysicalUnitTerms is a physical unit + exponent
    /// we can then define that the result of the equation is the multiplication of all physicalunit with their exponents
    /// </summary>
    public class PhysicalUnitTerm
    {
        public PhysicalUnit Unit { get; set; }
        public Fraction Exponent { get; set; }

        public string DimensionalFormula => throw new NotImplementedException();

        public PhysicalUnitTerm(PhysicalUnit unit, Fraction exponent)
        {
            Unit = unit;
            Exponent = exponent;
        }

        public PhysicalUnitTerm()
        {
        }
    }
}