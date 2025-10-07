using MatthL.PhysicalUnits.Core.Enums;

namespace MatthL.PhysicalUnits.Core.EnumHelpers
{
    // This class contain all informations about prefixes
    public static class PrefixHelper
    {
        // Structure for the informations
        public class PrefixInfo
        {
            public string Symbol { get; }
            public string Name { get; }
            public decimal Size { get; }

            public PrefixInfo(string symbol, string name, decimal size)
            {
                Symbol = symbol;
                Name = name;
                Size = size;
            }
        }

        // Principal dictionary that attributes the informations to the prefix
        private static readonly Dictionary<Prefix, PrefixInfo> PrefixData = new Dictionary<Prefix, PrefixInfo>
    {
        { Prefix.yotta, new PrefixInfo("Y", "yotta", 1e+24m) },
        { Prefix.zetta, new PrefixInfo("Z", "zetta", 1e+21m) },
        { Prefix.exa, new PrefixInfo("E", "exa", 1e+18m) },
        { Prefix.peta, new PrefixInfo("P", "peta", 1e+15m) },
        { Prefix.tera, new PrefixInfo("T", "tera", 1e+12m) },
        { Prefix.giga, new PrefixInfo("G", "giga", 1e+9m) },
        { Prefix.mega, new PrefixInfo("M", "mega", 1e+6m) },
        { Prefix.kilo, new PrefixInfo("k", "kilo", 1e+3m) },
        { Prefix.hecto, new PrefixInfo("h", "hecto", 1e+2m) },
        { Prefix.deka, new PrefixInfo("da", "deca", 1e+1m) },
        { Prefix.SI, new PrefixInfo("", "", 1e0m) },
        { Prefix.deci, new PrefixInfo("d", "deci", 1e-1m) },
        { Prefix.centi, new PrefixInfo("c", "centi", 1e-2m) },
        { Prefix.milli, new PrefixInfo("m", "milli", 1e-3m) },
        { Prefix.micro, new PrefixInfo("μ", "micro", 1e-6m) },
        { Prefix.nano, new PrefixInfo("n", "nano", 1e-9m) },
        { Prefix.pico, new PrefixInfo("p", "pico", 1e-12m) },
        { Prefix.femto, new PrefixInfo("f", "femto", 1e-15m) },
        { Prefix.atto, new PrefixInfo("a", "atto", 1e-18m) },
        { Prefix.zepto, new PrefixInfo("z", "zepto", 1e-21m) },
        { Prefix.yocto, new PrefixInfo("y", "yocto", 1e-24m) }
    };

        // Secondary dictionaries for quick research
        private static readonly Dictionary<string, Prefix> SymbolToPrefix = new Dictionary<string, Prefix>();

        private static readonly Dictionary<string, Prefix> NameToPrefix = new Dictionary<string, Prefix>(StringComparer.OrdinalIgnoreCase);

        // Initialization of secondary Dictionaries
        static PrefixHelper()
        {
            foreach (var kvp in PrefixData)
            {
                if (!string.IsNullOrEmpty(kvp.Value.Symbol))
                    SymbolToPrefix[kvp.Value.Symbol] = kvp.Key;

                if (!string.IsNullOrEmpty(kvp.Value.Name))
                    NameToPrefix[kvp.Value.Name] = kvp.Key;
            }
        }

        // Data access
        public static PrefixInfo GetInfo(Prefix Prefix)
        {
            return PrefixData.TryGetValue(Prefix, out var info) ? info : null;
        }

        public static string GetSymbol(Prefix Prefix)
        {
            return GetInfo(Prefix)?.Symbol ?? "";
        }

        public static string GetName(Prefix Prefix)
        {
            return GetInfo(Prefix)?.Name ?? "";
        }

        public static decimal GetSize(Prefix Prefix)
        {
            return GetInfo(Prefix)?.Size ?? 0m;
        }

        // Recherche de préfixe par symbole ou nom
        public static Prefix? GetPrefixBySymbol(string symbol)
        {
            if (symbol == "" || symbol == string.Empty)
            {
                return Prefix.SI;
            }
            return SymbolToPrefix.TryGetValue(symbol, out var _Prefix) ? _Prefix : (Prefix?)null;
        }

        public static Prefix? GetPrefixByName(string name)
        {
            return NameToPrefix.TryGetValue(name, out var Prefix) ? Prefix : (Prefix?)null;
        }

        // Pour maintenir la compatibilité
        public static decimal PrefixSISize(Prefix Prefix)
        {
            return GetSize(Prefix);
        }

        public static string PrefixSISymbol(Prefix Prefix)
        {
            return GetSymbol(Prefix);
        }

        // Utilitaires supplémentaires
        public static decimal Convert(decimal value, Prefix fromPrefix, Prefix toPrefix)
        {
            return value * (GetSize(fromPrefix) / GetSize(toPrefix));
        }

        public static Prefix FindBestPrefix(decimal value)
        {
            // Ignorer le signe pour la détermination du préfixe
            value = Math.Abs(value);

            if (value == 0) return Prefix.SI;

            // Récupérer tous les préfixes
            var Prefixes = Enum.GetValues(typeof(Prefix));

            //the list is from bigger to lower we just take the first prefix for which the value is > 
            foreach (Prefix Prefix in Prefixes)
            {

                var size = GetSize(Prefix);
                if(value>= size) return Prefix;

                
            }

            return Prefix.SI; 
        }
    }

    // Extensions d'enum pour une syntaxe plus fluide
}