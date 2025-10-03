using MatthL.PhysicalUnits.Core.Enums;

namespace MatthL.PhysicalUnits.Core.EnumHelpers
{
    public static class PrefixExtensions
    {
        public static string GetSymbol(this Prefix Prefix) => PrefixHelper.GetSymbol(Prefix);

        public static string GetName(this Prefix Prefix) => PrefixHelper.GetName(Prefix);

        public static decimal GetSize(this Prefix Prefix) => PrefixHelper.GetSize(Prefix);

        public static decimal ConvertTo(this decimal value, Prefix fromPrefix, Prefix toPrefix)
        {
            return PrefixHelper.Convert(value, fromPrefix, toPrefix);
        }
    }
}