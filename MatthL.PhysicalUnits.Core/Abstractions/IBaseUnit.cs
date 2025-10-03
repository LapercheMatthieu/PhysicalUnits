using MatthL.PhysicalUnits.Core.Enums;

namespace MatthL.PhysicalUnits.Core.Abstractions
{
    public interface IBaseUnit
    {
        public int Id { get; set; }
        public UnitType UnitType { get; }
        public string Name { get; }
        public bool IsSI { get; }
        public StandardUnitSystem UnitSystem { get; }
    }
}