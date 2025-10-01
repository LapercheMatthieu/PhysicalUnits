
using MatthL.PhysicalUnits.Core.Abstractions;
using MatthL.PhysicalUnits.Core.DimensionFormulas;
using MatthL.PhysicalUnits.Core.Enums;
using MatthL.SqliteEF.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace MatthL.PhysicalUnits.Core.Models
{
    /// <summary>
    /// Représente une unité physique 
    /// Contient une liste de BaseUnits avec leurs exposants
    /// </summary>
    public class PhysicalUnit : IBaseEntity, IBaseUnit
    {
        [Key]
        public int Id { get; set; }
        public virtual ICollection<BaseUnit> BaseUnits { get; set; } = new List<BaseUnit>();
        public UnitType UnitType { get; set; } = UnitType.Unknown_Special;

        [NotMapped]
        public string Name
        {
            get
            {
                return PhysicalUnitNameHelper.GetPhysicalUnitName(this);
            }
        }
       
        [NotMapped]
        public bool IsSI => BaseUnits.All(b => b.IsSI);

        [NotMapped]
        public StandardUnitSystem UnitSystem
        {
            get
            {
                var systems = BaseUnits.Select(b => b.UnitSystem).Distinct().ToList();
                if (systems.Count == 1) return systems[0];
                return StandardUnitSystem.Mixed;
            }
        }
        [NotMapped]
        public string DimensionalFormula => CalculateDimensionalFormula();

        public PhysicalUnit() { }

        public PhysicalUnit(BaseUnit baseUnit)
        {
            BaseUnits.Add(baseUnit);
            UnitType = baseUnit.UnitType;
        }
        public PhysicalUnit(PhysicalUnit CopyUnit)
        {
            if (CopyUnit == null) return;
            UnitType = CopyUnit.UnitType;
            if(CopyUnit != null && CopyUnit.BaseUnits != null)
            {
                BaseUnits = CopyUnit.BaseUnits;
            }
            
        }
        public PhysicalUnit(PhysicalUnit CopyUnit, Prefix prefix)
        {
            BaseUnits = CopyUnit.BaseUnits;
            UnitType = CopyUnit.UnitType;
            BaseUnits.First().Prefix = prefix;
        }
        public static PhysicalUnit Clone(PhysicalUnit CopyUnit)
        {
            if (CopyUnit == null) return new PhysicalUnit();
            var result = new PhysicalUnit()
            {
                UnitType = CopyUnit.UnitType,
                
            };
            // Cloner les RawUnits
            foreach (var unit in CopyUnit.BaseUnits)
            {
                result.BaseUnits.Add(BaseUnit.Clone(unit));
            }
            return result;
        }

        private string CalculateDimensionalFormula()
        {
            return DimensionalFormulaHelper.GetFormulaString(this);
        }

        public override string ToString() => PhysicalUnitNameHelper.GetPhysicalUnitSymbol(this);

        public void ConfigureEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PhysicalUnit>(builder =>
            {
                builder.ToTable("PhysicalUnits");
                builder.HasKey(e => e.Id);

                // Relation avec Sensors (Many-to-Many)
                builder.HasMany(e => e.BaseUnits)
                                .WithOne(r => r.PhysicalUnit)
                                .HasForeignKey(r => r.PhysicalUnitId)
                                .OnDelete(DeleteBehavior.Cascade);
            });
        }


    }
}
