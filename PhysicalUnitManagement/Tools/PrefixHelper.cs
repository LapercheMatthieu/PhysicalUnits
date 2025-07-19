
    using  PhysicalUnitManagement.Enums;
    using System;
    using System.Collections.Generic;

    // Cette classe contient toutes les informations liées aux préfixes SI
    public static class PrefixHelper
    {
        // Structure pour contenir les infos de chaque préfixe
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

        // Dictionnaire principal qui associe chaque Prefix à ses informations
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

        // Dictionnaires secondaires pour les recherches inversées
        private static readonly Dictionary<string, Prefix> SymbolToPrefix = new Dictionary<string, Prefix>(StringComparer.OrdinalIgnoreCase);
        private static readonly Dictionary<string, Prefix> NameToPrefix = new Dictionary<string, Prefix>(StringComparer.OrdinalIgnoreCase);

        // Initialisation des dictionnaires secondaires
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

        // Méthodes d'accès aux données
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
            return SymbolToPrefix.TryGetValue(symbol, out var Prefix) ? Prefix : (Prefix?)null;
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

            Prefix bestPrefix = Prefix.SI;
            decimal bestDiff = decimal.MaxValue;

            foreach (Prefix Prefix in Prefixes)
            {
                var size = GetSize(Prefix);
                if (size <= 0) continue;

                // Chercher le préfixe qui minimise le nombre de chiffres avant la virgule
                var scaledValue = value / size;
                if (scaledValue >= 1 && scaledValue < 1000)
                {
                    var diff = Math.Abs(1 - scaledValue);
                    if (diff < bestDiff)
                    {
                        bestDiff = diff;
                        bestPrefix = Prefix;
                    }
                }
            }

            return bestPrefix;
        }
    }

    // Extensions d'enum pour une syntaxe plus fluide
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

    // Classe qui encapsule une valeur avec son préfixe SI
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

