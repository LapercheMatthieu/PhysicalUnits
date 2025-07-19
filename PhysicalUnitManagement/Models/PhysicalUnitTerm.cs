using Fractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  PhysicalUnitManagement.Models
{
    public class PhysicalUnitTerm
    {
        public PhysicalUnit Unit { get; set; }
        public Fraction Exponent { get; set; }
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
