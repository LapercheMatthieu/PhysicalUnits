namespace MatthL.PhysicalUnits.Core.EnumHelpers
{
    /// <summary>
    /// Attribut to specify a display name for the enums
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class DisplayedNameAttribute : Attribute
    {
        public string DisplayedName { get; }

        public DisplayedNameAttribute(string displayName)
        {
            DisplayedName = displayName;
        }
    }
}