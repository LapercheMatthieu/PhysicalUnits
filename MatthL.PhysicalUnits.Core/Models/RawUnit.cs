using Fractions;
using MatthL.PhysicalUnits.Core.EnumHelpers;
using MatthL.PhysicalUnits.Core.Enums;
using MatthL.PhysicalUnits.Core.Tools;
using MatthL.SqliteEF.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MatthL.PhysicalUnits.Core.Models
{
    /// <summary>
    /// Represent a pure PhysicalDimension with its exponent
    /// Ex: Length^1, Time^-2
    /// </summary>
    public class RawUnit : IBaseEntity
    {
        [Key]
        public int Id { get; set; }

        public BaseUnitType UnitType { get; set; } // Type de dimension de base (length, mass, time, etc.)

        #region EXPONENT

        // Stockage des Fractions comme int
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

        #endregion EXPONENT

        [NotMapped] public string Symbol => UnitType.GetBaseSymbol();

        public int BaseUnitId { get; set; }

        [ForeignKey("BaseUnitId")]
        public BaseUnit BaseUnit { get; set; }

        public RawUnit()     { }
        public RawUnit(BaseUnitType unitType, int exponent)
        {
            UnitType = unitType;
            Exponent = exponent;
        }

        public RawUnit(BaseUnitType unitType, Fraction exponent)
        {
            UnitType = unitType;
            Exponent = exponent;
        }

        public RawUnit Power(Fraction PowerOf)
        {
            return new RawUnit(UnitType, Exponent * PowerOf);
        }

        /// <summary>
        /// Clone avec nouvel exposant
        /// </summary>
        public RawUnit WithExponent(int newExponent)
        {
            return new RawUnit(UnitType, newExponent);
        }

        /// <summary>
        /// Représentation textuelle
        /// </summary>
        public override string ToString()
        {
            return EquationToStringHelper.FormatWithExponent(Symbol, Exponent);
        }

        public override bool Equals(object obj)
        {
            if (obj is RawUnit other)
                return UnitType == other.UnitType && Exponent == other.Exponent;
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(UnitType, Exponent);
        }

        public void ConfigureEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RawUnit>(builder =>
            {
                builder.ToTable("RawUnits");
                builder.HasKey(e => e.Id);
                builder.Property(e => e.UnitType).IsRequired();

                // Relation avec Sensors (Many-to-Many)
                builder.HasOne(e => e.BaseUnit)
                                .WithMany(b => b.RawUnits)
                                .HasForeignKey(e => e.BaseUnitId)
                                .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}