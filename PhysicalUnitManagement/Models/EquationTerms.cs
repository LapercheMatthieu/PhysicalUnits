using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace  PhysicalUnitManagement.Models
{
    public class EquationTerms
    {
        public IEnumerable<PhysicalUnitTerm> Terms { get; set; } = new List<PhysicalUnitTerm>();

        public EquationTerms(params  PhysicalUnitTerm[] terms)
        {
            Terms = terms;
        }
    }
}
