using MatthL.PhysicalUnits.Core.EnumHelpers;
using MatthL.PhysicalUnits.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatthL.PhysicalUnits.Core.Models
{
    // Classe qui encapsule une valeur avec son préfixe SI A SUPPRIMER
    public class SIValue
    {
        public decimal Value { get; }
        public Prefix Prefix { get; }

        public decimal BaseValue => Value * Prefix.GetSize();

        public SIValue(decimal value, Prefix Prefix)
        {
            Value = value;
            Prefix = Prefix;
        }

        public static SIValue FromBaseValue(decimal baseValue)
        {
            var bestPrefix = PrefixHelper.FindBestPrefix(baseValue);
            var value = baseValue / bestPrefix.GetSize();
            return new SIValue(value, bestPrefix);
        }

        public SIValue ConvertTo(Prefix targetPrefix)
        {
            var newValue = Value * (Prefix.GetSize() / targetPrefix.GetSize());
            return new SIValue(newValue, targetPrefix);
        }

        public override string ToString()
        {
            return $"{Value} {Prefix.GetSymbol()}";
        }

        public string ToString(string unit)
        {
            return $"{Value} {Prefix.GetSymbol()}{unit}";
        }

        public string ToStringWithName(string unit)
        {
            return $"{Value} {Prefix.GetName()}{unit}";
        }
    }
}
