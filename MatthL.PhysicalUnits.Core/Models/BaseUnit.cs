using Fractions;
using MatthL.PhysicalUnits.Core.Enums;
using MatthL.SqliteEF.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using MatthL.PhysicalUnits.Core.EnumHelpers;
using MatthL.PhysicalUnits.Core.Abstractions;
using MatthL.PhysicalUnits.Core.Formulas;

namespace MatthL.PhysicalUnits.Core.Models
{
    /// <summary>
    /// Represent a base unit (Newton, meter etc...) with its dimensions
    /// Content the rawunits, prefix, exponent and conversion factors 
    /// </summary>
    public class BaseUnit : IBaseEntity, IBaseUnit
    {
        [Key]
        public int Id { get; set; }
        public UnitType UnitType { get; set; } //Force_Mech
        public StandardUnitSystem UnitSystem { get; set; } //Metric
        public string Name { get; set; } // Newton
        public string Symbol { get; set; } = string.Empty; //Type N 
        public virtual ICollection<RawUnit> RawUnits { get; set; } = new List<RawUnit>(); //The elementary composition kg m/s²
        public Prefix Prefix { get; set; } 
        public bool IsSI { get; set; }
        public double Offset { get; set; } //for offset relations like Kelvin and degrees
        public int PhysicalUnitId { get; set; }
        [ForeignKey("PhysicalUnitId")] public PhysicalUnit PhysicalUnit { get; set; }

        #region EXPONENT
        // Exponent stored as a fraction
        public int Exponent_Numerator { get; set; } = 1;
        public int Exponent_Denominator { get; set; } = 1;

        [NotMapped]
        public Fraction Exponent
        {
            get => new Fraction(Exponent_Numerator, Exponent_Denominator);
            set
            {
                // Vérification que ça rentre dans un int
                if (value.Numerator > int.MaxValue || value.Numerator < int.MinValue)
                    throw new OverflowException($"Numerator {value.Numerator} exceeds int range");
                if (value.Denominator > int.MaxValue || value.Denominator < int.MinValue)
                    throw new OverflowException($"Denominator {value.Denominator} exceeds int range");

                Exponent_Numerator = (int)value.Numerator;
                Exponent_Denominator = (int)value.Denominator;
            }
        }

        #endregion

        #region ConversionFactor
        // Conversion factor as a fraction
        public string ConversionFactor_Numerator { get; set; } = "1";
        public string ConversionFactor_Denominator { get; set; } = "1";

        [NotMapped]
        public Fraction ConversionFactor
        {
            get => new Fraction(BigInteger.Parse(ConversionFactor_Numerator), BigInteger.Parse(ConversionFactor_Denominator));
            set
            {
                ConversionFactor_Numerator = value.Numerator.ToString();
                ConversionFactor_Denominator = value.Denominator.ToString();
            }
        }

        #endregion


        [NotMapped] public PhysicalUnitDomain Domain => UnitType.GetDomain();
        [NotMapped] public string DimensionalFormula => CalculateDimensionalFormula();
        [NotMapped] public string PrefixedSymbol => Prefix.GetSymbol() + Symbol;
        [NotMapped] public string PrefixedName => Prefix.GetName() + Name;

        public BaseUnit() { }

        public static BaseUnit Clone(BaseUnit unit)
        {
            var cloned = new BaseUnit()
            {
                ConversionFactor = unit.ConversionFactor,
                Exponent = unit.Exponent,
                IsSI = unit.IsSI,
                Name = unit.Name,
                Offset = unit.Offset,
                Prefix = unit.Prefix,
                Symbol = unit.Symbol,
                UnitSystem = unit.UnitSystem,
                UnitType = unit.UnitType,
                // Ne pas copier PhysicalUnit et PhysicalUnitId pour éviter les références circulaires
            };

            // Cloner les RawUnits
            foreach (var rawUnit in unit.RawUnits)
            {
                cloned.RawUnits.Add(RawUnit.Clone(rawUnit));
            }

            return cloned;
        }

        /// <summary>
        /// Textual Representation
        /// </summary>
        public override string ToString()
        {
            return EquationToStringHelper.FormatWithExponent(PrefixedSymbol, Exponent);
        }

        public void ConfigureEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BaseUnit>(builder =>
            {
                builder.ToTable("BaseUnits");
                builder.HasKey(e => e.Id);
                builder.Property(e => e.UnitType).IsRequired();

                // Relation avec Sensors (Many-to-Many)
                builder.HasOne(e => e.PhysicalUnit)
                                .WithMany()
                                .HasForeignKey(e => e.PhysicalUnitId)
                                .OnDelete(DeleteBehavior.Cascade);

                // Relation avec Sensors (Many-to-Many)
                builder.HasMany(e => e.RawUnits)
                                .WithOne(r => r.BaseUnit)
                                .HasForeignKey(r => r.BaseUnitId)
                                .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
