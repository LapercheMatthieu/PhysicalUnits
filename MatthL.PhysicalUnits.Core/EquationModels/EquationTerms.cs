using MatthL.PhysicalUnits.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace MatthL.PhysicalUnits.Core.EquationModels
{
    /// <summary>
    /// Class to be used for equations
    /// composed of Physical Unit Term in the list Terms
    /// PhysicalUnitTerms is a physical unit + exponent 
    /// we can then define that the result of the equation is the multiplication of all physicalunit with their exponents
    /// </summary>
    public class EquationTerms
    {
        public IEnumerable<PhysicalUnitTerm> Terms { get; set; } = new List<PhysicalUnitTerm>();

        public EquationTerms(params  PhysicalUnitTerm[] terms)
        {
            Terms = terms;
        }
    }
}
