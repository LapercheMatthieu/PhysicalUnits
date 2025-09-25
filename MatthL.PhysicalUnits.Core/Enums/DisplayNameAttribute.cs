using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatthL.PhysicalUnits.Enums
{
    /// <summary>
    /// Attribut pour spécifier un nom d'affichage
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class DisplayNameAttribute : Attribute
    {
        /// <summary>
        /// Nom d'affichage
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        /// Initialise une nouvelle instance de l'attribut DisplayName
        /// </summary>
        /// <param name="displayName">Nom d'affichage</param>
        public DisplayNameAttribute(string displayName)
        {
            DisplayName = displayName;
        }
    }
}
